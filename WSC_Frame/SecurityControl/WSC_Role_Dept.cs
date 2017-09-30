/************************************************************************************************
**********Created by Anson Lin on 29-Mar-2006                                           *********
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
    /// <summary>
    /// 角色权与部门
    /// </summary>
    internal sealed class WSC_Role_Dept : I_WSC_FactoryModule
    {/// <summary>
        /// 角色权与部门
        /// </summary>
        public WSC_Role_Dept()
        { }

        private string m_strLastError = "";

        public string GetLastError
        {
            get { return m_strLastError; }
        }

        /// <summary>
        /// Add one new Role-Dept security.
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="DeptCode"></param>
        public string Add(string RoleID, string DeptCode)
        {
            //Added by Anson on 29-Mar-2006
            m_strLastError = "";
            if (RoleID.Trim() == "" || DeptCode.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY");//"The parameters can not be empty value!";

            //Check security, Added by Anson on 29-Mar-2006
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                bool bolR = CheckRoleDeptExist(RoleID.Trim(), DeptCode.Trim());
                if (!bolR)
                {
                    return CultureRes.GetSysFrameResource("MSG_ERR_SETTING_EXIST");//"This setting has already existed in the database.";
                }

                FileLogger.WriteLog_WSC("WSC", "ROLE_DEPT_SECURITY", CommonEnum.LogActionType.Add, "SUCCESS! RoleID=" + RoleID.Trim() + "  Dept=" + DeptCode.Trim());

                string strSQL = "EXEC SP_WSC_SECURITY_ROLE_DEPT_ADD '" + RoleID.Trim() + "','" + DeptCode.Trim() + "','" + GlobalDefinition.System_Name() + "'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE_DEPT_SECURITY", CommonEnum.LogActionType.Error, "ERROR: " + ex.Message + "--  RoleID=" + RoleID.Trim() + "  Dept=" + DeptCode.Trim());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ADD") + ex.Message;
                return m_strLastError;
            }
        }

        /// <summary>
        /// Revise the Role-Dept security.Return SUCCESS|Error message.
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="DeptCode"></param>
        /// <returns>SUCCESS|Error message</returns>
        public string Update(string RoleID, string DeptCode)
        {
            //Added by Anson on 29-Mar-2006
            m_strLastError = "";
            if (RoleID.Trim() == "" || DeptCode.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY");//"The parameters can not be empty value!";

            //Check security, Added by Anson on 29-Mar-2006
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE_DEPT_SECURITY", CommonEnum.LogActionType.Update, "SUCCESS! RoleID=" + RoleID.Trim() + "  Dept=" + DeptCode.Trim());

                string strSQL = "EXEC SP_WSC_SECURITY_ROLE_DEPT_DEL '" + RoleID.Trim() + "','" + DeptCode.Trim() + "','" + GlobalDefinition.System_Name() + "'; ";
                strSQL += "EXEC SP_WSC_SECURITY_ROLE_DEPT_ADD '" + RoleID.Trim() + "','" + DeptCode.Trim() + "','" + GlobalDefinition.System_Name() + "'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE_DEPT_SECURITY", CommonEnum.LogActionType.Error, "ERROR: " + ex.Message + "--  RoleID=" + RoleID.Trim() + "  Dept=" + DeptCode.Trim());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_UPDATE") + ex.Message;
                return m_strLastError;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="RoleID"></param>   
        /// <param name="DeptCode"></param>
        /// <returns>SUCCESS|Error message</returns>
        public string Delete(string RoleID, string DeptCode)
        {
            //Added by Anson on 29-Mar-2006
            m_strLastError = "";
            if (RoleID.Trim() == "" && DeptCode.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY");//"The two parameters can not be all empty values!";

            //Check security, Added by Anson on 29-Mar-2006
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE_DEPT_SECURITY", CommonEnum.LogActionType.Delete, "SUCCESS! RoleID=" + RoleID.Trim() + "  Dept=" + DeptCode.Trim());

                string strSQL = "EXEC SP_WSC_SECURITY_ROLE_DEPT_DEL '" + RoleID.Trim() + "','" + DeptCode.Trim() + "','" + GlobalDefinition.System_Name() + "' ";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "ROLE_DEPT_SECURITY", CommonEnum.LogActionType.Error, "ERROR: " + ex.Message + "--  RoleID=" + RoleID.Trim() + "  Dept=" + DeptCode.Trim());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_DELETE") + ex.Message;
                return m_strLastError + ex.Message;
            }
        }

        
        /// <summary>
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="DeptCode"></param>
        /// <returns></returns>
        private bool CheckRoleDeptExist(string RoleID, string DeptCode)
        {
            //Added by Anson on 29-Mar-2006
            m_strLastError = "";
            try
            {
                string strSQL = "SELECT ROLE_ID FROM WSC_ROLE_DEPT WHERE SYS_ID='" + GlobalDefinition.System_Name()
                    + "' AND DEPT_CODE = '" + DeptCode.Trim() + "' AND ROLE_ID='" + RoleID + "'";
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
        /// </summary>
        /// <param name="PType">Parent type</param>
        /// <returns></returns>
        public DataSet TreeGetParentItems(WSC_TypeEnum.TreeParentType PType)
        {
            //Added by Anson on 29-Mar-2006
            m_strLastError = "";
            DataSet ds = new DataSet();

            try
            {
                string strSQL = "";
                if (PType == WSC_TypeEnum.TreeParentType.RoleDept_byRole)
                {
                    strSQL = "SELECT DISTINCT RM.ROLE_ID AS ID,R.ROLE_NAME AS NAME FROM WSC_ROLE_DEPT RM,WSC_ROLE R WHERE R.ROLE_ID=RM.ROLE_ID AND RM.SYS_ID='" + GlobalDefinition.System_Name() + "' AND R.SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY RM.ROLE_ID";
                }
                else if (PType == WSC_TypeEnum.TreeParentType.RoleDept_byDept)
                {
                    strSQL = "SELECT DISTINCT RM.DEPT_CODE AS ID,DEPT_CODE AS NAME FROM WSC_ROLE_DEPT RM WHERE RM.SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY RM.DEPT_CODE";
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
        /// </summary>
        /// <param name="PType">Parent type</param>
        /// <param name="ParentID">Parent ID</param>
        /// <returns></returns>
        public DataSet TreeGetChildItems(WSC_TypeEnum.TreeParentType PType, string ParentID)
        {
            //Added by Anson on 29-Mar-2006
            m_strLastError = "";
            DataSet ds = new DataSet();

            try
            {
                string strSQL = "";
                if (PType == WSC_TypeEnum.TreeParentType.RoleDept_byRole)
                {
                    strSQL = "SELECT DISTINCT RM.DEPT_CODE AS ID,DEPT_CODE AS NAME FROM WSC_ROLE_DEPT RM ";
                    strSQL += " WHERE RM.ROLE_ID='" + ParentID.Trim() + "' AND RM.SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY RM.DEPT_CODE";
                }
                else if (PType == WSC_TypeEnum.TreeParentType.RoleDept_byDept)
                {
                    strSQL = "SELECT DISTINCT RM.ROLE_ID AS ID,R.ROLE_NAME AS NAME FROM WSC_ROLE_DEPT RM,WSC_ROLE R ";
                    strSQL += " WHERE RM.DEPT_CODE='" + ParentID.Trim() + "' AND R.ROLE_ID=RM.ROLE_ID AND RM.SYS_ID='" + GlobalDefinition.System_Name() + "' AND R.SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY RM.ROLE_ID";
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
