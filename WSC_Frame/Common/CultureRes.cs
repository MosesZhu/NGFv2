using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security;
using System.Security.Permissions;
using WSC.Common;
/*****************************************************************************************************
Author        : Anson.Lin
Date	      : Feb.22,2006
Description   : 
/*****************************************************************************************************/
namespace WSC.Common
{
    /// <summary>
    /// The Culture resource.    
    /// </summary>
    public sealed class CultureRes
    {
        private static string m_strLastError = "";
        /// <summary>
        /// Last error
        /// </summary>
        public static string GetLastError
        {
            get { return m_strLastError; }
        }
        internal static CommonEnum.Culture CurrentCulture
        {
            //Created by Anson Lin on 18-Jan-2006
            get
            {
                try
                {
                    string strLangID = "EN";

                    string strSQL = "EXEC SP_CULTURE_RES_USER_GET '" + GlobalDefinition.System_Name() + "','"
                        + GlobalDefinition.Cookie_LoginUser + "'";

                    using (WSC_DataConn conn = new WSC_DataConn())
                    {
                        using (SqlDataReader dr = conn.ExecuteReader(strSQL))
                        {
                            if (dr.Read())
                                strLangID = dr.GetValue(0).ToString().Trim();
                            dr.Close();
                        }
                    }

                    CommonEnum.Culture myC = ReturnCulture(strLangID);

                    return myC;
                }
                catch{ return CommonEnum.Culture.SC; }
            }
            set
            {          
                try
                {
                string strLangID = ReturnCultureString(value);

                string strSQL = "EXEC SP_CULTURE_RES_USER_SET '" + GlobalDefinition.System_Name() + "','" + GlobalDefinition.Cookie_LoginUser + "','" + strLangID + "'";
                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch(Exception ex) { throw ex; }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Culture"></param>
        /// <returns></returns>
        public static string ReturnCultureString(CommonEnum.Culture Culture)
        {
            //Created by Anson Lin on 18-Jan-2006
            string strLangID = "SC";
            switch (Culture)
            {
                case CommonEnum.Culture.EN: strLangID = "EN"; break;
                case CommonEnum.Culture.SC: strLangID = "SC"; break;
                case CommonEnum.Culture.TC: strLangID = "TC"; break;
            }
                        
            return strLangID;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CultureString"></param>
        /// <returns></returns>
        public static CommonEnum.Culture ReturnCulture(string CultureString)
        {
            //Created by Anson Lin on 18-Jan-2006
            CommonEnum.Culture myC = CommonEnum.Culture.SC;

            switch (CultureString)
            {
                case "EN": myC = CommonEnum.Culture.EN; break;
                case "SC": myC = CommonEnum.Culture.SC; break;
                case "TC": myC = CommonEnum.Culture.TC; break;
            }
            
            return myC;
        }
        public static string GetResourceCaption(string MessageID)
        {
            //Created by Anson Lin on 18-Jan-2006
            m_strLastError = "";
            try
            {

                string strBaseName = "CultureRes_" + "Caption_" + CultureRes.ReturnCultureString(GlobalDefinition.CurrentCulture);

                return HttpContext.GetGlobalResourceObject(strBaseName, MessageID).ToString();

            }
            catch (Exception ex)
            {
                m_strLastError = ex.Message;
                FileLogger.WriteLog_WSC("CultureRes_Error", "GetResourceCaption", CommonEnum.LogActionType.Error, "Error! " + ex.Message);
                return "";
            }
        }      
        public static string GetResourceMessage(string MessageID)
        {
            //Created by Anson Lin on 18-Jan-2006
            m_strLastError = "";
            try
            {
                string strBaseName = "CultureRes_" + "Message_" + CultureRes.ReturnCultureString(GlobalDefinition.CurrentCulture);

                return HttpContext.GetGlobalResourceObject(strBaseName, MessageID).ToString();
            }
            catch (Exception ex)
            {
                m_strLastError = ex.Message;
                FileLogger.WriteLog_WSC("CultureRes_Error", "GetResourceMessage", CommonEnum.LogActionType.Error, "Error! " + ex.Message);
                return "";
            }
        }
        public static string GetSysFrameResource(string MessageID)
        {
            //Created by Anson Lin on 18-Jan-2006
            m_strLastError = "";
            try
            {                
                System.Resources.ResourceManager temp = null;

                if (GlobalDefinition.CurrentCulture == CommonEnum.Culture.EN)
                    temp = new System.Resources.ResourceManager("WSC.Res.CultureRes_EN", typeof(WSC.Res.CultureRes_EN).Assembly);
                if (GlobalDefinition.CurrentCulture == CommonEnum.Culture.SC)
                    temp = new System.Resources.ResourceManager("WSC.Res.CultureRes_SC", typeof(WSC.Res.CultureRes_SC).Assembly);
                if (GlobalDefinition.CurrentCulture == CommonEnum.Culture.TC)
                    temp = new System.Resources.ResourceManager("WSC.Res.CultureRes_TC", typeof(WSC.Res.CultureRes_TC).Assembly);

                return temp.GetString(MessageID).Trim();
            }
            catch (Exception ex)
            {
                m_strLastError = ex.Message;
                FileLogger.WriteLog_WSC("CultureRes_Error", "GetSysFrameResource", CommonEnum.LogActionType.Error, "Error! " + ex.Message);
                return "";
            }
        }


        //JFK ADD 2009-08-12 For Contorl
        public static string GetAspPagerResource(string MessageID)
        {
            m_strLastError = "";
            try
            {
                System.Resources.ResourceManager temp = null;

                if (GlobalDefinition.CurrentCulture == CommonEnum.Culture.EN)
                    temp = new System.Resources.ResourceManager("WSC.Res.AspPagerRes_EN", typeof(WSC.Res.AspPagerRes_EN).Assembly);
                if (GlobalDefinition.CurrentCulture == CommonEnum.Culture.SC)
                    temp = new System.Resources.ResourceManager("WSC.Res.AspPagerRes_SC", typeof(WSC.Res.AspPagerRes_SC).Assembly);
                if (GlobalDefinition.CurrentCulture == CommonEnum.Culture.TC)
                    temp = new System.Resources.ResourceManager("WSC.Res.AspPagerRes_TC", typeof(WSC.Res.AspPagerRes_TC).Assembly);

                return temp.GetString(MessageID).Trim();
            }
            catch (Exception ex)
            {
                m_strLastError = ex.Message;
                FileLogger.WriteLog_WSC("AspPagerRes_Error", "GetAspPagerResource", CommonEnum.LogActionType.Error, "Error! " + ex.Message);
                return "";
            }
        }
       

        #region Remarked
        /*
        /// <sumurce specified by culture.
        /// </summary>
        /// <pature">Lan</param>
        /// <param name="Conte
        public static string Save(CommonEnum.Culture Culture, string MsgID, string Content, string Class)
        {
            m_strLastError = "";
            try
            {
                if (Content.Trim() == "" || MsgID.Trim() == "")
                {
                    m_strLastEr value.";
                    return null;
                }                   
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                m_strLastEsage;
                return ex.Message;
            }
        }


        /// <summary>
        /// Save resource.
        /// </summary>ed Chinese Content</param>
        /// <param name="ContentTC">Tra        /// <returns></returns>
        public static string Save(string MsgID, string EN, string ContentSC, stringtTC)
        {
            m_strLastError = "";
            try
            {
                if (MsgID.Trim() == "")s Content and MsgID can not be empty value.";
                    return null;
                }
                else
                {                   
                    string strSQL = "EXEC  SP__RES_AD '" + GlobalDefinition.System_Name() + "','"
                        + ClasgID.Trim() + "','"
                        + ContentEN.Trim() + "','" + Conten "','" + ContentTC.Trim() + "'";

                    WSC_DataConn conn = new WSC_DataConn();
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                m_strLastError = ex.Meage;
                return e;
            }
        }


        /// <summary>
        /// Retrieve the sp
        /// <param name="Message
        public static string Geteoe(stringID)
        {
            m_strLastError = "";
            try
            {E_RES_GETMSG '" //+ GlobalDefinition.System_Name() + "','" 
                    + GlobalDefinition.CurrentCulture + "','" + MessageID.Trim() + "'";

                string strR = "";

                WSC_DataConn conn = new WSC_DataConn();
                SqlDataReader dr = conn.ExecuteReader(strSQL);
                if (dr.Read())
                    strR = dr.GetValue(0).ToString().Trim();

                dr.Close();
                dr.Dispose();
                conn.Dispose();

                return strR;
            }
            catch (Exception ex)
            {
            catch (Exception ex)
            {
                m_strLastError = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Retriurces.
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllValues()
        {
            m_strLastError = "";
            try
            {
                string strSQL =ture", CommonEnum.LogActionType.Delete, "SUCCESS! SQL=" + strSQL);
                    
                    WSC_DataConn conn = new WSC_DataConn();
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSCe", CommonEnum.LogActionType.Error, "E + "  --  SQL);
                m_strLastError = ex.Message;
                return ex.Message;
            }
        }
    
        */

        #endregion

      
    }
        
}
