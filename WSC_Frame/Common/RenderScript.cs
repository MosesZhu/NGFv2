// Created by Anson Lin on 15-Feb-2005
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace WSC.Common
{
    /// <summary>
    /// Render the javascrpit to current page.    
    /// </summary>
    public sealed class RenderScript
    {
        /// <summary>
        ///  Bind javascript to control.
        /// </summary>
        /// <returns></returns>
        public static void RenderCustomeScript(IAttributeAccessor control, string clientEventName, string script)
        {
            try
            {
                if (control == null)
                {
                    return;
                }
                control.SetAttribute(clientEventName, script);
            }
            catch { }
        }

        #region Delete confirm
        /// <summary>
        ///  Bind built-in javascript confirm function to specific control.
        ///  The message is "Are you sure to delete the selected item(s)"
        /// </summary>
        /// <param name="control">The control you want render script.</param>
        /// <returns></returns>
        public static void RenderDeleteButtonConfirm(Control control)
        {
            try
            {
                //string Message = "Are you sure to delete the selected item(s)?";           
                string Message = CultureRes.GetSysFrameResource("MSG_TIP_DELETE_CONFIRM");

                if (control == null)
                {
                    return;
                }
                Type t = control.GetType();
                switch (t.Namespace)
                {
                    case "Anthem":
                    case "System.Web.UI.WebControls":
                        ((WebControl)control).Attributes.Add("onclick", " return confirm('" + Message + "');");
                        break;
                    case "System.Web.UI.HtmlControls":
                        ((HtmlControl)control).Attributes.Add("onclick", " return confirm('" + Message + "');");
                        break;
                }
            }
            catch { }
        }

        /// <summary>
        ///  Bind built-in javascript confirm function to specific control.Executing additional scrpt before render Confirm script.
        ///  The message is "Are you sure to delete the selected item(s)"
        /// </summary>
        /// <param name="control">Button</param>
        /// <param name="PrevAdditionalScript">Executing additional scrpt before render Confirm script.</param>
        /// <returns></returns>
        public static void RenderDeleteButtonConfirm(Control control, string PrevAdditionalScript)
        {
            try
            {
                if (control == null)
                {
                    return;
                }
                Type t = control.GetType();
                switch (t.Namespace)
                {
                    case "Anthem":
                    case "System.Web.UI.WebControls":
                        ((WebControl)control).Attributes.Add("onclick", PrevAdditionalScript + " return confirm('Are you sure to delete the selected item(s)?');");
                        break;
                    case "System.Web.UI.HtmlControls":
                        ((HtmlControl)control).Attributes.Add("onclick", PrevAdditionalScript + " return confirm('Are you sure to delete the selected item(s)?');");
                        break;
                }
            }
            catch { }
        }
        #endregion

        #region Confirm
        /// <summary>
        ///  Bind built-in javascript confirm function to specific control.
        /// </summary>
        /// <param name="control">The control you want render script.</param>
        /// <param name="Message">The message to show.</param>
        /// <returns></returns>
        public static void RenderButtonConfirm(Control control, string Message)
        {
            if (Message.Trim() == "" || Message == null)
                Message = CultureRes.GetSysFrameResource("MSG_TIP_DELETE_CONFIRM");

            try
            {
                if (control == null)
                {
                    return;
                }
                Type t = control.GetType();
                switch (t.Namespace)
                {
                    case "Anthem":
                    case "System.Web.UI.WebControls":
                        ((WebControl)control).Attributes.Add("onclick", " return confirm('" + Message + "');");
                        break;
                    case "System.Web.UI.HtmlControls":
                        ((HtmlControl)control).Attributes.Add("onclick", " return confirm('" + Message + "');");
                        break;
                }
            }
            catch { }
        }

        /// <summary>
        ///  Bind built-in javascript confirm function to specific control.Executing additional scrpt before render Confirm script.
        /// </summary>
        /// <param name="control">Button</param>
        /// <param name="Message">The message to show.</param>
        /// <param name="PrevAdditionalScript">Executing additional scrpt before render Confirm script.</param>
        /// <returns></returns>
        public static void RenderButtonConfirm(Control control, string Message, string PrevAdditionalScript)
        {
            if (Message.Trim() == "" || Message == null)
                Message = CultureRes.GetSysFrameResource("MSG_TIP_DELETE_CONFIRM");

            try
            {
                if (control == null)
                {
                    return;
                }
                Type t = control.GetType();
                switch (t.Namespace)
                {
                    case "Anthem":
                    case "System.Web.UI.WebControls":
                        ((WebControl)control).Attributes.Add("onclick", PrevAdditionalScript + " return confirm('" + Message + "');");
                        break;
                    case "System.Web.UI.HtmlControls":
                        ((HtmlControl)control).Attributes.Add("onclick", PrevAdditionalScript + " return confirm('" + Message + "');");
                        break;
                }
            }
            catch { }
        }
        #endregion

        #region Modal Dialog

        /// <summary>
        ///  Bind built-in javascript ShowModalDialog function to specific control.
        /// </summary>
        /// <param name="control">Button ID</param>
        /// <param name="Features"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static void RenderModalDialogToControl(Control control, string Url, string Features)
        {
            try
            {
                if (control == null)
                {
                    return;
                }
                Type t = control.GetType();
                switch (t.Namespace)
                {
                    case "Anthem":
                    case "System.Web.UI.WebControls":
                        ((WebControl)control).Attributes.Add("onclick", "var strR=showModalDialog('" + Url + "','_blank','" + Features + "'); if(strR==\"SAVE\") document.location.reload();");
                        break;
                    case "System.Web.UI.HtmlControls":
                        ((HtmlControl)control).Attributes.Add("onclick", "var strR=showModalDialog('" + Url + "','_blank','" + Features + "'); if(strR==\"SAVE\") document.location.reload();");
                        break;
                }
            }
            catch { }
        }
        #endregion

        #region Upload script

        /// <summary>
        ///  Bind built-in javascript unpload file function to specific control.
        /// 自动写入调用Upload页面的nameOfFile域，需要调用者有名为nameOfFile的域
        /// </summary>
        /// <param name="control">Html button</param>       
        /// <param name="FormType">Type</param>
        /// <param name="ID">ID</param>
        /// <returns></returns>
        public static void RenderUploadScript(Control control, string FormType, string ID)
        {
            string strWebPath = GlobalDefinition.SystemWebPath;
            if (strWebPath == "")
                throw new Exception("The parameter WebPath does not exist in parameter list. ");

            string strUrl = strWebPath + "SysFrame/UploadFile.aspx?FORM_TYPE=" + FormType.Trim() + "&ID=" + ID.Trim();
            string strFeatures = "width=586px,height=400px,top=80,left=250,resizable=yes,scrollbars=yes,help=no,status=no";

            try
            {
                if (control == null)
                {
                    return;
                }
                Type t = control.GetType();
                switch (t.Namespace)
                {
                    case "Anthem":
                    case "System.Web.UI.WebControls":
                        ((WebControl)control).Attributes.Add("onclick", "window.open('" + strUrl + "','_blank','" + strFeatures + "'); ");
                        break;
                    case "System.Web.UI.HtmlControls":
                        ((HtmlControl)control).Attributes.Add("onclick", "window.open('" + strUrl + "','_blank','" + strFeatures + "'); ");
                        break;
                }
            }
            catch { }
        }


        /// <summary>
        ///  Bind built-in javascript unpload file function to specific control.
        /// 自动写入调用Upload页面的nameOfFile域或指定返回的控件ID。
        /// </summary>
        /// <param name="control">Html button</param>       
        /// <param name="FormType">Type</param>
        /// <param name="ID">ID</param>
        /// <param name="ReturnControlID">The control ID which will contain the returned code.DIV,SPAN...</param>
        /// <returns></returns>
        public static void RenderUploadScript(Control control, string FormType, string ID, string ReturnControlID)
        {
            string strWebPath = GlobalDefinition.SystemWebPath;
            if (strWebPath == "")
                throw new Exception("The parameter WebPath does not exist in parameter list. ");

            string strUrl = strWebPath + "SysFrame/UploadFile.aspx?FORM_TYPE=" + FormType.Trim() + "&ID=" + ID.Trim() + "&RETURN_CTRL=" + ReturnControlID.Trim();
            string strFeatures = "width=586px,height=400px,top=80,left=250,resizable=yes,scrollbars=yes,help=no,status=no";

            try
            {
                if (control == null)
                {
                    return;
                }
                Type t = control.GetType();
                switch (t.Namespace)
                {
                    case "Anthem":
                    case "System.Web.UI.WebControls":
                        ((WebControl)control).Attributes.Add("onclick", "window.open('" + strUrl + "','_blank','" + strFeatures + "'); ");
                        break;
                    case "System.Web.UI.HtmlControls":
                        ((HtmlControl)control).Attributes.Add("onclick", "window.open('" + strUrl + "','_blank','" + strFeatures + "'); ");
                        break;
                }
            }
            catch { }
        }
        #endregion

        #region  Calendar

        /// <summary>
        /// Bind built-in javascript Calendar.
        /// </summary>
        /// <param name="control">Html button</param>               
        /// <returns></returns>
        public static void RenderCalendarScript(Control control)
        {
            try
            {
                if (control == null)
                {
                    return;
                }
                //Type t = control.GetType();
                //switch (t.Namespace)
                //{
                //    case "Anthem":
                //    case "System.Web.UI.WebControls":
                //        ((WebControl)control).Attributes.Add("onfocus", "calendar(this);");
                //        break;
                //    case "System.Web.UI.HtmlControls":
                //        ((HtmlControl)control).Attributes.Add("onfocus", "calendar(this);");
                //        break;
                //}

                RenderCalendarScript(control, @"/");
            }
            catch { }
        }

        /// <summary>
        /// Bind built-in javascript Calendar.
        /// </summary>
        /// <param name="control">Html button</param>
        /// <param name="FormatCharacter">The character in  "/","-" and empty value<example>/ or - or ""</example></param>
        /// <returns></returns>
        public static void RenderCalendarScript(Control control, string FormatCharacter)
        {
            try
            {
                if (control == null)
                {
                    return;
                }
                Type t = control.GetType();
                string befoerYear = "2";
                string afterYear = "5";
                if (System.Configuration.ConfigurationManager.AppSettings["WSC_Render_Calendar_Before_Year"] != null)
                {
                    befoerYear = System.Configuration.ConfigurationManager.AppSettings["WSC_Render_Calendar_Before_Year"].Trim();
                    Regex regexp = new Regex(@"^[1-9]\d*$");
                    if (!regexp.IsMatch(befoerYear))
                    {
                        //MessageBox.Show("config中Key为WSC_Render_Calendar_Before_Year的Value必须为正整数!");
                        befoerYear = "2";
                    }
                }
                if (System.Configuration.ConfigurationManager.AppSettings["WSC_Render_Calendar_After_Year"] != null)
                {
                    afterYear = System.Configuration.ConfigurationManager.AppSettings["WSC_Render_Calendar_After_Year"].Trim();
                    Regex regexp = new Regex(@"^[1-9]\d*$");
                    if (!regexp.IsMatch(afterYear))
                    {
                        //MessageBox.Show("config中Key为WSC_Render_Calendar_After_Year的Value必须为正整数!");
                        afterYear = "5";
                    }
                }
                switch (t.Namespace)
                {
                    case "Anthem":
                    case "System.Web.UI.WebControls":
                        ((WebControl)control).Attributes.Add("onfocus", "calendar(this,this,'" + FormatCharacter.Trim() + "','" + befoerYear + "','" + afterYear + "');");
                        break;
                    case "System.Web.UI.HtmlControls":
                        ((HtmlControl)control).Attributes.Add("onfocus", "calendar(this,this,'" + FormatCharacter.Trim() + "','" + befoerYear + "','" + afterYear + "');");
                        break;
                }
            }
            catch { }
        }

        #endregion

        #region Only key in digital character

        /// <summary>
        /// Bind built-in javascript.
        /// Only can key in digital character and dot  0-9 and "."
        /// </summary>
        /// <param name="control">Html button</param>               
        /// <returns></returns>
        public static void RenderKeyPressDigitalOnlyWithDot(HtmlInputButton control)
        {
            if (control == null)
            {
                return;
            }
            control.Attributes.Add("onKeypress", "return (/\\.|\\d/.test(String.fromCharCode(event.keyCode)))");
        }

        /// <summary>
        /// Bind built-in javascript.
        /// Only can key in digital character and dot  0-9 and "."
        /// </summary>
        /// <param name="control">Html button</param>               
        /// <returns></returns>
        public static void RenderKeyPressDigitalOnlyWithDot(TextBox control)
        {
            if (control == null)
            {
                return;
            }
            control.Attributes.Add("onKeypress", "return (/\\.|\\d/.test(String.fromCharCode(event.keyCode)))");
        }

        /// <summary>
        /// Bind built-in javascript.
        /// Only can key in digital character without dot  0-9 
        /// </summary>
        /// <param name="control">Html button</param>               
        /// <returns></returns>
        public static void RenderKeyPressDigitalOnlyNoDot(HtmlInputButton control)
        {
            if (control == null)
            {
                return;
            }
            control.Attributes.Add("onKeypress", "return (/\\d/.test(String.fromCharCode(event.keyCode)))");
        }

        /// <summary>
        /// Bind built-in javascript.
        /// Only can key in digital character without dot  0-9 
        /// </summary>
        /// <param name="control">Html button</param>               
        /// <returns></returns>
        public static void RenderKeyPressDigitalOnlyNoDot(TextBox control)
        {
            if (control == null)
            {
                return;
            }
            control.Attributes.Add("onKeypress", "return (/\\d/.test(String.fromCharCode(event.keyCode)))");
        }

        /// <summary>
        /// Bind built-in javascript.
        /// Only can key in 0-9 and ".","-"
        /// </summary>
        /// <param name="control">Control</param>               
        /// <returns></returns>
        public static void RenderKeyPressDigitalOnlyWithDotMinus(IAttributeAccessor control)
        {
            if (control == null)
            {
                return;
            }
            control.SetAttribute("onKeypress", "return (/\\.|\\d|-/.test(String.fromCharCode(event.keyCode)))");
        }

        #endregion

        #region Select User

        /// <summary>
        ///  Bind built-in javascript unpload file function to specific control.
        /// 自动写入指定返回的控件。
        /// </summary>
        /// <param name="control">Html button</param>       
        /// <param name="ReturnControlID_Username">Returned Control ID</param>       
        /// <returns></returns>
        public static void RenderSelectUserScript(Control control, string ReturnControlID_Username)
        {
            string strWebPath = GlobalDefinition.SystemWebPath;
            if (strWebPath == "")
                throw new Exception("The parameter WebPath does not exist in parameter list. ");

            string strUrl = strWebPath + "SysFrame/UserSelector.aspx?RETURN_CTRL_User=" + ReturnControlID_Username.Trim();
            string strFeatures = "width=586px,height=460px,top=80,left=250,resizable=yes,scrollbars=yes,help=no,status=no";

            try
            {
                if (control == null)
                {
                    return;
                }
                Type t = control.GetType();
                switch (t.Namespace)
                {
                    case "Anthem":
                    case "System.Web.UI.WebControls":
                        ((WebControl)control).Attributes.Add("onclick", "window.open('" + strUrl + "','_blank','" + strFeatures + "'); ");
                        break;
                    case "System.Web.UI.HtmlControls":
                        ((HtmlControl)control).Attributes.Add("onclick", "window.open('" + strUrl + "','_blank','" + strFeatures + "'); ");
                        break;
                }
            }
            catch { }
        }

        /// <summary>
        ///  Bind built-in javascript unpload file function to specific control.
        /// 自动写入指定返回的控件。
        /// </summary>
        /// <param name="control">button control</param>
        /// <param name="ReturnControlID_Username">Returned Control ID--user name</param>
        /// <param name="ReturnControlID_Dept">Returned Control ID--Dept</param>
        /// <returns></returns>
        public static void RenderSelectUserScript(Control control, string ReturnControlID_Username, string ReturnControlID_Dept)
        {
            string strWebPath = GlobalDefinition.SystemWebPath;
            if (strWebPath == "")
                throw new Exception("The parameter WebPath does not exist in parameter list. ");

            string strUrl = strWebPath + "SysFrame/UserSelector.aspx?RETURN_CTRL_User=" + ReturnControlID_Username.Trim() + "&RETURN_CTRL_Dept=" + ReturnControlID_Dept.Trim();
            string strFeatures = "width=586px,height=460px,top=80,left=250,resizable=yes,scrollbars=yes,help=no,status=no";

            try
            {
                if (control == null)
                {
                    return;
                }
                Type t = control.GetType();
                switch (t.Namespace)
                {
                    case "Anthem":
                    case "System.Web.UI.WebControls":
                        ((WebControl)control).Attributes.Add("onclick", "window.open('" + strUrl + "','_blank','" + strFeatures + "'); ");
                        break;
                    case "System.Web.UI.HtmlControls":
                        ((HtmlControl)control).Attributes.Add("onclick", "window.open('" + strUrl + "','_blank','" + strFeatures + "'); ");
                        break;
                }
            }
            catch { }
        }
        #endregion

        #region Show Tips
        /// <summary>
        ///  Bind built-in javascript unpload file function to specific control.
        /// 自动写入指定返回的控件。
        /// </summary>
        /// <param name="control">Web button</param>       
        /// <param name="Message">Message</param>       
        /// <returns></returns>
        public static void RenderShowTipsScript(Control control, string Message)
        {
            try
            {
                if (control == null)
                {
                    return;
                }
                Type t = control.GetType();
                switch (t.Namespace)
                {
                    case "Anthem":
                    case "System.Web.UI.WebControls":
                        ((WebControl)control).Attributes.Add("onmouseover", "javascript:wscShowTips('" + Message.Trim() + "');");
                        ((WebControl)control).Attributes.Add("onmouseout", "javascript:wscHideTips();");
                        break;
                    case "System.Web.UI.HtmlControls":
                        ((HtmlControl)control).Attributes.Add("onmouseover", "javascript:wscShowTips('" + Message.Trim() + "');");
                        ((HtmlControl)control).Attributes.Add("onmouseout", "javascript:wscHideTips();");
                        break;
                }
            }
            catch { }
        }
        #endregion

        #region Show float form

        /// <summary>
        /// 显示漂浮窗口
        /// Show the tip form in web page
        /// </summary>
        /// <param name="control"></param>
        /// <param name="detailUrl">The Page Url will be shown.</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="hideAfterLeaving">Hide the FloatForm after mouse's leaving</param>
        public static void RenderShowFloatFormScript(Control control, string detailUrl, int width, int height, bool hideAfterLeaving)
        {
            try
            {
                width = (width == 0) ? 550 : width;
                height = (height == 0) ? 300 : height;

                if (control == null)
                {
                    return;
                }
                Type t = control.GetType();
                switch (t.Namespace)
                {
                    case "Anthem":
                    case "System.Web.UI.WebControls":
                        ((WebControl)control).Attributes.Add("onmouseover", "wscShowFloatForm(this,'" + detailUrl.Trim() + "'," + width + "," + height + ")");
                        if (hideAfterLeaving)
                        {
                            ((WebControl)control).Attributes.Add("onmouseout", "wscHideFloatForm()");
                        }
                        break;
                    case "System.Web.UI.HtmlControls":
                        ((HtmlControl)control).Attributes.Add("onmouseover", "wscShowFloatForm(this,'" + detailUrl.Trim() + "'," + width + "," + height + ")");
                        if (hideAfterLeaving)
                        {
                            ((HtmlControl)control).Attributes.Add("onmouseout", "wscHideFloatForm()");
                        }
                        break;
                }
            }
            catch { }
        }

        /// <summary>
        /// 显示漂浮窗口
        /// Show the tip form in web page
        /// </summary>
        /// <param name="control"></param>
        /// <param name="detailUrl"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void RenderShowFloatFormScript(Control control, string detailUrl, int width, int height)
        {
            if (control == null)
            {
                return;
            }
            RenderScript.RenderShowFloatFormScript(control, detailUrl, width, height, true);
        }

        #endregion
    }
}