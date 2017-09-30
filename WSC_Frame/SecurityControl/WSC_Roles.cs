/************************************************************************************************
**********Created by Anson Lin on 3-Feb-2006                                            *********
**********Description:                                                                  *********
*************************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using WSC.Common;

namespace WSC.SecurityControl
{
    /// <summary>
    /// 角色维护
    /// </summary>
    public sealed class WSC_Roles
    {
        private string m_strLastError = "";

        /// <summary>
        /// 角色维护
        /// </summary>
        public WSC_Roles()
        { }

        /// <summary>
        /// Last error
        /// </summary>
        public string GetLastError
        {
            get { return m_strLastError; }
        }

     
        public string Add(string RoleID, string RoleName)
        {
            m_strLastError = "";
            if (RoleID.Trim() == "" || RoleName.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY");//"The parameters can not be empty value!";

            //Check security, Added by Anson on 29-Mar-2006
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE", CommonEnum.LogActionType.Add, "SUCCESS! RoleID=" + RoleID.Trim() + "  RoleName=" + RoleName.Trim());

                string strSQL = "EXEC SP_WSC_ROLE_ADD '" + RoleID.Trim() + "','" + RoleName.Trim() + "','" + GlobalDefinition.System_Name() + "'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE", CommonEnum.LogActionType.Error, "ERROR: " + ex.Message + "-- RoleID=" + RoleID.Trim() + "  RoleName=" + RoleName.Trim());

                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ADD") + ex.Message;
                return m_strLastError;
            }
        }

      
        public string Update(string RoleID, string RoleName)
        {
            m_strLastError = "";
            if (RoleID.Trim() == "" || RoleName.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY");//"The parameters can not be empty value!";

            //Check security, Added by Anson on 29-Mar-2006
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE", CommonEnum.LogActionType.Update, "SUCCESS! RoleID=" + RoleID.Trim() + "  RoleName=" + RoleName.Trim());

                string strSQL = "EXEC SP_WSC_ROLE_UPDATE '" + RoleID.Trim() + "','" + RoleName.Trim() + "','" + GlobalDefinition.System_Name() + "'";
                
                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE", CommonEnum.LogActionType.Error, "ERROR: " + ex.Message + "-- RoleID=" + RoleID.Trim() + "  RoleName=" + RoleName.Trim());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_UPDATE") + ex.Message;
                return m_strLastError;
            }
        }

      
        public string Delete(string RoleID)
        {
            m_strLastError = "";
            if (RoleID.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY");

            //Check security, Added by Anson on 29-Mar-2006
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE", CommonEnum.LogActionType.Delete, "SUCCESS! RoleID=" + RoleID.Trim());

                string strSQL = "EXEC SP_WSC_ROLE_DEL '" + GlobalDefinition.System_Name() + "','" + RoleID.Trim() + "'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE", CommonEnum.LogActionType.Delete, "ERROR: " + ex.Message + "-- RoleID=" + RoleID.Trim());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_DELETE") + ex.Message;
                return m_strLastError;
            }
        }

      
        public string RoleName(string RoleID)
        {
            m_strLastError = "";           
            try
            {
                string strSQL = "SELECT * FROM WSC_ROLE WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' AND ROLE_ID='" + RoleID.Trim() + "'";
                string strR = "";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    using (SqlDataReader dr = conn.ExecuteReader(strSQL))
                    {
                        if (dr.Read())
                        {
                            strR = dr.GetValue(0).ToString().Trim();
                            dr.Close();
                        }
                    }
                }         

                return strR;
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return "";
            }
        }

        
        public DataSet Items(string RoleID)
        {
            m_strLastError = "";
            DataSet ds = new DataSet();
           
            try
            {
                string[] strItems = new string[2];

                string strSQL = "SELECT * FROM WSC_ROLE WHERE SYS_ID='" + GlobalDefinition.System_Name() + "'";
                if (RoleID.Trim() != "")
                    strSQL += " AND ROLE_ID='" + RoleID.Trim() + "'";

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
    }
}
