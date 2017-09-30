//=====================================================================
//Created by Anson Lin
//2002/01/31
//=====================================================================

using System;
using System.IO;
using System.Web;

namespace WSC.Common
{

    //Created by Anson Lin on 18-Jan-2006
	//****************************************************** 
	//* Description: This is an interface which 
	//* has one abstract method - WriteLog. 
	//* Components that support this interface 
	//* will provide the implementation of this method. 
	//****************************************************** 
    //public interface ILog
    //{
    //    void WriteLog(string strMsg, string eLogType, string strUser);
    //}
	
    
    public sealed class FileLogger  //: ILog
	{		
		 
        

		//******************************************************** 
		//* This is the implementation of the ILog.WriteLog method 
		//******************************************************** 

        /// <summary>
        /// Represents log writer for application. 
        /// Messages with Error type will be filled into a separate file.
        /// </summary>
        public static void WriteLog(string FileNameFlag, string Module, CommonEnum.LogActionType Action, string Message) 
		{
            try
            {
                WriteLog_ToDB(FileNameFlag.Trim(), Module.Trim(), Action, Message.Trim());
            }
            catch { }

           
			try
			{
                string strAction = GetLogActionString(Action);
                string strNFile = (strAction == "Error") ? "_Error" : "";
                strNFile = (FileNameFlag.Trim() == "") ? strNFile : "_" + FileNameFlag.Trim() + strNFile;
                string strPath = GlobalDefinition.LogFilePhysicalPath;
                string strFileName = ((string)(GlobalDefinition.System_Name() + "_"
                    + DateTime.Today.ToString("yyyyMMdd"))).Replace("/", "").Replace("-", "").Replace("\\", "").Replace(" ", "").Replace(":", "").Replace("：", "")
                    + strNFile + ".log";                
                using (StreamWriter OutputStream = new StreamWriter(strPath.Trim() + strFileName.Trim(), true, System.Text.Encoding.GetEncoding("UTF-8")))
                {
                    OutputStream.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
                    OutputStream.WriteLine("Begin at: " + System.DateTime.Now.ToString());
                    OutputStream.WriteLine("User: " + GlobalDefinition.Cookie_LoginUser);
                    OutputStream.WriteLine("Module: " + Module.Trim() + "\t\tLog Type: " + strAction);
                    OutputStream.WriteLine("Description: " + Message.Trim());
                    OutputStream.WriteLine("End");
                    OutputStream.Close();
                }
			}
			catch(Exception ex)
			{
                string strErr = CultureRes.GetSysFrameResource("MSG_ERR_LOG"); 
                throw new Exception(strErr + ex.Message);
			}			
		}

        /// <summary>
        /// Represents log writer for application. 
        /// Messages with Error type will be filled into a separate file.
        /// </summary>      
        public static void WriteLog(string Category,string FileNameFlag, string Module, CommonEnum.LogActionType Action, string Message)
        {
            //Created by Anson Lin
            try
            {
                WriteLog_ToDB(FileNameFlag.Trim(), Module.Trim(), Action, Message.Trim());
            }
            catch { }                        
            try
            {
                string strAction = GetLogActionString(Action);
                string strNFile = (strAction == "Error") ? "_Error" : "";
                strNFile = (FileNameFlag.Trim() == "") ? strNFile : "_" + FileNameFlag.Trim() + strNFile;
                string strPath = GlobalDefinition.LogFilePhysicalPath;
                if (Category.Trim() != "")
                    strPath += Category.Replace("\\", "_") + "\\";
                //path exist  Add by Hedda 20060331
                if (!System.IO.Directory.Exists(strPath))
                {
                    System.IO.Directory.CreateDirectory(strPath);
                }
                string strFileName = ((string)(GlobalDefinition.System_Name() + "_"
                    + DateTime.Today.ToString("yyyyMMdd"))).Replace("/", "").Replace("-", "").Replace("\\", "").Replace(" ", "").Replace(":", "").Replace("：", "")
                    + strNFile + ".log";
                using (StreamWriter OutputStream = new StreamWriter(strPath.Trim() + strFileName.Trim(), true, System.Text.Encoding.GetEncoding("UTF-8")))
                {
                    OutputStream.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
                    OutputStream.WriteLine("Begin at: " + System.DateTime.Now.ToString() + "\t\tModule: " + Module.Trim() + "\t\tLog Type: " + strAction + "\t\tUser: " + GlobalDefinition.Cookie_LoginUser);
                    OutputStream.WriteLine("Description: \t" + Message.Trim());
                    OutputStream.WriteLine("End");
                    OutputStream.Close();
                }
            }
            catch (Exception ex)
            {
                string strErr = CultureRes.GetSysFrameResource("MSG_ERR_LOG"); 
                throw new Exception(strErr + ex.Message);
            }
        }


        

