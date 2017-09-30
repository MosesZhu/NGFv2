/************************************************************************************************
**********Created by Anson Lin on 26-Jan-2006                                           *********
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

    internal sealed class WSC_Role_Module : I_WSC_FactoryModule
    {     
        public WSC_Role_Module()
        { }

        private string m_strLastError = "";

        public string GetLastError
        {
            get { return m_strLastError; }
        }        
        public string Add(string RoleID, string ModuleID)
        {
            m_strLastError = "";
            if (RoleID.Trim() == "" || ModuleID.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY");//"The parameters can not be empty value!";

            //Check security, Added by Anson on 29-Mar-2006
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                bool bolR = CheckRoleModuleExist(RoleID.Trim(), ModuleID.Trim());
                if (!bolR)
                {
                    return CultureRes.GetSysFrameResource("MSG_ERR_SETTING_EXIST");//"This setting has already existed in the database.";
                }

                FileLogger.WriteLog_WSC("WSC", "ROLE_MODULE_SECURITY", CommonEnum.LogActionType.Add, "SUCCESS! RoleID=" + RoleID.Trim() + "  Module=" + ModuleID.Trim());

                string strSQL = "EXEC SP_WSC_SECURITY_ROLE_MOD_ADD '" + RoleID.Trim() + "','" + ModuleID.Trim() + "','" + GlobalDefinition.System_Name() + "'";
                
                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE_MODULE_SECURITY", CommonEnum.LogActionType.Error, "ERROR: " + ex.Message + "--  RoleID=" + RoleID.Trim() + "  Module=" + ModuleID.Trim());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ADD") + ex.Message;
                return m_strLastError;
            }
        }

     
        public string Update(string RoleID, string ModuleID)
        {
            m_strLastError = "";
            if (RoleID.Trim() == "" || ModuleID.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY");//"The parameters can not be empty value!";

            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE_MODULE_SECURITY", CommonEnum.LogActionType.Update, "SUCCESS! RoleID=" + RoleID.Trim() + "  Module=" + ModuleID.Trim());

                string strSQL = "EXEC SP_WSC_SECURITY_ROLE_MOD_DEL '" + RoleID.Trim() + "','" + ModuleID.Trim() + "','" + GlobalDefinition.System_Name() + "'; ";
                strSQL += "EXEC SP_WSC_SECURITY_ROLE_MOD_ADD '" + RoleID.Trim() + "','" + ModuleID.Trim() + "','" + GlobalDefinition.System_Name() + "'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE_MODULE_SECURITY", CommonEnum.LogActionType.Error, "ERROR: " + ex.Message + "--  RoleID=" + RoleID.Trim() + "  Module=" + ModuleID.Trim());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_UPDATE") + ex.Message;
                return m_strLastError;
            }
        }

      
        public string Delete(string RoleID, string ModuleID)
        {
            m_strLastError = "";
            if (RoleID.Trim() == "" && ModuleID.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY");//"The two parameters can not be all empty values!";

            //Check security, Added by Anson on 29-Mar-2006
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE_MODULE_SECURITY", CommonEnum.LogActionType.Delete, "SUCCESS! RoleID=" + RoleID.Trim() + "  Module=" + ModuleID.Trim());

                string strSQL = "EXEC SP_WSC_SECURITY_ROLE_MOD_DEL '" + RoleID.Trim() + "','" + ModuleID.Trim() + "','" + GlobalDefinition.System_Name() + "' ";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE_MODULE_SECURITY", CommonEnum.LogActionType.Error, "ERROR: " + ex.Message + "--  RoleID=" + RoleID.Trim() + "  Module=" + ModuleID.Trim());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_DELETE") + ex.Message;
                return m_strLastError;
            }
        }      
        private bool CheckRoleModuleExist(string RoleID, string ModuleID)
        {
            m_strLastError = "";
            try
            {
                string strSQL = "SELECT MOD_ID FROM WSC_ROLE_MODULE WHERE SYS_ID='" + GlobalDefinition.System_Name()
                    + "' AND MOD_ID = '" + ModuleID.Trim() + "' AND ROLE_ID='" + RoleID + "'";
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
        public DataSet TreeGetParentItems(WSC_TypeEnum.TreeParentType PType)
        {
            // Created by Anson Lin on 23-Jan-2006
            m_strLastError = "";
            DataSet ds = new DataSet();

            try
            {
                string strSQL = "";
                if (PType == WSC_TypeEnum.TreeParentType.RoleModule_byRole)
                {
                    strSQL = "SELECT DISTINCT RM.ROLE_ID AS ID,R.ROLE_NAME AS NAME FROM WSC_ROLE_MODULE RM,WSC_ROLE R WHERE R.ROLE_ID=RM.ROLE_ID AND RM.SYS_ID='" + GlobalDefinition.System_Name() + "'  AND R.SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY RM.ROLE_ID";
                }
                else if (PType == WSC_TypeEnum.TreeParentType.RoleModule_byModule)
                {
                    strSQL = "SELECT DISTINCT RM.MOD_ID AS ID,M.MOD_NAME AS NAME FROM WSC_ROLE_MODULE RM,FRAME_MODULELIST M WHERE M.MOD_ID=RM.MOD_ID AND RM.SYS_ID='" + GlobalDefinition.System_Name() + "' AND M.SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY RM.MOD_ID";
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

        public DataSet TreeGetChildItems(WSC_TypeEnum.TreeParentType PType, string ParentID)
        {        // Created by Anson Lin on 23-Jan-2006
            m_strLastError = "";
            DataSet ds = new DataSet();

            try
            {
                string strSQL = "";
                if (PType == WSC_TypeEnum.TreeParentType.RoleModule_byRole)
                {
                    strSQL = "SELECT DISTINCT RM.MOD_ID AS ID,M.MOD_NAME AS NAME FROM WSC_ROLE_MODULE RM,FRAME_MODULELIST M,WSC_ROLE R ";
                    strSQL += " WHERE RM.ROLE_ID='" + ParentID.Trim() + "' AND R.ROLE_ID=RM.ROLE_ID AND M.MOD_ID=RM.MOD_ID AND RM.SYS_ID='" + GlobalDefinition.System_Name() + "' AND M.SYS_ID='" + GlobalDefinition.System_Name() + "'  AND R.SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY RM.MOD_ID";
                }
                else if (PType == WSC_TypeEnum.TreeParentType.RoleModule_byModule)
                {
                    strSQL = "SELECT DISTINCT RM.ROLE_ID AS ID,R.ROLE_NAME AS NAME FROM WSC_ROLE_MODULE RM,FRAME_MODULELIST M,WSC_ROLE R ";
                    strSQL += " WHERE RM.MOD_ID='" + ParentID.Trim() + "' AND R.ROLE_ID=RM.ROLE_ID AND M.MOD_ID=RM.MOD_ID AND RM.SYS_ID='" + GlobalDefinition.System_Name() + "' AND M.SYS_ID='" + GlobalDefinition.System_Name() + "'  AND R.SYS_ID='" + GlobalDefinition.System_Name() + "'  ORDER BY RM.ROLE_ID";
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
