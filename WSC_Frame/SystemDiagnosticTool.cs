/*****************************************************************************************************
Author        : Anson.Lin
Date	      : Feb 12,2006
Description   : 
/*****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using WSC.Common;

namespace WSC
{
    /// <summary>
    /// 系统初始化及配置信息诊断工具
    /// </summary>
    public sealed class SystemDiagnosticTool
    {
        // Created by Anson Lin on 12-Feb-2006

        /// <summary>
        /// 系统初始化及配置信息诊断工具        
        /// </summary>
        public SystemDiagnosticTool()
        {}

        /// <summary>
        /// 检查系统配置是否正确  
        /// </summary>
        /// <returns></returns>
        public DataSet Validate(ref string Result)
        {
           
            try
            {
                StringBuilder bstrXML = new StringBuilder("");
                DataSet ds = new DataSet();
                
                using (WSC_DataConn cnn = new WSC_DataConn())
                {
                    using (SqlCommand cmd = cnn.CreateStoreProcedureCommand("SP_SYSTEM_VALIDATE"))
                    {                       
                        cmd.Parameters.Add("@RESULT", SqlDbType.VarChar, 8);
                        cmd.Parameters.Add("@SYS_ID", SqlDbType.VarChar, 100);
                        cmd.Parameters["@SYS_ID"].Value = GlobalDefinition.System_Name();
                        cmd.Parameters["@RESULT"].Direction = ParameterDirection.Output;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);

                        Result = cmd.Parameters["@RESULT"].Value.ToString().Trim();
                        
                    }
                }

                string strC = "";
                try { strC = System.Configuration.ConfigurationManager.AppSettings["SYSTEM_ID"].Trim(); }
                catch {  }
                if (strC == "")
                {
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["SYS_ID"] = GlobalDefinition.System_Name();
                    dr["TABLE_NAME"] = "WEB.CONFIG";
                    dr["FIELD_VALUE"] = "SYSTEM_ID";
                    dr["OK"] = "N";

                    ds.Tables[0].Rows.Add(dr);
                }

                strC = "";
                try { strC = System.Configuration.ConfigurationManager.AppSettings["wscConnectionString"].Trim(); }
                catch {  }
                if (strC == "")
                {
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["SYS_ID"] = GlobalDefinition.System_Name();
                    dr["TABLE_NAME"] = "WEB.CONFIG";
                    dr["FIELD_VALUE"] = "wscConnectionString";
                    dr["OK"] = "N";

                    ds.Tables[0].Rows.Add(dr);
                }
              
                strC = "";
                try { strC = ConfigurationManager.AppSettings["LocalConnectionString"].Trim(); }
                catch {  }

                if (strC == "")
                {
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["SYS_ID"] = GlobalDefinition.System_Name();
                    dr["TABLE_NAME"] = "WEB.CONFIG";
                    dr["FIELD_VALUE"] = "LocalConnectionString";
                    dr["OK"] = "N";

                    ds.Tables[0].Rows.Add(dr);                    
                }
                                
                
                
                
                return ds;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 修复配置信息的错误
        /// </summary>
        /// <returns></returns>
        public DataSet Repaire()
        {                    
            try
            {
                StringBuilder bstrXML = new StringBuilder("");
                DataSet ds = new DataSet();

                using (WSC_DataConn cnn = new WSC_DataConn())
                {
                    string strSQL = "EXEC  SP_SYSTEM_REPAIRE '" + GlobalDefinition.System_Name() + " '";
                    ds = cnn.ExecuteQuery(strSQL);
                }
                
                FileLogger.WriteLog("SystemIL","Repaire", "CONFIG", CommonEnum.LogActionType.Info, "Configurations have been fixed in database.");

                string strC = "";
                try { strC = System.Configuration.ConfigurationManager.AppSettings["SYSTEM_ID"].Trim(); }
                catch { }
                if (strC == "")
                {
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["SYS_ID"] = GlobalDefinition.System_Name();
                    dr["TABLE_NAME"] = "WEB.CONFIG";
                    dr["FIELD_VALUE"] = "SYSTEM_ID";
                    dr["OK"] = "N";
                    dr["FIXED"] = "N";

                    ds.Tables[0].Rows.Add(dr);
                }

                strC = "";
                try { strC = System.Configuration.ConfigurationManager.AppSettings["wscConnectionString"].Trim(); }
                catch { }
                if (strC == "")
                {
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["SYS_ID"] = GlobalDefinition.System_Name();
                    dr["TABLE_NAME"] = "WEB.CONFIG";
                    dr["FIELD_VALUE"] = "wscConnectionString";
                    dr["OK"] = "N";
                    dr["FIXED"] = "Y";

                    ds.Tables[0].Rows.Add(dr);
                }

                strC = "";
                try { strC = ConfigurationManager.AppSettings["LocalConnectionString"].Trim(); }
                catch { }

                if (strC == "")
                {
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["SYS_ID"] = GlobalDefinition.System_Name();
                    dr["TABLE_NAME"] = "WEB.CONFIG";
                    dr["FIELD_VALUE"] = "LocalConnectionString";
                    dr["OK"] = "N";
                    dr["FIXED"] = "N";

                    ds.Tables[0].Rows.Add(dr);
                }


                return ds;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
