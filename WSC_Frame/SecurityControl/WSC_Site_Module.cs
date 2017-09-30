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
using System.Collections;

namespace WSC.SecurityControl
{

    internal sealed class WSC_Site_Module : I_WSC_FactoryModule
    {
        public WSC_Site_Module()
        { }

        private string m_strLastError = "";

        public string GetLastError
        {
            get { return m_strLastError; }
        }        
        public string Add(string site, string ModuleID)
        {
            m_strLastError = "";
            if (site.Trim() == "" || ModuleID.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY");//"The parameters can not be empty value!";

            //Check security, Added by Anson on 29-Mar-2006
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                bool bolR = CheckSiteModuleExist(site.Trim(), ModuleID.Trim());
                if (!bolR)
                {
                    return CultureRes.GetSysFrameResource("MSG_ERR_SETTING_EXIST");//"This setting has already existed in the database.";
                }

                FileLogger.WriteLog_WSC("WSC", "SITE_MODULE_SECURITY", CommonEnum.LogActionType.Add, "SUCCESS! site=" + site.Trim() + "  Module=" + ModuleID.Trim());

                string strSQL = "INSERT INTO WSC_SITE_MODULE(SITE,MOD_ID,SYS_ID)  values('" + site.Trim() + "','" + ModuleID.Trim() + "','" + GlobalDefinition.System_Name() + "')";
                
                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "SITE_MODULE_SECURITY", CommonEnum.LogActionType.Error, "ERROR: " + ex.Message + "--  site=" + site.Trim() + "  Module=" + ModuleID.Trim());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ADD") + ex.Message;
                return m_strLastError;
            }
        }

     
        public string Update(string site, string ModuleID)
        {
            m_strLastError = "";
            if (site.Trim() == "" || ModuleID.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY");//"The parameters can not be empty value!";

            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                FileLogger.WriteLog_WSC("WSC", "SITE_MODULE_SECURITY", CommonEnum.LogActionType.Update, "SUCCESS! site=" + site.Trim() + "  Module=" + ModuleID.Trim());

                string strSQL = "delete from  WSC_SITE_MODULE where SITE='" + site.Trim() + "' and MOD_ID='" + ModuleID.Trim() + "' and SYS_ID='" + GlobalDefinition.System_Name() + "'; ";
                strSQL += "INSERT INTO WSC_SITE_MODULE(SITE,MOD_ID,SYS_ID)  values('" + site.Trim() + "','" + ModuleID.Trim() + "','" + GlobalDefinition.System_Name() + "')";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "SITE_MODULE_SECURITY", CommonEnum.LogActionType.Error, "ERROR: " + ex.Message + "--  site=" + site.Trim() + "  Module=" + ModuleID.Trim());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_UPDATE") + ex.Message;
                return m_strLastError;
            }
        }

      
        public string Delete(string site, string ModuleID)
        {
            m_strLastError = "";
            if (site.Trim() == "" && ModuleID.Trim() == "")
                return CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY");//"The two parameters can not be all empty values!";

            //Check security, Added by Anson on 29-Mar-2006
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                FileLogger.WriteLog_WSC("WSC", "SITE_MODULE_SECURITY", CommonEnum.LogActionType.Delete, "SUCCESS! site=" + site.Trim() + "  Module=" + ModuleID.Trim());

                string strSQL = "delete from  WSC_SITE_MODULE where SITE='" + site.Trim() + "' and MOD_ID='" + ModuleID.Trim() + "' and SYS_ID='" + GlobalDefinition.System_Name() + "' ";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("WSC", "SITE_MODULE_SECURITY", CommonEnum.LogActionType.Error, "ERROR: " + ex.Message + "--  site=" + site.Trim() + "  Module=" + ModuleID.Trim());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_DELETE") + ex.Message;
                return m_strLastError;
            }
        }      

        private bool CheckSiteModuleExist(string site, string ModuleID)
        {
            m_strLastError = "";
            try
            {
                string strSQL = "SELECT MOD_ID FROM WSC_SITE_MODULE WHERE SYS_ID='" + GlobalDefinition.System_Name()
                    + "' AND MOD_ID = '" + ModuleID.Trim() + "' AND SITE='" + site + "'";
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
                if (PType == WSC_TypeEnum.TreeParentType.SiteModule_bySite)
                {
                    strSQL = "SELECT DISTINCT RM.SITE AS ID,RM.SITE AS NAME FROM WSC_SITE_MODULE RM WHERE RM.SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY RM.SITE";
                }
                else if (PType == WSC_TypeEnum.TreeParentType.SiteModule_byModule)
                {
                    strSQL = "SELECT DISTINCT RM.MOD_ID AS ID,M.MOD_NAME AS NAME FROM WSC_SITE_MODULE RM,FRAME_MODULELIST M WHERE M.MOD_ID=RM.MOD_ID AND RM.SYS_ID='" + GlobalDefinition.System_Name() + "' AND M.SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY RM.MOD_ID";
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
                if (PType == WSC_TypeEnum.TreeParentType.SiteModule_bySite)
                {
                    strSQL = "SELECT DISTINCT RM.MOD_ID AS ID,M.MOD_NAME AS NAME FROM WSC_SITE_MODULE RM,FRAME_MODULELIST M ";
                    strSQL += " WHERE RM.SITE='" + ParentID.Trim() + "' AND M.MOD_ID=RM.MOD_ID AND RM.SYS_ID='" + GlobalDefinition.System_Name() + "' AND M.SYS_ID='" + GlobalDefinition.System_Name() + "' ORDER BY RM.MOD_ID";
                }
                else if (PType == WSC_TypeEnum.TreeParentType.SiteModule_byModule)
                {
                    strSQL = "SELECT DISTINCT RM.SITE AS ID,RM.SITE AS NAME FROM WSC_SITE_MODULE RM,FRAME_MODULELIST M ";
                    strSQL += " WHERE RM.MOD_ID='" + ParentID.Trim() + "' AND M.MOD_ID=RM.MOD_ID AND RM.SYS_ID='" + GlobalDefinition.System_Name() + "' AND M.SYS_ID='" + GlobalDefinition.System_Name() + "'  ORDER BY RM.SITE";
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

        //Add by AIC21/arty yu on 20120502
        public Hashtable GetModeIdBySite(string site)
        {
            m_strLastError = "";
            Hashtable hashtableMode = new Hashtable();

            try
            {
                string strSQL = "select mod_id from WSC_SITE_MODULE where sys_id = '" + GlobalDefinition.System_Name() + "' and site= '" + site + "'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    DataSet ds = conn.ExecuteQuery(strSQL);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string temp = ds.Tables[0].Rows[i]["mod_id"].ToString().Trim();
                            if (temp != "")
                            {
                                if (!hashtableMode.ContainsKey(temp))
                                {
                                    hashtableMode.Add(temp, temp);
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
            }

            return hashtableMode;
        }
        //End Add by AIC21/arty yu on 20120502

        //Add by AIC21/arty yu on 20120511
        public int GetSiteModeIdCount()
        {
            m_strLastError = "";

            try
            {
                string strSQL = "select count(*) as counts from WSC_SITE_MODULE where sys_id = '" + GlobalDefinition.System_Name()+"'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    DataSet ds = conn.ExecuteQuery(strSQL);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        return int.Parse(ds.Tables[0].Rows[0]["counts"].ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
            }

            return -1;
        }
        //End Add by AIC21/arty yu on 20120511

        public string WSCCurrentSite(string strLoginName)
        {
            try
            {
                string strViewQuery = "getAmEmployee('" + GlobalDefinition.System_Name() + "','" + strLoginName + "')";
                string strSql = "SELECT SITE_CODE FROM " + strViewQuery + " WHERE UPPER(REPLACE(REPLACE(LOGIN_NAME,' ',''), '.',''))=REPLACE(REPLACE('" + strLoginName.Trim().ToUpper() + "',' ',''), '.','') AND ACTIVE='Y'";
                
                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.GetValue(strSql, 0).Trim();
                }
            }
            catch
            {
                return "";
            }
        }
    }
}
