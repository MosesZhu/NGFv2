/************************************************************************************************
**********Created by Anson Lin on 14-Jan-2006                                           *********
**********Description:                                                                  *********
*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;
using WSC.Common;
using WSC.Framework;
using System.Data;
using System.Text.RegularExpressions;

namespace WSC
{
    /// <summary>
    /// 全局定义（本地继承使用）
    /// </summary>
    public class GlobalDefinition : ParameterSetup
    {
        public static string System_Name()
        {
            string strC = "";
            try { strC = System.Configuration.ConfigurationManager.AppSettings["SYSTEM_ID"].Trim(); }
            catch
            {
                CreateLocalAppSetting("SYSTEM_ID", "");
                return "";
            }
            return strC;
        }

        public static string System_FullName()
        {
            string strFullName = "";
            try { strFullName = System.Configuration.ConfigurationManager.AppSettings["SYSTEM_NAME"].Trim(); }
            catch
            {
                CreateLocalAppSetting("SYSTEM_NAME", "");
                return "";
            }
            return strFullName;
        }

        public static string User_Information()
        {
            string strUserInfo = "";
            try { strUserInfo = System.Configuration.ConfigurationManager.AppSettings["User_Information"].Trim(); }
            catch
            {
                CreateLocalAppSetting("User_Information", "");
                return "";
            }
            return strUserInfo;
        }

        public static string System_Version()
        {
            string strVersion = "";
            try { strVersion = System.Configuration.ConfigurationManager.AppSettings["System_Version"].Trim(); }
            catch
            {
                CreateLocalAppSetting("System_Version", "");
                return "";
            }
            return strVersion;
        }

        public static string GetPageUrl()
        {
            string strPageUrl = "";
            try { strPageUrl = System.Configuration.ConfigurationManager.AppSettings["WSC_PageStart"].Trim(); }
            catch
            {
                CreateLocalAppSetting("WSC_PageStart", "");
                return "";
            }
            return strPageUrl;
        }

        public static string GetDomain()
        {
            string strDomain = "";
            try { strDomain = System.Configuration.ConfigurationManager.AppSettings["Domain"].Trim(); }
            catch
            {
                CreateLocalAppSetting("Domain", "");
                return "";
            }
            return strDomain;
        }

        public static string GetMoudleSecurity()
        {
            string strMoudleSecurity = "";
            try { strMoudleSecurity = System.Configuration.ConfigurationManager.AppSettings["MoudleSecurity"].Trim(); }
            catch
            {
                CreateLocalAppSetting("MoudleSecurity", "");
                return "";
            }
            return strMoudleSecurity;
        }

        public static string UploadFilePhysicalPath(string FormType)
        {
            string strPath = "";
            try
            {
                string strPathType = (FormType.Trim() == "" || FormType.Trim() == null) ? "" : FormType.Trim() + "\\";

                strPath = HttpRuntime.AppDomainAppPath + "Upload\\" + strPathType;

                if (!System.IO.Directory.Exists(strPath))
                    System.IO.Directory.CreateDirectory(strPath);

                return strPath;
            }
            catch { return ""; }

        }
        public static string UploadFileWebPath(string FormType)
        {
            try
            {
                string strPathType = ((FormType.Trim() == "" || FormType.Trim() == null) ? "" : FormType.Trim() + "/");

                string strWebPath = SystemWebPath + "Upload/" + strPathType;
                return strWebPath;
            }
            catch { return ""; }

        }


        public static string GetAppSettingValue(string AppSettingName)
        {
            string strC = "";
            try { strC = System.Configuration.ConfigurationManager.AppSettings[AppSettingName].Trim(); }
            catch
            {
                return "";
            }
            return strC;
        }


        internal static string CreateLocalAppSetting(string Key, string Value)
        {
            try
            {
                string[] strKeys = ConfigurationManager.AppSettings.AllKeys;
                foreach (string strKey in strKeys)
                {
                    if (strKey == Key)
                    {
                        return "EXISTS: This key[" + Key.Trim() + "] has already existed in configuration!";
                    }
                }

                string strPath = HttpRuntime.AppDomainAppPath + "web.config";

                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = strPath;

                System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                // Add an Application Setting.
                config.AppSettings.Settings.Add(Key.Trim(), Value.Trim());
                // Save the configuration file.
                config.Save(ConfigurationSaveMode.Modified);
                // Force a reload of a changed section.
                ConfigurationManager.RefreshSection("appSettings");

                return "SUCCESS";
            }
            catch (Exception ex) { return ex.Message; }
        }


        /// <summary>
        /// 当前登陆用户在FlowER中的英文名
        /// </summary>
        public static string Cookie_LoginUser
        {
            get
            {
                try
                {
                    //System.Web.UI.Page executingPage = HttpContext.Current.Handler as System.Web.UI.Page;

                    HttpContext executingPage = HttpContext.Current;
                    string SysName = System_Name();
                    string ckNameUser = SysName + "_Runtime";

                    string strRtn = "";

                    if (executingPage != null)
                    {
                        try
                        {
                            string strUserName = Security.DecryptInner(executingPage.Request.Cookies[SysName][ckNameUser].ToString().Trim());
                            strRtn = strUserName.Substring(0, strUserName.Length - 12);
                        }
                        catch
                        {
                            strRtn = "";
                        }

                        //add by AIC21/arty.yu on 20120515 for 取不到Cookie取NT
                        if (strRtn.Trim() == "")
                        {
                            string strLogin_User = executingPage.Request.ServerVariables["LOGON_USER"].Trim();

                            if (strLogin_User != "")
                            {
                                strLogin_User = strLogin_User.Substring(strLogin_User.IndexOf("\\") + 1, strLogin_User.Length - strLogin_User.IndexOf("\\") - 1);
                            }

                            strRtn = strLogin_User;
                        }
                        //end add by AIC21/arty.yu on 20120515 for 取不到Cookie取NT
                    }

                    
                    return strRtn.Trim();

                }
                catch
                {
                    return "";
                }
            }
            set
            {
                //System.Web.UI.Page executingPage = HttpContext.Current.Handler as System.Web.UI.Page;
                HttpContext executingPage = HttpContext.Current;                
                if (executingPage != null)
                {
                    executingPage.Request.Cookies.Clear();

                    HttpCookie Cookie = new HttpCookie(System_Name());

                    //解决跨站访问时取不到COOKIE值的方法 Lee Chen
                    Regex regexp = new Regex(@"(\d+)\.(\d+)\.(\d+)\.(\d+)");
                    string strHostName = HttpContext.Current.Request.Url.Host;
                    bool blHostIP = regexp.IsMatch(strHostName);
                    int intCount=strHostName.Split('.').Length;
                    if (!System.Diagnostics.Debugger.IsAttached && strHostName.ToLower() != "localhost" && !blHostIP && intCount > 2)
                    {
                        Cookie.Domain = GetDomain();
                    }

                    Cookie.Expires = new System.DateTime(2088, 11, 11);

                    string SysName = System_Name();
                    string ckNameUser = SysName + "_Runtime";
                    //string ckNameCulture = SysName + "_Culture" + value.Replace(" ", "");

                    Cookie.Values.Add(ckNameUser, Security.EncryptInner(value + ".User*#911LH"));
                    
                    executingPage.Response.Cookies.Add(Cookie);
                    
                    try { FileLogger.WriteLog_Access(); }
                    catch { }
                }
                else
                    throw new Exception(CultureRes.GetSysFrameResource("MSG_ERR_SET_CK"));
            }
        }

        /// <summary>
        /// 当前登陆用户在FlowER中的英文名
        /// </summary>
        public static string Cookie_SimulateUser
        {
            get
            {
                try
                {
                    //System.Web.UI.Page executingPage = HttpContext.Current.Handler as System.Web.UI.Page;

                    HttpContext executingPage = HttpContext.Current;
                    string SysName = System_Name() + "_Simulate";
                    string ckNameUser = SysName + "_Simulate";

                    string strRtn = "";

                    if (executingPage != null)
                    {
                        string strUserName = Security.DecryptInner(executingPage.Request.Cookies[SysName][ckNameUser].ToString().Trim());
                        strRtn = strUserName.Substring(0, strUserName.Length - 12);
                    }
                    return strRtn.Trim();

                }
                catch
                {
                    return "";
                }
            }
            set
            {
                //System.Web.UI.Page executingPage = HttpContext.Current.Handler as System.Web.UI.Page;
                HttpContext executingPage = HttpContext.Current;
                if (executingPage != null)
                {
                    //executingPage.Request.Cookies.Clear();

                    HttpCookie Cookie = new HttpCookie(System_Name() + "_Simulate");

                    //解决跨站访问时取不到COOKIE值的方法 Lee Chen
                    Regex regexp = new Regex(@"(\d+)\.(\d+)\.(\d+)\.(\d+)");
                    string strHostName = HttpContext.Current.Request.Url.Host;
                    bool blHostIP = regexp.IsMatch(strHostName);
                    int intCount = strHostName.Split('.').Length;
                    if (!System.Diagnostics.Debugger.IsAttached && strHostName.ToLower() != "localhost" && !blHostIP && intCount > 2)
                    {
                        Cookie.Domain = GetDomain();
                    }

                    Cookie.Expires = new System.DateTime(2088, 11, 11);

                    string SysName = System_Name() + "_Simulate";
                    string ckNameUser = SysName + "_Simulate";
                    //string ckNameCulture = SysName + "_Culture" + value.Replace(" ", "");

                    Cookie.Values.Add(ckNameUser, Security.EncryptInner(value + ".User*#911LH"));

                    executingPage.Response.Cookies.Add(Cookie);

                    try { FileLogger.WriteLog_Access(); }
                    catch { }
                }
                else
                    throw new Exception(CultureRes.GetSysFrameResource("MSG_ERR_SET_CK"));
            }
        }

        public static CommonEnum.Culture CurrentCulture
        {
            get
            {

                try
                {
                    return CultureRes.CurrentCulture;
                }
                catch { return CommonEnum.Culture.EN; }
            }

            set
            {
                CultureRes.CurrentCulture = value;

            }
        }


        public static string SystemWebPath
        {
            get
            {
                try
                {
                    System.Web.UI.Page executingPage = HttpContext.Current.Handler as System.Web.UI.Page;
                    if (executingPage != null)
                    {
                        string strProtocol = executingPage.Request.ServerVariables["SERVER_PROTOCOL"].Trim();
                        strProtocol = strProtocol.Substring(0, strProtocol.IndexOf("/")) + "://";

                        string strServerName = executingPage.Request.ServerVariables["SERVER_NAME"].Trim();

                        if (strServerName == "" || strServerName == null)
                            strServerName = executingPage.Request.UserHostName.Trim();

                        string strServerPort = (executingPage.Request.ServerVariables["SERVER_PORT"].Trim() == "" || executingPage.Request.ServerVariables["SERVER_PORT"].Trim() == "80") ? "" : ":" + executingPage.Request.ServerVariables["SERVER_PORT"].Trim();
                        string strApplicationPath = executingPage.Request.ApplicationPath;
                        if (strApplicationPath == "/")
                            strApplicationPath = "";
                        string strAppVirtualPath = strProtocol + strServerName + strServerPort + strApplicationPath + "/";

                        return strAppVirtualPath;
                    }
                    else
                        return "";
                }
                catch { return ""; }
            }
        }


        /// <summary>
        /// Retrieve the physical path of temp files.
        /// Read-only property.
        /// /* Add by Hedda 2008.3 */
        /// </summary>
        /// <returns></returns>
        public static string TempFilePhysicalPath
        {
            get
            {
                string strPath = "";
                try
                {
                    strPath = HttpRuntime.AppDomainAppPath + "TempFile";

                    if (!System.IO.Directory.Exists(strPath))
                        System.IO.Directory.CreateDirectory(strPath);

                    return strPath + "\\";
                }
                catch { return ""; }
            }
        }

        /// <summary>
        /// Retrieve the http web path set in Sarameter Setup of current system.
        /// Read-only property.
        /// /* Add by Hedda 2008.3 */
        /// </summary>
        /// <returns></returns>
        public static string TempFileWebPath
        {
            get
            {
                try
                {
                    string strWebPath = SystemWebPath + "TempFile/";
                    return strWebPath;
                }
                catch { return ""; }
            }

        }

        
        internal static string FlowER_WebServiceAddr
        {
            get
            {
                try
                {
                    string strR = "";
                    Framework.ParameterSetup ps = new WSC.Framework.ParameterSetup();
                    strR = ps.GetSystemValue("FLOWER_WS_PATH");
                    ps = null;
                    return strR;
                }

                catch { return ""; }
            }
        }


        public static string CurrentVersion
        {
            get
            {

                try
                {
                    string strR = "";
                    Framework.ParameterSetup ps = new WSC.Framework.ParameterSetup();
                    strR = ps.GetValue("SYS_VER");
                    ps = null;

                    return strR;
                }

                catch { return ""; }
            }
        }



        public static string LocalConnectionString
        {
            get
            {
                string strC = "";
                try { strC = System.Configuration.ConfigurationManager.AppSettings["LocalConnectionString"].Trim(); }
                catch
                {
                    CreateLocalAppSetting("LocalConnectionString", "");
                    return "";
                }
                return strC;
            }
        }




        /// <summary>
        /// 菜单是否展开（Y/N）
        /// </summary>
        public static string NavigationTree_Collapse
        {
            get
            {
                string strTC = "N";

                try
                {
                    Framework.ParameterSetup ps = new WSC.Framework.ParameterSetup();
                    strTC = ps.GetValue("COLLAPSE_TREE");

                }
                catch { }

                if (strTC == "" || (strTC != "Y" && strTC != "N")) strTC = "N";


                return strTC;
            }
        }


        internal static string LogFilePhysicalPath
        {
            get
            {
                string strPath = "";
                try
                {

                    strPath = HttpRuntime.AppDomainAppPath + "Log";

                    if (!System.IO.Directory.Exists(strPath))
                        System.IO.Directory.CreateDirectory(strPath);

                    return strPath + "\\";
                }
                catch { return ""; }
            }
        }

        public static string MailTemplatePhysicalPath
        {
            get
            {
                string strPath = "";
                try
                {
                    strPath = HttpRuntime.AppDomainAppPath + "MailTemplate";

                    if (!System.IO.Directory.Exists(strPath))
                        System.IO.Directory.CreateDirectory(strPath);

                    return strPath + "\\";
                }
                catch { return ""; }
            }
        }
        public static string WSC_ConnectionString
        {
            get
            {
                string strC = "";
                try { strC = System.Configuration.ConfigurationManager.AppSettings["wscConnectionString"].Trim(); }
                catch
                {
                    CreateLocalAppSetting("wscConnectionString", "Z2ob64x3BbkepYMbtnQhCa6DjYsmKyVt1P7FO3RHO5f8mC8tedA306LoXABvdSp+RIITF+h9JgJcBMzfenx1zs91kugq4zLizWm083FsCn991KbK/jfMNbTtG8f1oqcpklHIeSH7QCXM=");
                    return "Z2okrmMze5YgMXq6b6xbzsilzLsBbkepYMbtnQhCa6Djt1P7FO3tedA3GgqxKVJ06LoXABvdSh9JgJcBMzfenx1zs91kugq4zLizWm083FsCn991KbK/jfMNbTtG8f1oqcpklHIeSH7QCXM=";
                }
                return strC;
            }
        }
        internal static string MailService_ConnectionString
        {
            get
            {
                string strMailConn = "";

                try
                {
                    Framework.ParameterSetup ps = new WSC.Framework.ParameterSetup();
                    strMailConn = ps.GetSystemValue("MAIL_SERVICE_CONN");

                    ps = null;
                }
                catch { }

                return strMailConn;
            }
        }

        /// <summary>
        /// 显示错误页
        /// </summary>
        /// <param name="strType"></param>
        /// <param name="strTitle"></param>
        /// <param name="strMessage"></param>
        public static void RedirectError(string strType, string strTitle, string strMessage)
        {
            HttpContext.Current.Response.Redirect(GlobalDefinition.SystemWebPath + "SysFrame/Error.aspx?TYPE=" + strType.ToUpper().Trim() + "&TITLE=" + strTitle.Trim() + "&ERROR=" + strMessage.Trim());
            Logger.Instance.Error("TYPE=" + strType.ToUpper().Trim() + "&TITLE=" + strTitle.Trim() + "&ERROR=" + strMessage.Trim());
        }


        /// <summary>
        /// 自定义控件显示页数 JFK 2009616
        /// </summary>
        public static int PageSize
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["PageSize"] == null)
                {
                    return 10;
                }
                else
                {
                    return Convert.ToInt32(HttpContext.Current.Request.Cookies["PageSize"].Value);
                }
            }
            set
            {
                HttpContext.Current.Response.Cookies["PageSize"].Value = value.ToString();
            }
        }
    }
}