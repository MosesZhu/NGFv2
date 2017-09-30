/************************************************************************************************
**********Created by Anson Lin on 1-Feb-2006                                            *********
**********Description:                                                                  *********
*************************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Web;
using WSC.Common;

namespace WSC.SecurityControl
{
   
    internal sealed class WSC_Role_User : I_WSC_FactoryModule
    {

        public WSC_Role_User()
        { }

        private string m_strLastError = "";

        public string GetLastError
        {
            get { return m_strLastError; }
        }

        public string Add(string RoleID, string LoginName)
        {
            m_strLastError = "";
            if (RoleID.Trim() == "" || LoginName.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY");//"The parameters can not be empty value!";

           
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
            {
                //判断.Added By Paul.20070404.
                if (HttpContext.Current.Application["WebServiceContext"] == null)
                {
                    return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");
                }
            }

            try
            {
                bool bolR = CheckRoleUserExist(RoleID.Trim(), LoginName.Trim());
                if (!bolR)
                {
                    return CultureRes.GetSysFrameResource("MSG_ERR_SETTING_EXIST");//"This setting has already existed in the database.";
                }

                FileLogger.WriteLog_WSC("WSC", "ROLE_USER_SECURITY", CommonEnum.LogActionType.Add, "SUCCESS! RoleID=" + RoleID.Trim() + "  User=" + LoginName.Trim());

                string strSQL = "EXEC SP_WSC_SECURITY_ROLE_USER_ADD '" + RoleID.Trim() + "','" + LoginName.Trim() + "','" + GlobalDefinition.System_Name() + "'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE_USER_SECURITY", CommonEnum.LogActionType.Error, "ERROR: " + ex.Message + "--  RoleID=" + RoleID.Trim() + "  User=" + LoginName.Trim());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ADD") + ex.Message;
                return m_strLastError;
            }
        }

      
        public string Update(string RoleID, string LoginName)
        {
            m_strLastError = "";
            if (RoleID.Trim() == "" || LoginName.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY");//"The parameters can not be empty value!";

          
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE_USER_SECURITY", CommonEnum.LogActionType.Update, "SUCCESS! RoleID=" + RoleID.Trim() + "  User=" + LoginName.Trim());

                string strSQL = "EXEC SP_WSC_SECURITY_ROLE_USER_DEL '" + RoleID.Trim() + "','" + LoginName.Trim() + "','" + GlobalDefinition.System_Name() + "'; ";
                strSQL += "EXEC SP_WSC_SECURITY_ROLE_USER_ADD '" + RoleID.Trim() + "','" + LoginName.Trim() + "','" + GlobalDefinition.System_Name() + "'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE_USER_SECURITY", CommonEnum.LogActionType.Error, "ERROR: " + ex.Message + "--  RoleID=" + RoleID.Trim() + "  User=" + LoginName.Trim());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_UPDATE") + ex.Message;
                return m_strLastError;
            }
        }

      
        public string Delete(string RoleID, string LoginName)
        {
            m_strLastError = "";
            if (RoleID.Trim() == "" && LoginName.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY");//"The two parameters can not be all empty values!";

            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE_USER_SECURITY", CommonEnum.LogActionType.Delete, "SUCCESS! RoleID=" + RoleID.Trim() + "  User=" + LoginName.Trim());

                string strSQL = "EXEC SP_WSC_SECURITY_ROLE_USER_DEL '" + RoleID.Trim() + "','" + LoginName.Trim() + "','" + GlobalDefinition.System_Name() + "' ";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE_USER_SECURITY", CommonEnum.LogActionType.Error, "ERROR: " + ex.Message + "--  RoleID=" + RoleID.Trim() + "  User=" + LoginName.Trim());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_DELETE") + ex.Message;
                return m_strLastError;
            }
        }

        
        /// <summary>
        ///
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        private bool CheckRoleUserExist(string RoleID, string LoginName)
        {
            m_strLastError = "";
            try
            {
                string strSQL = "SELECT ROLE_ID FROM WSC_USER_ROLE WHERE SYS_ID='" + GlobalDefinition.System_Name()
                    + "' AND LOGIN_NAME = '" + LoginName.Trim() + "' AND ROLE_ID='" + RoleID + "'";
                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    using (SqlDataReader dr = conn.ExecuteReader(strSQL))
                    {
                        if (dr.Read())
                        {
                            dr.Close();
                            return false;
                        }
                        else
                        {
                            dr.Close();
                            return true;
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return false;
            }
        }
                
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PType">type</param>
        /// <returns></returns>
        public DataSet TreeGetParentItems(WSC_TypeEnum.TreeParentType PType)
        {
            // Created by Anson Lin on 23-Jan-2006
            m_strLastError = "";
            DataSet ds = new DataSet();

            try
            {
                string strSQL = "";
                if (PType == WSC_TypeEnum.TreeParentType.RoleUser_byRole)
                {
                    strSQL = "SELECT DISTINCT RM.ROLE_ID AS ID,R.ROLE_NAME AS NAME FROM WSC_USER_ROLE RM,WSC_ROLE R WHERE R.ROLE_ID=RM.ROLE_ID AND RM.SYS_ID='" + GlobalDefinition.System_Name() + "' AND R.SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY RM.ROLE_ID";
                }
                else if (PType == WSC_TypeEnum.TreeParentType.RoleUser_byUser)
                {
                    strSQL = "SELECT DISTINCT RM.LOGIN_NAME AS ID,LOGIN_NAME AS NAME FROM WSC_USER_ROLE RM WHERE RM.SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY RM.LOGIN_NAME";
                }

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

        /// <summary>
        ///
        /// </summary>
        /// <param name="PType">type</param>
        /// <param name="ParentID">ID</param>
        /// <returns></returns>
        public DataSet TreeGetChildItems(WSC_TypeEnum.TreeParentType PType, string ParentID)
        {
            // Created by Anson Lin on 23-Jan-2006
            m_strLastError = "";
            DataSet ds = new DataSet();

            try
            {
                string strSQL = "";
                if (PType == WSC_TypeEnum.TreeParentType.RoleUser_byRole)
                {
                    strSQL = "SELECT DISTINCT RM.LOGIN_NAME AS ID,LOGIN_NAME AS NAME FROM WSC_USER_ROLE RM ";
                    strSQL += " WHERE RM.ROLE_ID='" + ParentID.Trim() + "' AND RM.SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY RM.LOGIN_NAME";
                }
                else if (PType == WSC_TypeEnum.TreeParentType.RoleUser_byUser)
                {
                    strSQL = "SELECT DISTINCT RM.ROLE_ID AS ID,R.ROLE_NAME AS NAME FROM WSC_USER_ROLE RM,WSC_ROLE R ";
                    strSQL += " WHERE RM.LOGIN_NAME='" + ParentID.Trim() + "' AND R.ROLE_ID=RM.ROLE_ID AND RM.SYS_ID='" + GlobalDefinition.System_Name() + "' AND R.SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY RM.ROLE_ID";
                }

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
