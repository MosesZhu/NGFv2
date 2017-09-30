using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Text;
using WSC.Common;
using WSC.Framework;

namespace WSC.SecurityControl
{
    /// <summary>
    ///
    /// Created by Anson Lin on 4-Feb-2006
    /// </summary>
    public sealed class WSC_Permission
    {
        private Hashtable m_SYS_Sub_Id = new Hashtable(); //Add by AIC21/arty.yu on 20120612
        private Hashtable m_Site_Module = new Hashtable(); //Add by AIC21/arty.yu on 20120612
        private int m_Mode_Count = 0; //Add by AIC21/arty.yu on 20120612
        private DataTable m_Module = new DataTable(); //Add by AIC21/arty.yu on 20120612

        public WSC_Permission()
        {

            string strSQL = "EXEC  SP_WSC_CHECK_USER_SECURITY_New '" + GlobalDefinition.Cookie_LoginUser + "','" + GlobalDefinition.System_Name() + "'";

            using (WSC_DataConn conn = new WSC_DataConn())
            {
                m_Module = conn.ExecuteQuery(strSQL).Tables[0];
            }

            if (GlobalDefinition.Cookie_SimulateUser != null && GlobalDefinition.Cookie_SimulateUser != "")
            {
                NavigatingTreeData navigating = new NavigatingTreeData();
                m_SYS_Sub_Id = navigating.GetSysSubIdByUser(GlobalDefinition.Cookie_SimulateUser);
            }

            WSC_Site_Module siteModule = new WSC_Site_Module();
            m_Mode_Count = siteModule.GetSiteModeIdCount();

            string site = siteModule.WSCCurrentSite(GlobalDefinition.Cookie_LoginUser);
            if (site != "")
            {
                m_Site_Module = siteModule.GetModeIdBySite(site); ; //Add by AIC21/arty.yu on 20120612
            }
 
        }

        private static string m_strLastError = "";
        public static string GetLastError
        {
            get { return m_strLastError; }
        }

        public string Validate(string ModuleID, string SysSubId)
        {
            m_strLastError = "";
            try
            {
                //漏： ， Remarked by Anson on 29-Mar-2006

                DataRow[] rows = m_Module.Select("SYS_ID='" + GlobalDefinition.System_Name() + "' AND (MODULE_ID='*' OR MODULE_ID='" + ModuleID + "')");

                if (rows.Length == 0)
                    return "N";
                else

                    //add by AIC0/Arty Yu on 201205008
                    if (GlobalDefinition.Cookie_SimulateUser != null && GlobalDefinition.Cookie_SimulateUser != "")
                    {
                        if (SysSubId != "" && !m_SYS_Sub_Id.ContainsKey(SysSubId)) return "N";
                    }
                    //end add by AIC0/Arty Yu on 201205008


                    
                    //Add by AIC21/arty.yu on 20120502
                    if (m_Mode_Count != 0)
                    {
                        if (!m_Site_Module.ContainsKey(ModuleID.Trim()))
                        {
                            return "N";
                        }
                    }
                    //end Add by AIC21/arty.yu on 20120502

                    return "Y";
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return m_strLastError;
            }
        }




        public static string CheckPermission_SysMenu()
        {
            m_strLastError = "";
            try
            {
                //有漏： 检测， Remarked by Anson on 29-Mar-2006
                string strR = "FALSE";
                string strSQL = "EXEC  SP_WSC_CHECK_USER_SECURITY_SYS_MENU '" + GlobalDefinition.Cookie_LoginUser + "','" + GlobalDefinition.System_Name() + "'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    using (SqlDataReader dr = conn.ExecuteReader(strSQL))
                    {
                        if (dr.Read())
                            strR = dr.GetValue(0).ToString().Trim();   //TRUE=Y  FALSE=N

                        dr.Close();
                    }
                }

                if (strR != "TRUE")
                    return "N";
                else
                    return "Y";
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return m_strLastError;
            }
        }


        public static DataSet GetPermissions(string LoginName)
        {
            m_strLastError = "";
            DataSet ds = new DataSet();

            try
            {
                if (LoginName == "")
                    LoginName = GlobalDefinition.Cookie_LoginUser;

                string strSQL = "EXEC  SP_WSC_GET_USER_SECURITY_ALL '" + LoginName.Trim() + "','"
                     + GlobalDefinition.System_Name() + "'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    ds = conn.ExecuteQuery(strSQL);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return ds;
            }
        }



