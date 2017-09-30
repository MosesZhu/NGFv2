/*******************************************************************************
 * Created by Anson Lin on 15-Mar-2006
********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Configuration;
using WSC.Common;
using WSC.SecurityControl;
using WSC.Framework;
using System.Text.RegularExpressions;

namespace WSC
{
    public class FramePageFV : System.Web.UI.Page
    {
        /// <summary>
        /// 框架页父类（建议所有Page继承）
        /// </summary>
        public FramePageFV()
        {

            try
            {
                double result = 0.0;
                string input = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].Trim();
                MatchCollection matchs = new Regex(@"MSIE ([0-9]{1,}[\.0-9]{0,})").Matches(input);
                if ((matchs != null) && (matchs.Count > 0))
                {
                    result = double.TryParse(matchs[0].Groups[1].Value, out result) ? result : 0.0;
                    if ((result < 6.0) && (base.Request.CurrentExecutionFilePath.IndexOf("Error.aspx", StringComparison.OrdinalIgnoreCase) == -1))
                    {
                        HttpContext.Current.Response.Redirect("~/SysFrame/Error.aspx?TYPE=ERR&TITLE=Notice&ERROR=" + CultureRes.GetSysFrameResource("MSG_ERR_IE_VER"));
                        Logger.Instance.Error(CultureRes.GetSysFrameResource("MSG_ERR_IE_VER"));

                    }
                }
                if ((GlobalDefinition.System_Name() == "") && (HttpContext.Current.Request.CurrentExecutionFilePath.IndexOf("Error.aspx", StringComparison.OrdinalIgnoreCase) == -1))
                {
                    HttpContext.Current.Response.Redirect("~/SysFrame/Error.aspx?TYPE=ERR&TITLE=Error&ERROR=" + CultureRes.GetSysFrameResource("MSG_ERR_CFG_SYSID"));
                    Logger.Instance.Error(CultureRes.GetSysFrameResource("MSG_ERR_CFG_SYSID"));
                }
            }
            catch (Exception exception)
            {
                if (HttpContext.Current.Request.CurrentExecutionFilePath.IndexOf("Error.aspx", StringComparison.OrdinalIgnoreCase) == -1)
                {
                    HttpContext.Current.Response.Redirect("~/SysFrame/Error.aspx?TYPE=ERR&TITLE=Error&ERROR=Loading!" + exception.Message);
                }
                Logger.Instance.Error(exception);
            }
        }


        private string GetBackgroundImage
        {
            get
            {
                string strFilePath = GlobalDefinition.SystemWebPath + "SysFrame/images/bg_about.jpg";
                return strFilePath;
            }
        }
        private string GetTextBoxBackgroundImage
        {
            get
            {
                string strFilePath = GlobalDefinition.SystemWebPath + "SysFrame/images/bg_input.gif";
                return strFilePath;
            }
        }



        private void SetDefaultCSS(ControlCollection vControls)
        {
            try
            {
                for (int i = 0; i < vControls.Count; i++)
                {
                    Control vControl = vControls[i];
                    string ID = vControl.ID;

                    string PType = vControl.GetType().Name;
                    switch (PType)
                    {
                        case "LiteralControl":
                            //string strText = ((LiteralControl)vControl).Text.Replace("\r", "").Replace("\n", "");
                            break;
                        case "TextBox":
                            TextBox_CSS((TextBox)vControl, ((TextBox)vControl).Enabled);
                            break;
                        case "HtmlInputText":
                            //TextBox_CSS((HtmlInputText)vControl, !((HtmlInputText)vControl).Disabled);
                            break;
                        case "Button":
                            Button_CSS((WebControl)vControl);
                            break;
                        case "HtmlInputButton":
                            //Button_CSS((HtmlControl)vControl);
                            break;
                        case "LinkButton":
                            //Button_CSS((WebControl)vControl);
                            break;
                        case "DataGrid":
                            DataGrid_CSS((DataGrid)vControl);
                            break;
                        case "GridView":
                            GridView_CSS((GridView)vControl);
                            break;
                        case "DropDownList":
                            DropDownList_CSS((DropDownList)vControl);
                            break;
                    }
                    if (vControl.Controls.Count > 0)
                        SetDefaultCSS(vControl.Controls);
                }
            }
            catch { }
        }

        private void TextBox_CSS(TextBox ctrl, bool Enabled)
        {
            if (ctrl.CssClass == "")
            {
                if (Enabled)
                    ctrl.CssClass = "EditTextBox";
                else
                    ctrl.CssClass = "ReadOnlyTextBox";
            }
        }

        private void TextBox_CSS(HtmlInputText ctrl, bool Enabled)
        {
        }

        private void Button_CSS(WebControl ctrl)
        {
            if (ctrl.CssClass == "")
                ctrl.CssClass = "ButtonLong";
        }

        private void Button_CSS(HtmlControl ctrl)
        {
        }
        private string GetTarget(WSC.Common.CommonEnum.WindowTarget Target)
        {
            string strTarget = "";
            switch (Target)
            {
                case CommonEnum.WindowTarget._top: strTarget = "_top"; break;
                case CommonEnum.WindowTarget._blank: strTarget = "_blank"; break;
                case CommonEnum.WindowTarget._self: strTarget = "_self"; break;
                case CommonEnum.WindowTarget._parent: strTarget = "_parent"; break;
                case CommonEnum.WindowTarget._media: strTarget = "_media"; break;
                case CommonEnum.WindowTarget._search: strTarget = "_search"; break;
            }
            return strTarget;
        }
        public virtual void wscOpenWindow(string Url, WSC.Common.CommonEnum.WindowTarget Target, string Features)
        {
            string strTarget = this.GetTarget(Target);

            HttpContext.Current.Response.Write("<script language='JavaScript'>" + "window.open('" + Url.Trim() + "','" + strTarget + "','" + Features.Trim() + "'); " + "</script>");
        }
        /// <summary>
        /// Only For Anthem
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="Target"></param>
        /// <param name="Features"></param>
        public virtual void wscOpenWindowCallBack(string Url, WSC.Common.CommonEnum.WindowTarget Target, string Features)
        {
            string strTarget = this.GetTarget(Target);

            Anthem.Manager.AddScriptForClientSideEval("<script language='JavaScript'>" + "window.open('" + Url.Trim() + "','" + strTarget + "','" + Features.Trim() + "'); " + "</script>");
        }


        public virtual void wscOpenWindow(string Url, WSC.Common.CommonEnum.WindowTarget Target, int Width, int Height, int Top, int Left, string Resizable, string Scrollbars)
        {
            string strTarget = this.GetTarget(Target);
            string strFeatures = "width=" + Width.ToString() + "px,height=" + Height.ToString() + "px,top=" + Top.ToString() + ",left=" + Left.ToString() + ",resizable=" + Resizable.Trim() + ",scrollbars=" + Scrollbars.Trim() + ",help=no,status=no";

            HttpContext.Current.Response.Write("<script language='JavaScript'>" + "window.open('" + Url.Trim() + "','" + strTarget + "','" + strFeatures + "'); " + "</script>");
        }


        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                if (this.Page != null)
                {

                    try
                    {
                        string strDivTip = "<DIV id=\"wscDivTipsSystem\" style=\"font-family:Times New Roman,Arial, Helvetica, sans-serif;bgcolor:lightyellow;Z-INDEX:2222;position:absolute;left:0px;visibility:hidden;border:solid 1px black;filter:progid:DXImageTransform.Microsoft.Shadow(color='#999999', Direction=135, Strength=1);top:0px;\">"
                            + "<table cellspacing=\"0\" cellpadding=\"2\" bgcolor=\"lightyellow\" border=\"0\">"
                            + "<tr><td id=\"wscTdTipsText\" style=\"font-size:9pt;font-family:Times New Roman,Verdana,Arial;font-weight: normal;\"  nowrap></td></tr>"
                            + "</table>"
                            + "</DIV>";

                        string strDivFloatForm = "<style>.WSCFloatFormCSS{ font-family:Times New Roman,Arial, Helvetica, sans-serif;width:1px;height:1px;Z-INDEX:8000;position:absolute;left:0px;top:0px;border-left:darkgray 1px solid;border-right:darkgray 1px solid;border-bottom:darkgray 1px solid;border-top:darkgray 1px solid;visibility:hidden;BACKGROUND-COLOR:White;filter:progid:DXImageTransform.Microsoft.Shadow(color='#A474BC', Direction=135, Strength=3);/*for ie6,7,8*/-moz-box-shadow:2px 2px 5px #A474BC;/*firefox*/box-shadow:2px 2px 5px #A474BC;/*opera或ie9,10*/}</style><div id=\"wscFloatForm\" class=\"WSCFloatFormCSS\" onmouseout=\"wscHideFloatForm();\">"
                            + "<iframe id=\"wscFloatDetail\" style=\"position:absolute;left:1px;top:0px;width:100%;height:100%;BACKGROUND-COLOR:White;\" scrolling=\"no\" frameborder=\"no\">"
                            + "</iframe>"
                            + "</div>";

                        //string strPagerTip = "<div id=\"div_loading\" class=\"DIVGrid\" style=\"padding-left:8;padding-right:5;" +
                        //                     "font-family:Times New Roman,Arial;font-size:10pt;Z-INDEX:9999;height:16px;position:absolute;visibility:hidden;border:solid 1px DimGray;BACKGROUND-COLOR:#ffffcc; \" >"
                        //string strPagerTip = "<div id=\"div_l\"</div>";
                        /*
                         *  loading.id = "div_loading";			
			                loading.className = "DIVGrid";
			                loading.style.fontSize = "10pt";
                            loading.style.height = "16px";            
                            loading.style.borderWidth = "1px";
                            loading.style.border = "1px";
                            loading.style.borderColor = "DimGray";
                            loading.style.borderStyle = "solid";
                            loading.style.color = "Black";
                            loading.style.backgroundColor = "#ffffcc";
                            loading.style.paddingLeft = "8px";
			                loading.style.paddingRight = "5px";
			                loading.style.position = "absolute";
			                loading.style.right = (Math.abs(document.body.getBoundingClientRect().right));// + "20px";
			                loading.style.top = (Math.abs(document.body.getBoundingClientRect().top)); //+ "20px";
			                loading.style.zIndex = "9999";       
                         */
                        if (!ClientScript.IsClientScriptBlockRegistered("__divTipFloatForm__"))
                        {
                            ClientScript.RegisterClientScriptBlock(Form.GetType(), "__divTipFloatForm__", strDivTip + strDivFloatForm);
                        }
                    }
                    catch { }

                    #region Remarked by Anson Lin on June 15,2006, for out of form region if tag '<%%>' in form region



                    #endregion

                    string pageFadeOutSetting = ConfigurationManager.AppSettings["PAGE_FADE_OUT_EFFECT_SWTICH"];
                    bool pageFadeOutOn = true;
                    if (!string.IsNullOrEmpty(pageFadeOutSetting))
                    {
                        bool.TryParse(pageFadeOutSetting, out pageFadeOutOn);
                    }
                    if (pageFadeOutOn)
                    {
                        try
                        {
                            using (HtmlGenericControl objLink = new HtmlGenericControl("meta"))
                            {
                                objLink.Attributes["http-equiv"] = "Page-Exit";
                                objLink.Attributes["content"] = "progid:DXImageTransform.Microsoft.Fade(duration=.5)";
                                this.Page.Header.Controls.Add(objLink);
                            }
                        }
                        catch
                        {

                            string strH = "<meta http-equiv=\"Page-Exit\" content=\"progid:DXImageTransform.Microsoft.Fade(duration=.5)\" />";
                            if (!ClientScript.IsClientScriptBlockRegistered("__metaExit__"))
                                ClientScript.RegisterClientScriptBlock(strH.GetType(), "__metaExit__", strH);


                        }
                    }
                    #region Remarked -- icon

                    #endregion
                    SetDefaultCSS(this.Page.Controls);
                    this.Page.ClientScript.RegisterClientScriptResource(typeof(FramePage), "WSC.Res.NewCalendar.js");
                    this.Page.ClientScript.RegisterClientScriptResource(typeof(FramePage), "WSC.Res.CommonFunction.js");
                    this.Page.ClientScript.RegisterClientScriptResource(typeof(FramePage), "WSC.Res.wscShowTips.js");
                    this.Page.MaintainScrollPositionOnPostBack = true;
                }
            }
            catch { }
            base.OnPreRender(e);
        }



        public virtual void wscCheckSecuritySysMenu()
        {
            try
            {
                string strR = WSC_Permission.CheckPermission_SysMenu();
                if (strR != "Y")
                {
                    string strUrl = GlobalDefinition.SystemWebPath + "SysFrame/Error.aspx?TYPE=INFO&TITLE=SecurityError&ERROR=" +
                        Server.HtmlEncode(Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT"));
                    Logger.Instance.Error(CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT"));
                    wscOpenWindow(strUrl, CommonEnum.WindowTarget._self, "");
                }
            }
            catch (Exception ex)
            {
                string strMsg = "~/SysFrame/Error.aspx?TYPE=INFO&TITLE=SecurityError&ERROR=CheckSecurity_Sys:"
                   + "\\n" + Server.HtmlEncode(Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT"));
                HttpContext.Current.Response.Redirect(strMsg);
                Logger.Instance.Error(ex);
            }
        }

        /// <summary>
        /// Check whether the current user is SystemAdmin.
        /// </summary>
        public bool wscCheckIsAdmin()
        {
            try
            {
                string strR = WSC_Permission.IsSysAdmin();
                return (strR == "Y");
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 检测是否具备指定的Function权限，否则直接转到错误页
        /// </summary>
        /// <param name="strFunctionID"></param>
        public virtual void wscCheckSecurity(string strFunctionID)
        {
            try
            {
                string strMod = strFunctionID.Trim();

                //Add by AIC21/arty.yu on 20120612
                WSC_Permission wsc_Permission = new WSC_Permission();
                ModuleFunction module = new ModuleFunction();
                System.Data.DataTable dt = module.GetFunctionByFunctionID(strMod);
                string sub_Id = "";
                if (dt.Rows.Count == 1) sub_Id = dt.Rows[0]["SYS_SUB_ID"].ToString().Trim();
                //End Add by AIC21/arty.yu on 20120612

                string strR = wsc_Permission.Validate(strMod, sub_Id);
                if (strR != "Y")
                {
                    #region Error will be raised while call method Redirect directly. So instead of method Window.Open

                    string strUrl = GlobalDefinition.SystemWebPath + "SysFrame/Error.aspx?TYPE=INFO&TITLE=SecurityError&ERROR=" +
                        Server.HtmlEncode(Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT"));
                    Logger.Instance.Error(CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT"));
                    this.wscOpenWindow(strUrl, CommonEnum.WindowTarget._self, "");
                    #endregion

                }
            }
            catch (Exception ex)
            {
                string strMsg = "~/SysFrame/Error.aspx?TYPE=INFO&TITLE=SecurityError&ERROR=CheckSecurity!"
                    + "\\n" + Server.HtmlEncode(Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT"));
                HttpContext.Current.Response.Redirect(strMsg);
                Logger.Instance.Error(ex);
            }
        }



        /// <summary>
        /// 等同于GlobalDefinition中定义
        /// </summary>
        public virtual string wscCurrentLogonUser
        {
            get
            {
                string strSql = "SP_WSC_SECURITY_USER_FLOWER_FROM_NT '" + this.wscCurrentNTUser.Trim() + "','" + GlobalDefinition.System_Name() + "','" + this.wscCurrentNTUser.Trim() + "'";
                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.GetValue(strSql, 0).Trim();
                }
            }
        }

        public string wscCurrentLogonPassword(string strLoginName)
        {
            string strViewQuery = "getAmEmployee('" + GlobalDefinition.System_Name() + "','" + GlobalDefinition.Cookie_LoginUser + "')";
            string strSql = "SELECT PASS_WORD FROM " + strViewQuery + " WHERE UPPER(LOGIN_NAME)='" + strLoginName.Trim().ToUpper() + "' AND ACTIVE='Y'";
            using (WSC_DataConn conn = new WSC_DataConn())
            {
                return conn.GetValue(strSql, 0).Trim();
            }
        }


        /// <summary>
        /// NT account without domain, it's not same as GlobalDefinition.Cookie_LoginUser
        /// </summary>
        public virtual string wscCurrentNTUser
        {
            get
            {
                string strLogin_User = "";

                strLogin_User = HttpContext.Current.Request.ServerVariables["LOGON_USER"].Trim();

                if (strLogin_User == "")
                    strLogin_User = User.Identity.Name.Trim();

                if (strLogin_User == "")
                    HttpContext.Current.Response.Redirect("~/SysFrame/Login.aspx");

                strLogin_User = strLogin_User.Substring(strLogin_User.IndexOf("\\") + 1, strLogin_User.Length - strLogin_User.IndexOf("\\") - 1);
                return strLogin_User;
            }
        }

        /// <summary>
        /// Domain
        /// </summary>
        public virtual string wscCurrentDomain
        {
            get
            {
                string strLogin_User = HttpContext.Current.Request.ServerVariables["LOGON_USER"].Trim();

                if (strLogin_User == "")
                    strLogin_User = User.Identity.Name.Trim();

                strLogin_User = strLogin_User.Substring(0, strLogin_User.IndexOf("\\"));
                return strLogin_User;
            }
        }

        /// <summary>
        /// NT account with domain
        /// </summary>
        public virtual string wscCurrentLogonUserFull
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["LOGON_USER"].Trim();
            }
        }



        /// <summary>
        /// Only For Anthem
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="Target"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Top"></param>
        /// <param name="Left"></param>
        /// <param name="Resizable"></param>
        /// <param name="Scrollbars"></param>
        public virtual void wscOpenWindowCallBack(string Url, WSC.Common.CommonEnum.WindowTarget Target, int Width, int Height, int Top, int Left, string Resizable, string Scrollbars)
        {
            string strTarget = this.GetTarget(Target);
            string strFeatures = "width=" + Width.ToString() + "px,height=" + Height.ToString() + "px,top=" + Top.ToString() + ",left=" + Left.ToString() + ",resizable=" + Resizable.Trim() + ",scrollbars=" + Scrollbars.Trim() + ",help=no,status=no";

            Anthem.Manager.AddScriptForClientSideEval("<script language='JavaScript'>" + "window.open('" + Url.Trim() + "','" + strTarget + "','" + strFeatures + "'); " + "</script>");
        }


        public virtual void wscOpenModalDialog(string Url, WSC.Common.CommonEnum.WindowTarget Target, string Features)
        {
            string strTarget = this.GetTarget(Target);
            HttpContext.Current.Response.Write("<script language='JavaScript'>" + "var strR=showModalDialog('" + Url.Trim() + "','" + strTarget + "','" + Features.Trim() + "'); " + "</script>");
        }


        public virtual void wscOpenModalDialogCallBack(string Url, WSC.Common.CommonEnum.WindowTarget Target, string Features)
        {
            string strTarget = this.GetTarget(Target);
            Anthem.Manager.AddScriptForClientSideEval("<script language='JavaScript'>" + "var strR=showModalDialog('" + Url.Trim() + "','" + strTarget + "','" + Features.Trim() + "'); " + "</script>");
        }


        /* Add by Hedda */
        /// <summary>
        /// 悬浮框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="detailUrl"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="hideAfterLeaving"></param>
        public void wscShowFloatForm(Control sender, string detailUrl, int width, int height, bool hideAfterLeaving)
        {
            RenderScript.RenderShowFloatFormScript(sender, detailUrl, width, height, hideAfterLeaving);
        }


        public void wscShowFloatForm(Control sender, string detailUrl, int width, int height)
        {
            this.wscShowFloatForm(sender, detailUrl, width, height, false);
        }


        private void GridView_CSS(GridView ctrl)
        {
            // Fengkan Add Start 20100515
            //if (ctrl.CssClass == "DIVGrid")
           
            ctrl.CssClass = "myGV";
            ctrl.HeaderStyle.Reset();
            ctrl.RowStyle.Reset();
            ctrl.AlternatingRowStyle.Reset();
            ctrl.BorderWidth = 1;
            ctrl.BorderStyle = BorderStyle.Solid;
            ctrl.EmptyDataRowStyle.Reset();


            if (ctrl.CssClass == "") ctrl.CssClass = "myGV";
            if (ctrl.HeaderStyle.CssClass == "") ctrl.HeaderStyle.CssClass = "GVHeader";
            if (ctrl.RowStyle.CssClass == "") ctrl.RowStyle.CssClass = "GVDataTR";
            if (ctrl.AlternatingRowStyle.CssClass == "") ctrl.AlternatingRowStyle.CssClass = "GVAlterDataTR";
            // Fengkan Add End 20100515

            // Fengkan marked start 20100515
            //if (ctrl.BorderColor.IsEmpty) ctrl.BorderColor = System.Drawing.Color.DimGray;
            //if (ctrl.BorderWidth.IsEmpty) ctrl.BorderWidth = 1;
            //if (ctrl.EmptyDataText == string.Empty) ctrl.EmptyDataText = "No data.";
            //if (ctrl.EmptyDataRowStyle.BackColor.IsEmpty) ctrl.EmptyDataRowStyle.BackColor = System.Drawing.Color.LightYellow;
            //if (ctrl.EmptyDataRowStyle.ForeColor.IsEmpty) ctrl.EmptyDataRowStyle.ForeColor = System.Drawing.Color.Black;
            //if (ctrl.EmptyDataRowStyle.Font.Size.IsEmpty) ctrl.EmptyDataRowStyle.Font.Size = 9;
            //if (ctrl.HeaderStyle.CssClass == "") ctrl.HeaderStyle.CssClass = "bg_GridHeader";
            //if (ctrl.HeaderStyle.ForeColor.IsEmpty) ctrl.HeaderStyle.ForeColor = System.Drawing.Color.White;
            //if (ctrl.AlternatingRowStyle.BackColor.IsEmpty) ctrl.AlternatingRowStyle.BackColor = System.Drawing.Color.White;
            //if (ctrl.AlternatingRowStyle.ForeColor.IsEmpty) ctrl.AlternatingRowStyle.ForeColor = System.Drawing.Color.DimGray;
            //if (ctrl.RowStyle.BackColor.IsEmpty) ctrl.RowStyle.BackColor = System.Drawing.Color.LightYellow;
            //if (ctrl.RowStyle.ForeColor.IsEmpty) ctrl.RowStyle.ForeColor = System.Drawing.Color.DimGray;
            // Fengkan marked end 20100515
        }

        public void wscShowTips(Control sender, string Message)
        {
            RenderScript.RenderShowTipsScript(sender, Message);
        }

        public string wscInitializeUploadedFiles(string ControlID, string UploadType, string ID)
        {
            return UploadFile.InitializeUploadedFiles(ControlID, UploadType, ID);
        }
        public virtual void wscOpenModalDialog(string Url, WSC.Common.CommonEnum.WindowTarget Target, int Width, int Height)
        {
            string strTarget = this.GetTarget(Target);
            if (Width <= 0) Width = 445;
            if (Height <= 0) Height = 236;

            string strFeatures = "dialogWidth=" + Width.ToString() + "px,dialogHeight=" + Height.ToString() + ",center=yes;help=no;status=no";

            HttpContext.Current.Response.Write("<script language='JavaScript'>" + "window.open('" + Url.Trim() + "','" + strTarget + "','" + strFeatures + "'); " + "</script>");
        }

        public virtual void wscOpenModalDialogCallBack(string Url, WSC.Common.CommonEnum.WindowTarget Target, int Width, int Height)
        {
            string strTarget = this.GetTarget(Target);
            if (Width <= 0) Width = 445;
            if (Height <= 0) Height = 236;

            string strFeatures = "dialogWidth=" + Width.ToString() + "px,dialogHeight=" + Height.ToString() + ",center=yes;help=no;status=no";

            Anthem.Manager.AddScriptForClientSideEval("<script language='JavaScript'>" + "window.open('" + Url.Trim() + "','" + strTarget + "','" + strFeatures + "'); " + "</script>");
        }


        /// <summary>
        /// 等同于MessageBox.Show
        /// </summary>
        /// <param name="msg"></param>
        public void wscShowMsg(string msg)
        {
            MessageBox.Show(msg);
        }

        /// <summary>
        /// Only For Anthem
        /// </summary>
        /// <param name="msg"></param>
        public void wscShowMsgCB(string msg)
        {
            MessageBox.ShowCB(msg);
        }


        public virtual void wscAddScriptForCallBack(string script)
        {

            Anthem.Manager.AddScriptForClientSideEval(script.Trim());
        }



        public string wscGetRequestQueryString
        {
            get
            {
                string strReturn = "";
                for (int intCount = 0; intCount < HttpContext.Current.Request.QueryString.Count; intCount++)
                {
                    if (intCount > 0)
                        strReturn += ("&" + HttpContext.Current.Request.QueryString.GetKey(intCount) + "=" + HttpContext.Current.Request.QueryString[intCount]);
                    else
                        strReturn += (HttpContext.Current.Request.QueryString.GetKey(intCount) + "=" + HttpContext.Current.Request.QueryString[intCount]);
                }
                return strReturn;
            }
        }
        private void DropDownList_CSS(DropDownList ctrl)
        {
            if (ctrl.CssClass == "")
                ctrl.CssClass = "DropDownList";

        }

        private void DataGrid_CSS(DataGrid ctrl)
        {
            if (ctrl.CssClass == "") ctrl.CssClass = "DIVGrid";
            if (ctrl.BorderColor.IsEmpty) ctrl.BorderColor = System.Drawing.Color.Gray;
            if (ctrl.BorderWidth.IsEmpty) ctrl.BorderWidth = 1;
            if (ctrl.HeaderStyle.CssClass == "") ctrl.HeaderStyle.CssClass = "bg_GridHeader";
            ctrl.HeaderStyle.BorderStyle = BorderStyle.None;
            if (ctrl.HeaderStyle.ForeColor.IsEmpty) ctrl.HeaderStyle.ForeColor = System.Drawing.Color.White;
            if (ctrl.AlternatingItemStyle.BackColor.IsEmpty) ctrl.AlternatingItemStyle.BackColor = System.Drawing.Color.White;
            if (ctrl.AlternatingItemStyle.ForeColor.IsEmpty) ctrl.AlternatingItemStyle.ForeColor = System.Drawing.Color.DimGray;
            if (ctrl.ItemStyle.BackColor.IsEmpty) ctrl.ItemStyle.BackColor = System.Drawing.Color.LightYellow;
            if (ctrl.ItemStyle.ForeColor.IsEmpty) ctrl.ItemStyle.ForeColor = System.Drawing.Color.DimGray;

        }

        /// <summary>
        /// 检测用户是否具备当前页权限，否则直接跳转到错误页
        /// </summary>
        public virtual void wscCheckSecurity()
        {
            try
            {
                string strMod = "";
                try
                {
                    strMod = HttpContext.Current.Request["AuthModuleID"].Trim();
                }
                catch { }

                //Add by AIC21/arty.yu on 20120612
                WSC_Permission wsc_Permission = new WSC_Permission();
                ModuleFunction module = new ModuleFunction();
                System.Data.DataTable dt = module.GetFunctionByFunctionID(strMod);
                string sub_Id = "";
                if (dt.Rows.Count == 1) sub_Id = dt.Rows[0]["SYS_SUB_ID"].ToString().Trim();
                //End Add by AIC21/arty.yu on 20120612

                string strR = wsc_Permission.Validate(strMod, sub_Id);
                if (strR != "Y")
                {
                    #region Error will be raised while call method Redirect directly. So instead of method Window.Open

                    string strUrl = GlobalDefinition.SystemWebPath + "SysFrame/Error.aspx?TYPE=INFO&TITLE=SecurityError&ERROR=" +
                        Server.HtmlEncode(Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT"));
                    Logger.Instance.Error(CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT"));
                    this.wscOpenWindow(strUrl, CommonEnum.WindowTarget._self, "");
                    #endregion
                }
            }
            catch (Exception ex)
            {
                string strMsg = "~/SysFrame/Error.aspx?TYPE=INFO&TITLE=SecurityError&ERROR=CheckSecurity!"
                    + "\\n" + Server.HtmlEncode(Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT"));
                HttpContext.Current.Response.Redirect(strMsg);
                Logger.Instance.Error(ex);
            }
        }

        /// <summary>
        /// 检测用户是否具备当前页权限
        /// Add by Hedda 2007.3
        /// </summary>
        /// <param name="strFunctionID"></param>
        /// <param name="inputNull"></param>
        /// <returns></returns>
        public virtual bool wscCheckSecurity(string strFunctionID, object inputNull)
        {
            try
            {
                string strMod = strFunctionID.Trim();

                //Add by AIC21/arty.yu on 20120612
                WSC_Permission wsc_Permission = new WSC_Permission();
                ModuleFunction module = new ModuleFunction();
                System.Data.DataTable dt = module.GetFunctionByFunctionID(strMod);
                string sub_Id = "";
                if (dt.Rows.Count == 1) sub_Id = dt.Rows[0]["SYS_SUB_ID"].ToString().Trim();
                //End Add by AIC21/arty.yu on 20120612

                string strR = wsc_Permission.Validate(strMod, sub_Id);
                return (strR == "Y");
            }
            catch
            {
                return false;
            }
        }

        private Style CssButton
        {
            get
            {
                Style css = new Style();

                //css.AddAttributesToRender(
                return css;
            }
        }

        //实现Culture不彻底 by Hedda 200902
        //protected override void InitializeCulture()
        //{
        //    this.Culture=new System.Globalization.CultureInfo(
        //    base.InitializeCulture();
        //}
    }
}


