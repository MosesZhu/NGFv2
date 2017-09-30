// Created by Anson Lin on 18-Jan-2006
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Text;
using System.Collections.Specialized;

namespace WSC.Common
{
    /// <summary>
    /// 邮件服务类，必须有本地邮件服务配套支持
    /// </summary>
    public sealed class MailService
    {
      
        public static string m_LastError = "";
        public static string DefaultHtmlContentTemplate(string Sender, string Title, string Content)
        {
            StringBuilder bstrHtmlText = new StringBuilder();
            string strPath = HttpRuntime.AppDomainAppPath + "SysFrame\\MailTemplate";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\MailFrameWSC.htm";
            if (!File.Exists(strPath))
            {
                StringBuilder bstrHW = new StringBuilder("");
                bstrHW.Append("<html xmlns='http://www.w3.org/1999/xhtml'><head><title>FrameWSC Mail Notice</title>");
                bstrHW.Append("<META http-equiv=Content-Type content='text/html; charset=gb2312'></head><body background='$FORMAT_SYS_LINK/SysFrame/images/Mail_BodyBG.jpg'>");
                bstrHW.Append("<table align='center' border='0' bgcolor='white' cellpadding='0' cellspacing='0'>  <tr height='90'><td align='center' valign='top'>");
                bstrHW.Append("<img align='absMiddle' border='0' src='$FORMAT_SYS_LINK/SysFrame/images/Mail_Title.jpg' /></td></tr>  <tr><td valign='top'>");
                bstrHW.Append("<table border='0' cellpadding='8' cellspacing='0' width='100%'><tr><td nowrap='nowrap' colspan='3' style='height: 18px'></td>  </tr>");
                bstrHW.Append("<tr><td nowrap='nowrap'></td><td valign='bottom' width='250'><img align='absBottom' border='0' src='$FORMAT_SYS_LINK/SysFrame/images/Mail_Publisher.jpg' />");
                bstrHW.Append("<span style='font-weight: bold; font-size: 11pt; color: #ff0033'>$FORMAT_SENDER</span></td>");
                bstrHW.Append("<td valign='bottom' style='color: dimgray'><img align='absBottom' border='0' src='$FORMAT_SYS_LINK/SysFrame/images/Mail_Pubdate.jpg' />");
                bstrHW.Append("$FORMAT_DATE</td>  </tr><tr><td colspan='3' nowrap='nowrap' style='font-size: 1pt; height: 1px; background-color: gainsboro'></td>  </tr></table></td></tr>");
                bstrHW.Append("<tr><td valign='top'></td>  </tr>  <tr><td valign='top'>    <table border='0' cellpadding='8' cellspacing='0' width='100%'>  <tr height='30'><td nowrap='nowrap' style='width: 18px'></td>");
                bstrHW.Append("<td style='width: 36px; font-weight: bold; color: dimgray' align='left'>    主题</td><td style='color: dimgray'>$FORMAT_TITLE</td></tr>  <tr>");
                bstrHW.Append("<td nowrap='nowrap' align='left' valign='top' style='width: 18px'></td><td align='left' style='width: 36px; font-weight: bold; color: dimgray' valign='top'>");
                bstrHW.Append("内容</td><td align='left' valign='top'><div style='color: dimgray'>$FORMAT_CONTENT</div></td></tr></table></td></tr>");
                bstrHW.Append("<tr><td><div>&nbsp;&nbsp;<img src='$FORMAT_SYS_LINK/SysFrame/images/Mail_DotLine.gif' /></div></td>  </tr>  <tr><td style='color: dimgray'>");
                bstrHW.Append("&nbsp;&nbsp; &nbsp; 系统链接:&nbsp;&nbsp;&nbsp;&nbsp;<a href='$FORMAT_SYS_LINK' target='_blank' >$FORMAT_SYS_NAME</a></td></tr><tr><td align='right' valign='bottom'>");
                bstrHW.Append("<img src='$FORMAT_SYS_LINK/SysFrame/images/Mail_BottomLtd.jpg' /></td></tr></table></body></html>");
                StreamWriter sw = new StreamWriter(strPath, false, System.Text.Encoding.GetEncoding("GB2312"));
                sw.WriteLine(bstrHW.ToString());
                sw.Flush();
                sw.Close();
                bstrHtmlText = bstrHW;
            }
            else
            {
                StreamReader sr = new StreamReader(strPath, System.Text.Encoding.GetEncoding("GB2312"));
                string strLine;
                while ((strLine = sr.ReadLine()) != null)
                    bstrHtmlText.Append(strLine);
                    sr.Close();
            }
            bstrHtmlText.Replace("$FORMAT_SENDER", Sender.Trim() + "@" + GlobalDefinition.System_Name());
            bstrHtmlText.Replace("$FORMAT_DATE", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            bstrHtmlText.Replace("$FORMAT_TITLE", Title.Trim());
            bstrHtmlText.Replace("$FORMAT_CONTENT", Content.Trim());
            bstrHtmlText.Replace("$FORMAT_SYS_NAME", "");
            bstrHtmlText.Replace("$FORMAT_SYS_LINK", "");
            return bstrHtmlText.ToString().Replace("'", "\"").Trim();
        }
        public static string Add(CommonEnum.MailType MailType, string strReceiver, string CC, string BCC,
            string Subject, string Content)
        {
            string strSysName = GlobalDefinition.System_Name();

            if (strReceiver.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_MAIL_ADD_RECEIVER"); 

            if (Subject.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_MAIL_ADD_SUBJECT");

            if (Content.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_MAIL_ADD_CONTENT"); 

            try
            {
                string strMailType = "";

                switch (MailType)
                {
                    case CommonEnum.MailType.GetPassword: strMailType = "GetPwd"; break;
                    case CommonEnum.MailType.Error: strMailType = "Error"; break;
                    case CommonEnum.MailType.Information: strMailType = "Info"; break;
                    case CommonEnum.MailType.ToAdmin: strMailType = "ToAdmin"; break;
                    case CommonEnum.MailType.ToUser: strMailType = "ToUser"; break;
                }

                string strConn = GlobalDefinition.MailService_ConnectionString;

                if (strConn == "")
                    return CultureRes.GetSysFrameResource("MSG_ERR_MAIL_CONN_SEND"); 

                string strR = "";
                using (WSC_DataConn conn = new WSC_DataConn(false))
                {
                    conn.DBConnString = strConn;
                    conn.Open();

                    string strSQL = "EXEC SP_MS_MAIL_ADD '" + strSysName.Trim() + "','" + strMailType.Trim() + "','";
                    strSQL += "','" + strReceiver.Trim() + "','" + CC.Trim() + "','" + BCC.Trim() + "','";
                    strSQL += Subject.Trim() + "','" + Content.Trim() + "'";

                    strR = conn.ExecuteSqlNonQuery(strSQL);
                }

                return strR;

            }
            catch (Exception ex)
            {
                return CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message; 
            }

        }             
        /// <summary>
        /// 组装Mail
        /// by Hedda 20060330
        /// </summary>
        /// <param name="MailTemplateFileName"></param>
        /// <param name="mailParams"></param>
        /// <returns></returns>
        public static string LoadMail(string MailTemplateFileName,NameValueCollection mailParams)
        {
            try
            {
                m_LastError = "";            
                StringBuilder sbMail = new StringBuilder();                
                string strMailTemplatePath=GlobalDefinition.MailTemplatePhysicalPath+MailTemplateFileName;

                if (!System.IO.File.Exists(strMailTemplatePath))
                {                   
                    return "";
                }
                    StreamReader sr = new StreamReader(strMailTemplatePath, System.Text.Encoding.GetEncoding("GB2312"));                
                    string strLine;
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        sbMail.Append(strLine);
                    }
                    sr.Close();
                
                for (int i = 0; i < mailParams.Count; i++)
                {
                    
                    string strParamName = mailParams.GetKey(i).ToUpper().Trim();
                    string strParamValue = mailParams[strParamName];                  
                    sbMail.Replace("$" + strParamName, strParamValue);
                }
                return sbMail.ToString();
            }
            catch (Exception error)
            {              
                return "";
            }
        }

       
    }
}
