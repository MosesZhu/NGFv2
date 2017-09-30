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
    /// 部门权限维护
    /// Created by Anson Lin on 3-Feb-2006 
    /// </summary>
    internal sealed class WSC_Dept_Module : I_WSC_FactoryModule
    {
        private string m_strLastError = "";
      
        /// <summary>
        /// 部门权限维护
        /// Created by Anson Lin on 3-Feb-2006 
        /// </summary>
        public WSC_Dept_Module()
        { }

        /// <summary>
        /// Last error
        /// </summary>
        public string GetLastError
        {
            get { return m_strLastError; }
        }


        /// <summary>
        /// Add one new Dept-Module security.
        /// </summary>
        /// <param name="Dept"></param>
        /// <param name="ModuleID"></param>
        /// <returns>SUCCESS|Error message</returns>
        public string Add(string Dept, string ModuleID)
        {            
            m_strLastError = "";
            if (Dept.Trim() == "" || ModuleID.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY"); //"The parameters can not be empty value!";

            //Check security, Added by Anson on 29-Mar-2006
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                bool bolR = CheckDeptModuleExist(Dept.Trim(), ModuleID.Trim());
                if (!bolR)
                {
                    return CultureRes.GetSysFrameResource("MSG_ERR_SETTING_EXIST"); //"This setting has already existed in the database.";
                }

                FileLogger.WriteLog_WSC("WSC", "DEPT_MODULE_SECURITY", CommonEnum.LogActionType.Add, "SUCCESS! Department=" + Dept.Trim() + "  Module=" + ModuleID.Trim());

                string strSQL = "EXEC SP_WSC_SECURITY_DEPT_MOD_ADD '" + Dept.Trim() + "','" + ModuleID.Trim() + "','" + GlobalDefinition.System_Name() + "'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC","DEPT_MODULE_SECURITY", CommonEnum.LogActionType.Error, "ERROR: " + ex.Message + "-- Department=" + Dept.Trim() + "  Module=" + ModuleID.Trim());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ADD") + ",  " + ex.Message;
                return m_strLastError;
            }
        }

        /// <summary>
        /// Revise the Dept-Module security.
        /// </summary>
        /// <param name="Dept"></param>
        /// <param name="ModuleID"></param>
        public string Update(string Dept, string ModuleID)
        {
            m_strLastError = "";
            if (Dept.Trim() == "" || ModuleID.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY"); //"The parameters can not be empty value!";

            //Check security, Added by Anson on 29-Mar-2006
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                FileLogger.WriteLog_WSC("WSC", "DEPT_MODULE_SECURITY", CommonEnum.LogActionType.Update, "SUCCESS! Department=" + Dept.Trim() + "  Module=" + ModuleID.Trim());

                string strSQL = "EXEC SP_WSC_SECURITY_DEPT_MOD_DEL '" + Dept.Trim() + "','" + ModuleID.Trim() + "','" + GlobalDefinition.System_Name() + "'; ";
                strSQL += "EXEC SP_WSC_SECURITY_DEPT_MOD_ADD '" + Dept.Trim() + "','" + ModuleID.Trim() + "','" + GlobalDefinition.System_Name() + "'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "DEPT_MODULE_SECURITY", CommonEnum.LogActionType.Error, "ERROR: " + ex.Message + "-- Department=" + Dept.Trim() + "  Module=" + ModuleID.Trim());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_UPDATE") + ", " + ex.Message;
                return m_strLastError;
            }
        }

        /// <summary>
        /// Delete the Dept-Module.security.
        /// </summary>
        /// <param name="Dept"></param>   
        /// <param name="ModuleID"></param>
        public string Delete(string Dept, string ModuleID)
        {            
            m_strLastError = "";
            if (Dept.Trim() == "" && ModuleID.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY"); //"The two parameters can not be all empty values!";

            //Check security, Added by Anson on 29-Mar-2006
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                FileLogger.WriteLog_WSC("WSC", "DEPT_MODULE_SECURITY", CommonEnum.LogActionType.Delete, "SUCCESS! Department=" + Dept.Trim() + "  Module=" + ModuleID.Trim());

                string strSQL = "EXEC SP_WSC_SECURITY_DEPT_MOD_DEL '" + Dept.Trim() + "','" + ModuleID.Trim() + "','" + GlobalDefinition.System_Name() + "' ";
               
                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "DEPT_MODULE_SECURITY", CommonEnum.LogActionType.Error, "ERROR: " + ex.Message + "-- Department=" + Dept.Trim() + "  Module=" + ModuleID.Trim());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_DELETE") + ", " + ex.Message;
                return m_strLastError;
            }
        }


        /// <summary>
        /// 检查该部门权限是否已存在于数据库中
        /// </summary>
        /// <param name="Dept"></param>
        /// <param name="ModuleID"></param>
        /// <returns></returns>
        private bool CheckDeptModuleExist(string Dept, string ModuleID)
        {
            m_strLastError = "";
            try
            {
                string strSQL = "SELECT MOD_ID FROM WSC_DEPT_MODULE WHERE SYS_ID='" + GlobalDefinition.System_Name()
                    + "' AND MOD_ID = '" + ModuleID.Trim() + "' AND DEPT_CODE='" + Dept + "'";
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
        /// Retrieve all the parent .
        /// Created by Anson Lin on 23-Jan-2006
        /// </summary>
        /// <param name="PType">Parent type</param>
        /// <returns></returns>
        public DataSet TreeGetParentItems(WSC_TypeEnum.TreeParentType PType)
        {                       
            m_strLastError = "";
            DataSet ds = new DataSet();

            try
            {
                string strSQL = "";
                if (PType == WSC_TypeEnum.TreeParentType.DeptModule_byDept )
                {
                    strSQL = "SELECT DISTINCT DEPT_CODE AS ID,DEPT_CODE AS NAME FROM WSC_DEPT_MODULE WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY DEPT_CODE";
                }
                else if (PType == WSC_TypeEnum.TreeParentType.DeptModule_byModule)
                {
                    strSQL = "SELECT DISTINCT RM.MOD_ID AS ID,M.MOD_NAME AS NAME FROM WSC_DEPT_MODULE RM,FRAME_MODULELIST M WHERE M.MOD_ID=RM.MOD_ID AND RM.SYS_ID='" + GlobalDefinition.System_Name() + "' AND M.SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY RM.MOD_ID";
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
        /// Retrieve the child items by parent.
        /// Created by Anson Lin on 23-Jan-2006
        /// </summary>
        /// <param name="PType">Parent type</param>
        /// <param name="ParentID">Parent ID</param>
        /// <returns></returns>
        public DataSet TreeGetChildItems(WSC_TypeEnum.TreeParentType PType, string ParentID)
        {
            m_strLastError = "";
            DataSet ds = new DataSet();

            try
            {
                string strSQL = "";
                if (PType == WSC_TypeEnum.TreeParentType.DeptModule_byDept)
                {
                    strSQL = "SELECT DISTINCT RM.MOD_ID AS ID,M.MOD_NAME AS NAME FROM WSC_DEPT_MODULE RM,FRAME_MODULELIST M ";
                    strSQL += " WHERE RM.DEPT_CODE='" + ParentID.Trim() + "' AND M.MOD_ID=RM.MOD_ID AND RM.SYS_ID='" + GlobalDefinition.System_Name() + "' AND M.SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY RM.MOD_ID";
                }
                else if (PType == WSC_TypeEnum.TreeParentType.DeptModule_byModule)
                {
                    strSQL = "SELECT DISTINCT DEPT_CODE AS ID,DEPT_CODE AS NAME FROM WSC_DEPT_MODULE  ";
                    strSQL += " WHERE MOD_ID='" + ParentID.Trim() + "' AND SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY DEPT_CODE";
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
