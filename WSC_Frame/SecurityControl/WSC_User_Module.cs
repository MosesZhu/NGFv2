﻿/************************************************************************************************
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
  
    internal sealed class WSC_User_Module : I_WSC_FactoryModule
    {
       
        public WSC_User_Module()
        { }

        private string m_strLastError = "";     
        public string GetLastError
        {
            get { return m_strLastError; }
        }           
        public string Add(string LoginName, string ModuleID)
        {
            m_strLastError = "";
            if (LoginName.Trim() == "" || ModuleID.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY"); 

            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                bool bolR = CheckUserModuleExist(LoginName.Trim(), ModuleID.Trim());
                if (!bolR)
                {
                    return CultureRes.GetSysFrameResource("MSG_ERR_SETTING_EXIST");                  }

                FileLogger.WriteLog_WSC("WSC", "USER_MODULE_SECURITY", CommonEnum.LogActionType.Add, "SUCCESS!  Module=" + ModuleID.Trim() + "  User=" + LoginName.Trim());

                string strSQL = "EXEC SP_WSC_SECURITY_USER_MOD_ADD '" + LoginName.Trim() + "','" + ModuleID.Trim() + "','" + GlobalDefinition.System_Name() + "'";
               
                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "USER_MODULE_SECURITY", CommonEnum.LogActionType.Error, "Error trying to add User-Module!  " + ex.Message);
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ADD") + "\\r\\n" + ex.Message;
                return m_strLastError;
            }
        }       
        public string Update(string LoginName, string ModuleID)
        {
            m_strLastError = "";
            if (LoginName.Trim() == "" || ModuleID.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY");
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");
            try
            {
                FileLogger.WriteLog_WSC("WSC", "USER_MODULE_SECURITY", CommonEnum.LogActionType.Update, "SUCCESS!  Module=" + ModuleID.Trim() + "  User=" + LoginName.Trim());
                string strSQL = "EXEC SP_WSC_SECURITY_USER_MOD_DEL '" + LoginName.Trim() + "','" + ModuleID.Trim() + "','" + GlobalDefinition.System_Name() + "'; ";
                strSQL += "EXEC SP_WSC_SECURITY_USER_MOD_ADD '" + LoginName.Trim() + "','" + ModuleID.Trim() + "','" + GlobalDefinition.System_Name() + "'";
                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "USER_MODULE_SECURITY", CommonEnum.LogActionType.Error, "Error trying to update User-Module!  " + ex.Message);
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_UPDATE") + "\\r\\n" + ex.Message;
                return m_strLastError;
            }
        }


        public string Delete(string LoginName, string ModuleID)
        {
            m_strLastError = "";
            if (LoginName.Trim() == "" && ModuleID.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY"); // "The two parameters can not be all empty values!";

            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                FileLogger.WriteLog_WSC("WSC", "USER_MODULE_SECURITY", CommonEnum.LogActionType.Delete, "SUCCESS!  Module=" + ModuleID.Trim() + "  User=" + LoginName.Trim());

                string strSQL = "EXEC SP_WSC_SECURITY_USER_MOD_DEL '" + LoginName.Trim() + "','" + ModuleID.Trim() + "','" + GlobalDefinition.System_Name() + "' ";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "USER_MODULE_SECURITY", CommonEnum.LogActionType.Error, "Error trying to delete User-Module!  " + ex.Message);
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_DELETE") + "\\r\\n" + ex.Message;
                return m_strLastError;
            }
        }


        private bool CheckUserModuleExist(string LoginName, string ModuleID)
        {
            m_strLastError = "";
            try
            {
                string strSQL = "SELECT MOD_ID FROM WSC_USER_MODULE WHERE SYS_ID='" + GlobalDefinition.System_Name()
                    + "' AND MOD_ID = '" + ModuleID.Trim() + "' AND LOGIN_NAME='" + LoginName + "'";
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
            m_strLastError = "";
            DataSet ds = new DataSet();

            try
            {
                string strSQL = "";
                if (PType == WSC_TypeEnum.TreeParentType.UserModule_byUser)
                {
                    strSQL = "SELECT DISTINCT LOGIN_NAME AS ID,LOGIN_NAME AS NAME FROM WSC_USER_MODULE WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY LOGIN_NAME";
                }
                else if (PType == WSC_TypeEnum.TreeParentType.UserModule_byModule)
                {
                    strSQL = "SELECT DISTINCT RM.MOD_ID AS ID,M.MOD_NAME AS NAME FROM WSC_USER_MODULE RM,FRAME_MODULELIST M WHERE M.MOD_ID=RM.MOD_ID AND RM.SYS_ID='" + GlobalDefinition.System_Name() + "' AND M.SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY RM.MOD_ID";
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
        {
            m_strLastError = "";
            DataSet ds = new DataSet();

            try
            {
                string strSQL = "";
                if (PType == WSC_TypeEnum.TreeParentType.UserModule_byUser)
                {
                    strSQL = "SELECT DISTINCT RM.MOD_ID AS ID,M.MOD_NAME AS NAME FROM WSC_USER_MODULE RM,FRAME_MODULELIST M ";
                    strSQL += " WHERE RM.LOGIN_NAME='" + ParentID.Trim() + "' AND M.MOD_ID=RM.MOD_ID AND RM.SYS_ID='" + GlobalDefinition.System_Name() + "' AND M.SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY RM.MOD_ID";
                }
                else if (PType == WSC_TypeEnum.TreeParentType.UserModule_byModule)
                {
                    strSQL = "SELECT DISTINCT LOGIN_NAME AS ID,LOGIN_NAME AS NAME FROM WSC_USER_MODULE  ";
                    strSQL += " WHERE MOD_ID='" + ParentID.Trim() + "' AND SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY LOGIN_NAME";
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