        /// <summary>
        /// Represents log writer for application. 
        /// Messages with Error type will be filled into a separate file.
        /// </summary>
        internal static void WriteLog_Access()
        {
        

            try
            {
                //Record visitor's info, 20-Feb-2006 16:58
                using(System.Web.UI.Page executingPage = HttpContext.Current.Handler as System.Web.UI.Page)
                {
                    if (executingPage != null)
                    {
                        StreamWriter OutputStream;
                        string strPath = GlobalDefinition.LogFilePhysicalPath + "Access\\";
                        if (!System.IO.Directory.Exists(strPath))
                            System.IO.Directory.CreateDirectory(strPath);
                        string strFileName = ((string)(GlobalDefinition.System_Name() + "_"
                            + DateTime.Today.ToString("yyyyMMdd"))).Replace("/", "").Replace("-", "").Replace("\\", "").Replace(" ", "").Replace(":", "").Replace("：", "")
                            + "_Access" + ".log";
                        using (OutputStream = new StreamWriter(strPath.Trim() + strFileName.Trim(), true, System.Text.Encoding.GetEncoding("UTF-8")))
                        {
                            string strUserAuthType = executingPage.User.Identity.AuthenticationType;
                            string strUserAgent = executingPage.Request.UserAgent;
                            string strUserHostName = executingPage.Request.UserHostName;
                            string strUserHostAddr = executingPage.Request.UserHostAddress;

                            OutputStream.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
                            OutputStream.WriteLine("Begin at: " + System.DateTime.Now.ToString());
                            OutputStream.WriteLine("User: " + GlobalDefinition.Cookie_LoginUser + "\t\tRemote Name: " + strUserHostName + "\t\tRemote Addr: " + strUserHostAddr + "\t\tAuthenticationType: " + strUserAuthType);
                            //OutputStream.WriteLine("Remote Name: " + strUserHostName + "\t\tRemote Addr: " + strUserHostAddr);
                            OutputStream.WriteLine("User Agent: " + strUserAgent.Trim());
                            OutputStream.WriteLine("Site: " + GlobalDefinition.SystemWebPath + "\t\tCookie.");
                            OutputStream.WriteLine("End");
                            OutputStream.Close();
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                throw new Exception(CultureRes.GetSysFrameResource("MSG_ERR_LOG") + ex.Message);
            }
        }               
        internal static void WriteLog_WSC(string Flag, string Module, CommonEnum.LogActionType Action, string Message)
		{
            try
            {
                WriteLog("SystemIL", Flag.Trim(), Module.Trim(), Action, Message);
            }
            catch { }

        }       
        internal static string WriteLog_ToDB(string Flag, string Module, CommonEnum.LogActionType Action, string Message)
        {
            try
            {

                //TODO: Add the switch to control
                //5-April-2005
                Framework.ParameterSetup ps = new WSC.Framework.ParameterSetup();
                string strRnpm = ps.GetValue("WRITE_LOG_TO_DB");
                string strRsys = ps.GetSystemValue("WRITE_LOG_TO_DB");
                if (strRnpm != "Y" || strRsys != "Y")
                    return "Switch is off.";
                string strAction = GetLogActionString(Action);                
                string strUrlLocalPath = HttpContext.Current.Request.Url.LocalPath;
                string strUrlQuery = HttpContext.Current.Request.Url.Query;
                string strSQL = "EXEC SP_WSC_LOG_ADD '" + GlobalDefinition.System_Name() + "','" + Flag.Trim() + "','"
                    + Module.Trim() + "','" + strAction.Trim() + "','" + Message.Trim() + "','"
                    + GlobalDefinition.Cookie_LoginUser + "','" + strUrlLocalPath + "','" + strUrlQuery + "'";
                string strResult = "";
                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    strResult = conn.ExecuteSqlNonQuery(strSQL);
                }
                if (strResult == "SUCCESS")
                    return strResult;
                else
                    return CultureRes.GetSysFrameResource("MSG_ERR_LOG") + "\\r\\n" + strResult;
            }
            catch (Exception ex)
            {               
                throw new Exception(CultureRes.GetSysFrameResource("MSG_ERR_LOG") + ex.Message);
            }
        }      
        internal static string GetLogActionString(CommonEnum.LogActionType Action)
        {
            string strAction = "";

            if (Action == CommonEnum.LogActionType.Add)
                strAction = "ADD";
            else if (Action == CommonEnum.LogActionType.Update)
                strAction = "UPDATE";
            else if (Action == CommonEnum.LogActionType.Delete)
                strAction = "DELETE";
            else if (Action == CommonEnum.LogActionType.Error)
                strAction = "ERROR";
            else if (Action == CommonEnum.LogActionType.Info)
                strAction = "INFO";
            else if (Action == CommonEnum.LogActionType.Get)
                strAction = "GET";
            else if (Action == CommonEnum.LogActionType.Success)
                strAction = "Success";
            else if (Action == CommonEnum.LogActionType.Query)
                strAction = "QUERY";
            else if (Action == CommonEnum.LogActionType.Report)
                strAction = "REPORT";
            else if (Action == CommonEnum.LogActionType.Login)
                strAction = "LOGIN";

            return strAction;
        }
	}
           
}

