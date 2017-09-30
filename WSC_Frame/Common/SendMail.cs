using System;
using System.Data;
using System.Data.SqlClient;

namespace WSC.Common
{
	/// <summary>
    /// SendMail: Add mail to mail service.
	/// </summary>
    public sealed class SendMail
	{				
		public string m_LastError="";
        /// <summary>
        /// SendMail: Add mail to mail service.
        /// </summary>
		public SendMail()
		{			
		}

        		
	
        /// <summary>
        /// ����FrameWSC��Ĭ���ʼ����ݵ�HTML���룬By Anson Lin,13-July-2005
        /// </summary>
        /// <param name="p_strFormNo"></param>
        /// <param name="p_strUserName">�û���</param>
        /// <param name="p_strSYS_Name">ϵͳ����</param>
        /// <param name="p_strReceiver">�ռ���</param>
        /// <param name="p_strSender">������</param>
        /// <param name="p_strSubject">����</param>
        /// <param name="p_strContent">�ʼ�����</param>
        /// <returns></returns>
        public string Mail_Content(string p_strFormNo, string p_strUserName, string p_strSYS_Name,
			string p_strReceiver, string p_strSender,string p_strSubject,string p_strContent)
		{
			try
			{

                //string strWebAddr = System.Configuration.ConfigurationManager.AppSettings["NPM_ADDR"];
                //System.Text.StringBuilder strR = new System.Text.StringBuilder("");
                /*
                    <htmlxmlns="http://www.w3.org/1999/xhtml"><head><title>UntitledPage</title></head>
                    <bodybackground=HTTP://BQSNET/FrameWSC/SysFrame/images/Mail_BodyBG.jpg><tablealign="center"border="0"bgcolor=whitecellpadding="0"cellspacing="0"><trheight="90"><tdalign="center"valign="top"><imgalign="absMiddle"border="0"src="HTTP://BQSNET/FrameWSC/SysFrame/images/Mail_Title.jpg"/></td>
                    </tr>
                    <tr>
                    <tdvalign="top">
                    <tableborder="0"cellpadding="8"cellspacing="0"width="100%">
                    <tr>
                    <tdnowrap="nowrap"colspan="3"style="height:18px">
                    </td>
                    </tr>
                    <tr>
                    <tdnowrap="nowrap"></td>
                    <tdvalign="bottom"width="250">
                    <imgalign="absBottom"border="0"src="HTTP://BQSNET/FrameWSC/SysFrame/images/Mail_Publisher.jpg"/>
                    <Spanstyle="font-weight:bold;font-size:11pt;color:#ff0033">FrameWSC@TEST</Span>
                    </td>
                    <tdvalign="bottom"style="color:dimgray">
                    <imgalign="absBottom"border="0"src="HTTP://BQSNET/FrameWSC/SysFrame/images/Mail_Pubdate.jpg"/>
                    1/10/20065:00:56PM</td>
                    </tr>
                    <tr>
                    <tdcolspan="3"nowrap="nowrap"style="font-size:1pt;height:1px;
                    background-color:gainsboro">
                    </td>
                    </tr>
                    </table>
                    </td>
                    </tr>
                    <tr>
                    <tdvalign="top">
                    </td>
                    </tr>
                    <tr>
                    <tdvalign="top">
                    <tableborder="0"cellpadding="8"cellspacing="0"width="100%">
                    <trheight="30">
                    <tdnowrap="nowrap"style="width:18px">
                    </td>
                    <tdstyle="width:36px;font-weight:bold;color:dimgray"align="left">
                    Title</td>
                    <tdstyle="color:dimgray">
                    TitleTest</td>
                    </tr>
                    <tr>
                    <tdnowrap="nowrap"align="left"valign="top"style="width:18px">
                    </td>
                    <tdalign="left"style="width:36px;font-weight:bold;color:dimgray"valign="top">
                    Content</td>
                    <tdalign="left"valign="top">
                    <divstyle="color:dimgray">ContentTest</div>
                    </td>
                    </tr>
                    </table>

                    </td>
                    </tr>
                    <tr>
                    <td>
                    <div>&nbsp;&nbsp;<imgsrc="HTTP://BQSNET/FrameWSC/SysFrame/images/Mail_DotLine.gif"/></div>
                    </td>
                    </tr>
                    <tr>
                    <tdstyle="color:dimgray">
                    &nbsp;&nbsp;&nbsp;ϵͳ����:
                    &nbsp;&nbsp;&nbsp;&nbsp;<ahref="HTTP://BQSNET/FrameWSC/"
                    target="_blank"title="HTTP://BQSNET/FrameWSC/">FrameWSC</a>
                    </td>
                    </tr>
                    <tr>
                    <tdalign="right"valign="bottom">
                    <imgsrc="HTTP://BQSNET/FrameWSC/SysFrame/images/Mail_BottomLtd.jpg"/>
                    </td>
                    </tr>
                    </table>
                    </body>
                    </html>

                    */

                //strR=strR.Replace("'","");
				//return strR.ToString();

                return p_strContent.Trim();
			}
			catch{return "";}
		}

		/// <summary>
		/// 
		/// </summary>		
		/// <param name="strMailType">�ʼ�����,�ɵ����߶���</param>
		/// <param name="strFormNo"></param>
        /// <param name="strReceiver">�ռ���</param>
		/// <param name="strCC">����</param>
		/// <param name="strBCC">����</param>
        /// <param name="strSubject">����</param>
        /// <param name="strContent">�ʼ�����</param>
		/// <returns>SUCCESS | Error:...</returns>
		public string Mail_Add(string strMailType,string strFormNo,
			string strReceiver, string strCC, string strBCC,
            string strSubject, string strContent)
		{
            string strSysName = GlobalDefinition.System_Name();

            if (strReceiver.Trim() == "")
				return "ERROR:" + "RECEIVER can not be empty";

            if (strSubject.Trim() == "")
				return "ERROR:" + "SUBJECT can not be empty";

            if (strContent.Trim() == "")
				return "ERROR:" + "CONTENT can not be empty";

			try
            {
                string strConn = GlobalDefinition.MailService_ConnectionString;
                if(strConn=="")               
                    return "Mail service configuration is incorrect!";
                
                WSC_DataConn conn = new WSC_DataConn();
                conn.DBConnString = strConn;
                
				string strSQL="EXEC SP_MS_MAIL_ADD '" + strSysName.Trim() + "','" + strMailType.Trim() + "','";
                strSQL += strFormNo.Trim() + "','" + strReceiver.Trim() + "','" + strCC.Trim() + "','" + strBCC.Trim() + "','";
				strSQL += strSubject.Trim() + "','" + strContent.Trim() + "'";

				string strR = conn.ExecuteSqlNonQuery(strSQL);
                conn.Dispose();
                conn = null;
				
				return strR;
				
			}
			catch(Exception ex)
			{return "ERROR: " + ex.Message;}

		}
	}
}
