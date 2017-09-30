/************************************************************************************************
**********Created by Anson Lin on 18-Jan-2006                                           *********
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

    public class ParameterSetup
    {
        private string m_strLastError = "";
      
    
        public ParameterSetup()
        { }

      
        public string GetLastError
        {
            get { return m_strLastError; }
        }

        public DataSet GetValues(string ParaName)
        {
            //Created by Anson Lin on 18-Jan-2006 
            m_strLastError = "";
            DataSet ds = new DataSet();
                        try
            {
                ParaName = ParaName.Trim();

                string strSQL = "SELECT * FROM SYSTEM_CONFIG WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' ";
                
                if (ParaName != "" && ParaName != null)
                    strSQL += " AND CFG_NAME='" + ParaName + "'";

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
              
        public string GetValue(string ParaName)
        {
            //Created by Anson Lin on 18-Jan-2006 
            m_strLastError = "";
            try
            {
                ParaName = ParaName.Trim();

                string strR = "";
                string strSQL = "SELECT CFG_VALUE FROM SYSTEM_CONFIG WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' ";
                              
                strSQL += " AND CFG_NAME='" + ParaName + "'";
                                
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
        public DataSet GetValues(int paramId)
        {
            m_strLastError = "";
            DataSet ds = new DataSet();

            try
            {
                string strSQL = "SELECT * FROM SYSTEM_CONFIG WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' ";
                strSQL += " AND CFG_ID=" + paramId.ToString();

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
        internal string GetSystemValue(string ParaName)
        {
            //Created by Anson Lin on 18-Jan-2006 
            m_strLastError = "";
            try
            {
                ParaName = ParaName.Trim();
                string strR = "";
                string strSQL = "SELECT CFG_VALUE FROM SYSTEM_CONFIG WHERE SYS_ID='SYS' ";
                
                strSQL += " AND CFG_NAME='" + ParaName + "'";
                
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

        public DataTable GetSystemValues(string ParaName)
        {
            //Created by Anson Lin on 18-Jan-2006 
            DataTable returnValue = null;

            m_strLastError = "";
            try
            {
                ParaName = ParaName.Trim();

                string strSQL = "SELECT CFG_VALUE FROM SYSTEM_CONFIG WHERE SYS_ID='SYS' ";

                strSQL += " AND CFG_NAME='" + ParaName + "'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    returnValue = conn.ExecuteQuery(strSQL).Tables[0];
                }

            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
            }

            return returnValue;
        }
        
        public string Delete(int paramId)
        {
            //Created by Anson Lin on 18-Jan-2006 
            m_strLastError = "";

            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                //ParaName = ParaName.Trim();

                FileLogger.WriteLog_WSC("FRAME", "CONFIG", CommonEnum.LogActionType.Delete, "SUCCESS!  ID=" + paramId.ToString());

                string strSQL = "DELETE FROM SYSTEM_CONFIG WHERE IS_SYS='N' AND SYS_ID='" + GlobalDefinition.System_Name() + "' AND CFG_ID=" + paramId.ToString();

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("FRAME", "CONFIG", CommonEnum.LogActionType.Error, "Error: " + ex.Message + "--  ID=" + paramId.ToString());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return m_strLastError;
            }
        }
        public string Update(CommonEnum.EditActionType Action, string ParaName, string ParaValue, string ParaDesc, int paramId)
        {
            //Created by Anson Lin on 18-Jan-2006
            m_strLastError = "";

            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                ParaName = ParaName.Trim();

                string strSQL = "";
                if (Action ==  CommonEnum.EditActionType.New)
                {
                    FileLogger.WriteLog_WSC("FRAME", "CONFIG", CommonEnum.LogActionType.Add, "SUCCESS!  -- Name=" + ParaName + "   Value=" + ParaValue);

                    strSQL = "INSERT INTO SYSTEM_CONFIG(SYS_ID,CFG_NAME,CFG_VALUE,CFG_DESC,IS_SYS) VALUES('" + GlobalDefinition.System_Name() + "','" + ParaName + "','" + ParaValue + "','" + ParaDesc + "','N')";
                }
                else if (Action == CommonEnum.EditActionType.Update)
                {
                    FileLogger.WriteLog_WSC("FRAME", "CONFIG", CommonEnum.LogActionType.Update, "SUCCESS!  -- ID=" + paramId.ToString() + "   Value=" + ParaValue);

                    strSQL = "UPDATE SYSTEM_CONFIG SET CFG_VALUE='" + ParaValue + "',CFG_DESC='" + ParaDesc + "' WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' AND CFG_ID=" + paramId.ToString();
                }

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("FRAME", "CONFIG", CommonEnum.LogActionType.Error, "Error: " + ex.Message + "--  Name=" + ParaName.Trim() + "   Value=" + ParaValue + "   ID=" + paramId.ToString());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return m_strLastError;
            }
        }
    }
}