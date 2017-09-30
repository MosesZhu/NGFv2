/************************************************************************************************
**********Created by Anson Lin on 14-Feb-2006                                           *********
**********Description:                                                                  *********
*************************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Web;
using WSC.Common;
using WSC.SecurityControl;


namespace WSC.Framework
{
   
    public sealed class ModuleFunction
    {
      
        public ModuleFunction()
        {}

        private string m_strLastError = "";
    
        public string GetLastError
        {
            get { return m_strLastError; }
        }

      
        public DataSet GetValues(string ParentModID, string FunctionID)
        {
            m_strLastError = "";
            DataSet ds = new DataSet();

            try
            {
                string strSQL = "SELECT *,(SELECT MOD_NAME FROM FRAME_MODULELIST B WHERE B.MOD_ID=A.MOD_DESC) AS PMOD_NAME FROM FRAME_MODULELIST A WHERE SYS_ID='" 
                    + GlobalDefinition.System_Name() + "' AND PID='*_*FUNCTION*_*' ";

                if (ParentModID != "")
                    strSQL += " AND MOD_DESC='" + ParentModID.Trim() + "'";

                if (FunctionID != "")
                    strSQL += " AND MOD_ID='" + FunctionID.Trim() + "'";

                strSQL += " ORDER BY MOD_DESC,MOD_ID";

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


        public string Delete(string FunctionID)
        {
            m_strLastError = "";

            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                FileLogger.WriteLog_WSC("FRAME", "FUNCTION", CommonEnum.LogActionType.Delete, "SUCCESS! FunctionID=" + FunctionID.Trim());

                string strSQL = "DELETE FROM FRAME_MODULELIST WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' AND FLAG<>'S' AND MOD_ID='" + FunctionID.Trim() + "'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("FRAME", "FUNCTION", CommonEnum.LogActionType.Error, "Error trying to delete Function[" + FunctionID.Trim() + "]! " + ex.Message);
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
            }
        }

        private bool CheckFunctionExist(string FunctionID)
        {
            m_strLastError = "";
            try
            {
                string strSQL = "SELECT MOD_ID FROM FRAME_MODULELIST WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' AND MOD_ID = '" + FunctionID.Trim() + "'";
                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    SqlDataReader dr = conn.ExecuteReader(strSQL);
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
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return false;
            }
        }

        //Add by AIC21/arty yu on 20120420
        public DataTable GetFunctionID(string pageName)
        {
            m_strLastError = "";
            DataTable dt = null;

            try
            {
                string strSQL = "SELECT mod_id,check_authority  FROM FRAME_MODULELIST where sys_id = '" + GlobalDefinition.System_Name() + "' and address like '%" + pageName + "%'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    DataSet ds = conn.ExecuteQuery(strSQL);
                    dt =  ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
            }

            return dt;
        }
        //End Add by AIC21/arty yu on 20120420


        //Add by AIC21/arty yu on 20120508
        public DataTable GetFunctionByFunctionID(string FunctionID)
        {
            m_strLastError = "";
            DataTable dt = null;

            try
            {
                string strSQL = "SELECT *  FROM FRAME_MODULELIST where sys_id = '" + GlobalDefinition.System_Name() + "' and MOD_ID = '" + FunctionID + "'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    DataSet ds = conn.ExecuteQuery(strSQL);
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
            }

            return dt;
        }
        //End Add by AIC21/arty yu on 20120508


        public string Update(CommonEnum.EditActionType Action, string FunctionID, string Name, string ParentModID)
        {
            m_strLastError = "";

            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                bool bolExist = CheckFunctionExist(FunctionID.Trim());

                string strSQL = "";

                if (Action == CommonEnum.EditActionType.New)
                {
                    if (!bolExist)
                        return CultureRes.GetSysFrameResource("MSG_ERR_SETTING_EXIST");//"This function ID has already existed in database.";
                    

                    FileLogger.WriteLog_WSC("FRAME", "FUNCTION", CommonEnum.LogActionType.Add, "SUCCESS! Function=" + Name.Trim() + "   FuncID=" + FunctionID.Trim());

                    strSQL = "INSERT INTO FRAME_MODULELIST (SYS_ID, MOD_ID, MOD_NAME, MOD_DESC, PID, FLAG) "
                     + "VALUES('" + GlobalDefinition.System_Name() + "','" + "Func_" + FunctionID.Trim() 
                     + "','Func_" + Name.Trim() + "','" + ParentModID.Trim() + "','*_*FUNCTION*_*','N')";

                }
                else if (Action == CommonEnum.EditActionType.Update)
                {
                    if (bolExist)
                        return CultureRes.GetSysFrameResource("MSG_ERR_SETTING_NOTEXIST");// "This function ID does not exist in the database.";
                    
                    FileLogger.WriteLog_WSC("FRAME", "FUNCTION", CommonEnum.LogActionType.Update, "SUCCESS! Function=" + Name.Trim() + "   FuncID=" + FunctionID.Trim());

                    strSQL = "UPDATE FRAME_MODULELIST SET MOD_NAME='" + Name.Trim() + "',MOD_DESC='" + ParentModID.Trim() + "' "
                        + " WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' AND MOD_ID='" + FunctionID.Trim() + "'";
                }

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {                
                FileLogger.WriteLog_WSC("FRAME", "FUNCTION", CommonEnum.LogActionType.Error, "Error trying to operate Function[" + FunctionID.Trim() + "]! " + ex.Message);
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
            }
        }

    }
}
