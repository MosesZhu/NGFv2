using ITS.WebFramework.Configuration;
using ITS.WebFramework.PermissionComponent.ServiceProxy;
using ITS.WebFramework.SSO.Business;
using ITS.WebFramework.SSO.Common;
using NGF.Base.Base;
using NGF.Base.Config;
using NGF.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using static NGF.Web.LoginService;

namespace NGF.Web
{
    public partial class Login : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (NGFConfig.SystemMode == NGFSystemModeEnum.Single 
                    || NGFConfig.NGFAuthMode == NGFAuthModeEnum.WSC
                    || NGFConfig.NGFCleanVersion == true)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "SwitchToSingleMode", @"_Context.SystemMode = 'S'; $('#ddlProduct').parents('tr').hide();
$('#ddlOrg').parents('tr').hide();", true);

                    string systemId = "00000000-0000-0000-0000-000000000001";
                    if (NGFConfig.NGFCleanVersion == false)
                    {
                        systemId = NGFConfig.NGFSystemId;
                        PermissionService permissionService = new PermissionService();
                        permissionService.Url = Config.Global.PermissionServiceUrl;
                        SystemDTO systemInfo = permissionService.GetSystemInfo(Guid.Parse(systemId));
                        DomainDTO domain = permissionService.GetDomainInfo(systemInfo.Domain_Id);
                        Page.ClientScript.RegisterStartupScript(GetType(), "SetSingleModeInfo", @"SingleModeProductId = '" + systemInfo.Product_Id
                        + "'; SingleModeOrgId = '" + systemInfo.Org_Id + "'; SingleModeDomain = '" + domain.Name + "';", true);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "SetSingleModeInfo", @"SingleModeProductId = '" 
                            + systemId
                            + "'; SingleModeOrgId = '" + systemId 
                            + "'; SingleModeDomain = '" + "Global" + "';", true);
                    }                    
                    if (NGFConfig.NGFAuthMode == NGFAuthModeEnum.WSC) {
                        Page.ClientScript.RegisterStartupScript(GetType(), "SwitchToWSCMode", @"_Context.AuthMode = 'WSC';$('#areaNT').hide();
$('#rowDomain').hide();", true);
                    }

                    if (NGFConfig.NGFCleanVersion == true)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "SwitchToCleanVersion", @"_Context.IsCleanVersion=true;$('#rowProduct').hide();
$('#rowOrg').hide();", true);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "SwitchToCleanVersion", @"$('#rowProduct').show();
$('#rowOrg').show();", true);
                    }
                }                

                InitializeSSORequest();

                if (_SSORequest != null)
                {
                    if (_SSORequest.LoginType == LoginTypeEnum.Debug)
                    {
                        string[] datas = _SSORequest.Data.Split(',');
                        if (datas.Length >= 5)
                        {
                            if (!string.IsNullOrWhiteSpace(datas[2]) && !string.IsNullOrWhiteSpace(datas[3]))
                            {
                                string productName = datas[2];
                                string orgName = datas[3];
                                string userName = datas[4];
                                LoginService service = new LoginService();

                                List<SimpleProductOrgDTO> productOrgList = (List<SimpleProductOrgDTO>)service.getProductOrgList().data;
                                SimpleProductOrgDTO product = productOrgList.FirstOrDefault(p => p.Name.Equals(productName, StringComparison.CurrentCultureIgnoreCase));
                                if (product != null)
                                {
                                    OrgDTO org = product.OrgList.FirstOrDefault(o => o.Name.Equals(orgName, StringComparison.CurrentCultureIgnoreCase));
                                    if (org != null)
                                    {
                                        LogonInfo logonInfo = new LogonInfo();
                                        logonInfo.SSORequest = _SSORequest;
                                        logonInfo.IsNT = true;
                                        logonInfo.OrgID = org.Id;
                                        logonInfo.OrgName = orgName;
                                        logonInfo.ProductID = product.Id;
                                        logonInfo.ProductName = productName;
                                        logonInfo.UserName = userName;
                                        logonInfo.Language = "zh-CN";
                                        service.wfkLoginForDebug(logonInfo);
                                    }
                                }
                            }
                        }
                    }
                    else if (_SSORequest.LoginType == LoginTypeEnum.AdminSimulate)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "HidePassword", @"$('#tbxPassword').parents('tr').hide();
$('#ddlProduct').parents('tr').hide();
$('#ddlOrg').parents('tr').hide();
$('#ddlDomain').parents('tr').hide();", true);
                    }
                }

                if (NGFConfig.SystemMode == NGFSystemModeEnum.Single)
                {
                    lblSystemName.Text = NGFConfig.NGFSystemName;
                    lblSystemName.Attributes["lang"] = "";
                }

                if (NGFConfig.NGFEnvironmentVisible)
                {
                    this.textEnvironmentInfo.Text = "(" + NGFConfig.NGFEnvironment + ")";
                }

                if (NGFConfig.NGFNtAuth &&
                    !(Request.QueryString.Get("action") != null && Request.QueryString.Get("action").Equals("logout", StringComparison.CurrentCultureIgnoreCase)))
                {
                    NtLogin();
                    //Page.ClientScript.RegisterStartupScript(GetType(), "Test", @"alert('" + name + "')", true);
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

        }

        override
        public List<string> GetFormMultiLanguageKeyList() 
        {
            List<string> keyList = new List<string>();
            keyList.Add("lang_error");
            keyList.Add("lang_msg_login_failed");
            keyList.Add("lang_msg_must_input_login_name");
            keyList.Add("lang_msg_must_choose_org");
            return keyList;
        }

        private SSORequest _SSORequest
        {
            get
            {
                if (Session == null)
                {
                    SSORequest request = new SSORequest();
                    request.LoginType = LoginTypeEnum.Normal;
                    request.ReturnUrl = HttpContext.Current.Request.Url.ToString().Substring(0, HttpContext.Current.Request.Url.ToString().ToLower().IndexOf("loginservice.asmx")) + "Portal";
                    return request;
                }

                object obj = Session["SSORequest"];
                if (obj != null)
                    return (SSORequest)obj;
                else
                {
                    SSORequest request = new SSORequest();
                    request.LoginType = LoginTypeEnum.Normal;
                    return request;
                }
            }
            set
            {
                Session["SSORequest"] = value;
            }
        }

        SSOHelper _SSOHelper = new SSOHelper();
        private SSOTicket GetSSOTicketFromCookie()
        {
            SSOTicket ssoTicket = _SSOHelper.LoadSSOTicket(Config.Global.SSOTicketName);
            return ssoTicket;
        }

        private void InitializeSSORequest()
        {
            string encrypedSSORequest = Request.QueryString["SSORequest"];
            string encryptedSSOTicket = Request.QueryString["SSOTicket"];

            if (!string.IsNullOrEmpty(encryptedSSOTicket))
            {
                try
                {
                    SSOTicket ssoTicket = _SSOHelper.DecryptSSOTicket(encryptedSSOTicket);
                    _SSOHelper.SaveSSOTicket(ssoTicket);
                    //_SSOAuth.RedirectToOnSuccessUrl(ssoTicket, _SSOAuth.GetSSOPortalUrl(ssoTicket));
                }
                catch (System.Exception ex)
                {
                    Response.Write(ex.Message);
                }
                Response.End();
            }

            if (!string.IsNullOrEmpty(encrypedSSORequest))
            {
                _SSORequest = _SSOHelper.DecryptSSORequest(encrypedSSORequest);
            }
            else
            {
                _SSORequest = null;
            }

            if (_SSORequest == null
                && !string.IsNullOrEmpty(Request.QueryString["FromExternalSystemCall"]))
            {
                _SSORequest = new SSORequest();
                _SSORequest.LoginType = LoginTypeEnum.AutoLogon;
                _SSORequest.RequestDate = DateTime.UtcNow;
                if (Request.UrlReferrer != null)
                {
                    _SSORequest.ReturnUrl = Request.UrlReferrer.ToString();
                    //_SSORequest.ReturnUrl = "http://aic0-s2.qcs.qcorp.com/PermissionManagement/OrgUser/Department/DepartmentInquiry.aspx";
                }
            }

            if (_SSORequest == null
                || _SSORequest.LoginType == LoginTypeEnum.Logout)
            {
                _SSORequest = new SSORequest();
                _SSORequest.LoginType = LoginTypeEnum.DirectLogin;
                _SSORequest.RequestDate = DateTime.UtcNow;
                _SSORequest.ReturnUrl = "";
            }

        }

        private void NtLogin()
        {
            string id = HttpContext.Current.User.Identity.Name;
            string domain = id.Split('\\')[0];
            string userName = id.Split('\\')[1];
            string productName = NGFConfig.NGFNtProduct;
            string orgName = NGFConfig.NGFNtOrg;
            LoginService service = new LoginService();
            List<SimpleProductOrgDTO> allProductOrg = (List<SimpleProductOrgDTO>)service.getProductOrgList().data;
            SimpleProductOrgDTO product = allProductOrg.FirstOrDefault(p => p.Name.Equals(productName, StringComparison.CurrentCultureIgnoreCase));
            if (product != null)
            {
                OrgDTO org = product.OrgList.FirstOrDefault(o => o.Name.Equals(orgName, StringComparison.CurrentCultureIgnoreCase));
                if (org != null)
                {
                    wfkLogin(userName, product.Id, product.Name, 
                        org.Id, org.Name, domain, true, "en-US");
                }
            }
        }

        public void wfkLogin(string userName, Guid productId, string productName, Guid orgId, string orgName, string domain, bool isInternal, string language)
        {
            SSOTicket ssoTicket = GetSSOTicketFromCookie();

            LogonInfo logonInfo = new LogonInfo();
            logonInfo.SSORequest = _SSORequest;
            logonInfo.IsNT = isInternal;
            logonInfo.OrgID = orgId;
            logonInfo.OrgName = orgName;
            logonInfo.ProductID = productId;
            logonInfo.ProductName = productName;

            logonInfo.UserName = userName;

            if (ssoTicket == null
                && _SSORequest.LoginType != LoginTypeEnum.AdminSimulate
                && _SSORequest.LoginType != LoginTypeEnum.Debug)
            {
                //logonInfo.Password = password;
                if (logonInfo.IsNT)
                {
                    logonInfo.Domain = domain;
                }
            }
            else
            {
                logonInfo.IsSSOTicketAleadyExisted = true;
            }
            logonInfo.Language = language;
            logonInfo.AutoLogon = true;

            try
            {
                string url = new SSOAuthentication().LogonWithPortalUrl(logonInfo);
                string queryStr = url.Split('?')[1];
                string portalUrl = "Portal?" + queryStr;
                Response.Redirect(portalUrl);
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlProductServer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}