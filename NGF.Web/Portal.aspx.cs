using System;
using System.Collections.Generic;
using System.Web.UI;
using NGF.Base.Config;
using NGF.Base.Base;
using NGF.Base.Enums;

namespace NGF.Web
{
    public partial class Portal : PageBase
    {
        public override List<string> GetFormMultiLanguageKeyList()
        {
            List<string> _multyKeyArray = new List<string>();
            _multyKeyArray.Add("msg_confirm_close_tab");
            _multyKeyArray.Add("msg_confirm_close_window");
            _multyKeyArray.Add("lang_success");
            _multyKeyArray.Add("msg_save_success");
            _multyKeyArray.Add("lang_favorites");
            _multyKeyArray.Add("lang_message");
            _multyKeyArray.Add("lang_change_password");
            _multyKeyArray.Add("lang_new_password");
            _multyKeyArray.Add("lang_old_password");
            _multyKeyArray.Add("lang_confirm_password");
            _multyKeyArray.Add("msg_change_pwd_successed");
            _multyKeyArray.Add("msg_change_pwd_failed");
            return _multyKeyArray;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = NGFConfig.NGFPortalTitle;

            string requestDomain = Request.QueryString["domain"];
            if (string.IsNullOrEmpty(requestDomain))
            {
                requestDomain = NGFConfig.NGFCleanVersionDefaultDomain;
            }

            if (!string.IsNullOrEmpty(NGFConfig.NGFPortalHeaderInfo))
            {
                this.textHeaderInfo.Text = NGFConfig.NGFPortalHeaderInfo;
            }

            if (NGFConfig.NGFCleanVersion)
            {
                if (!string.IsNullOrEmpty(requestDomain))
                {
                    this.textHeaderInfo.Text = requestDomain;
                    this.Title = requestDomain;
                }                
            }            

            if (NGFConfig.NGFEnvironmentVisible)
            {
                this.textEnvironmentInfo.Text = "(" + NGFConfig.NGFEnvironment + ")";
            }

            

            if (!string.IsNullOrEmpty(NGFConfig.NGFPortalFooterInfo))
            {
                this.textFooterInfo.Text = NGFConfig.NGFPortalFooterInfo;
            }
            
            if (!Page.IsPostBack)
            {
                if (NGFConfig.NGFAuthMode == NGFAuthModeEnum.WSC)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "SwitchToWSCMode", @"_Context.AuthMode = 'WSC';", true);                            
                }

                if (NGFConfig.NGFCleanVersion)
                {                    
                    if (!string.IsNullOrEmpty(requestDomain))
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "SwitchToCleanVersion", @"_PortalContext.IsCleanVersion = true;_PortalContext.CleanVersionDomain = '" + requestDomain + "';", true);
                    }
                }
            }
        }
    }
}