        public static bool LoginValidate(string LoginName, string Password)
        {
            m_strLastError = "";
            bool bolR = false;
            try
            {
                string strViewQuery = "getAmEmployee('" + GlobalDefinition.System_Name() + "','" + LoginName + "')";
                string strPwd = "";

                string strSQL = "SELECT PASS_WORD FROM " + strViewQuery + " WHERE UPPER(LOGIN_NAME)='" + LoginName.Trim().ToUpper() + "' AND ACTIVE='Y'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    using (SqlDataReader dr = conn.ExecuteReader(strSQL))
                    {
                        if (dr.Read())
                            strPwd = dr.GetValue(0).ToString().Trim();

                        dr.Close();
                    }
                }

                if (Password.Trim().ToUpper() == strPwd.Trim().ToUpper())
                    bolR = true;

                return bolR;
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return false;
            }
        }


        public static string RetrievePassword(string LoginName)
        {
            m_strLastError = "";

            try
            {
                string strPwd = "";

                if (LoginName.Trim() == "") LoginName = GlobalDefinition.Cookie_LoginUser;
                string strViewQuery = "getAmEmployee('" + GlobalDefinition.System_Name() + "','" + LoginName + "')";

                string strSQL = "SELECT PASS_WORD FROM " + strViewQuery + " WHERE UPPER(LOGIN_NAME)='" + LoginName.Trim().ToUpper() + "' AND ACTIVE='Y'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    using (SqlDataReader dr = conn.ExecuteReader(strSQL))
                    {
                        if (dr.Read())
                            strPwd = dr.GetValue(0).ToString().Trim();

                        dr.Close();
                    }
                }

                if (strPwd == "")
                    return CultureRes.GetSysFrameResource("MSG_ERR_PASSWORD_INCORRECT");

                string strContent = MailService.DefaultHtmlContentTemplate(GlobalDefinition.System_Name() + "_Admin", "取回密码", "您的密码是： " + strPwd);
                return MailService.Add(CommonEnum.MailType.GetPassword, PublicFunctions.eMailAddress(LoginName.Trim()), "", "", "取回密码", strContent);

            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_PASSWORD_RTV") + "\\r\\n" + ex.Message;
                return m_strLastError;
            }
        }

        /// <summary>
        /// Check whether the current user is SystemAdmin.
        /// add by Hedda 20080218
        /// </summary>
        /// <returns></returns>
        public static string IsSysAdmin()
        {
            m_strLastError = "";
            try
            {
                string strR = "FALSE";
                string strSQL = "EXEC  SP_WSC_CHECK_USER_ISADMIN '" + GlobalDefinition.System_Name() + "','" + GlobalDefinition.Cookie_LoginUser + "'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    using (SqlDataReader dr = conn.ExecuteReader(strSQL))
                    {
                        if (dr.Read())
                            strR = dr.GetValue(0).ToString().Trim();   //TRUE=Y  FALSE=N

                        dr.Close();
                    }
                }

                if (strR != "TRUE")
                    return "N";
                else
                    return "Y";
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return m_strLastError;
            }
        }
        /// <summary>
        /// Check whether the user exists in System.
        /// add by Tyler.Liu 200901104
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public static bool IsUserExists(string LoginName)
        {
            int count = 0;

            try
            {
                string strViewQuery = "getAmEmployee('" + GlobalDefinition.System_Name() + "','" + LoginName + "')";


                string strSQL = "SELECT COUNT(*) FROM " + strViewQuery + " WHERE UPPER(replace(replace(LOGIN_NAME,'.',''),' ',''))='" + LoginName.Trim().ToUpper().Replace(".","").Replace(" ","")  + "' AND ACTIVE='Y'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    using (SqlDataReader dr = conn.ExecuteReader(strSQL))
                    {
                        if (dr.Read())
                            count = dr.GetInt32(0);
                        dr.Close();
                    }
                }

                return (count > 0);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Get flower name from NT login name.
        /// add by Tyler.Liu 200901104
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public static string GetFlowerLoginFromNTLogin(string LoginName)
        {
            string flowerLoginName=string.Empty;

            try
            {
                string strViewQuery = "getAmEmployee('" + GlobalDefinition.System_Name() + "','" + LoginName + "')";


                string strSQL = "SELECT LOGIN_NAME FROM VIEW_AM_EMPLOYEE WHERE UPPER(replace(replace(LOGIN_NAME,'.',''),' ',''))='" + LoginName.Trim().ToUpper().Replace(".", "").Replace(" ", "") + "' AND ACTIVE='Y'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    using (SqlDataReader dr = conn.ExecuteReader(strSQL))
                    {
                        if (dr.Read())
                            flowerLoginName = dr[0].ToString();
                        dr.Close();
                    }
                }                
            }
            catch { }
            return flowerLoginName;
        }
       
    }
}
