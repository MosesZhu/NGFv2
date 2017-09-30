/************************************************************************************************
**********Created by Anson Lin on 23-Jan-2006                                           *********
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
using System.Collections.Generic;
using System.Collections;

namespace WSC.Framework
{  
    public sealed class NavigatingTreeData
    {
        private string m_strLastError = "";
        public NavigatingTreeData()
        {}

        public string GetLastError
        {
            get { return m_strLastError; }
        }
        internal DataSet TreeGetChildItemsByParentID(string SysID, string strRtValue)
        {
            // Created by Anson Lin on 23-Jan-2006
            m_strLastError = "";
            DataSet ds = new DataSet();

            try
            {
               
                string strCheckSysMenu = "N";
                if (strRtValue != null && strRtValue !="")
                {
                    strCheckSysMenu = "Y";
                }
                string strSQL = "EXEC  SP_FRAME_MENU_CHILDREN_GET_For_Language '" + SysID.Trim() + "','" + GlobalDefinition.Cookie_LoginUser + "','" + strCheckSysMenu + "','" + GlobalDefinition.CurrentCulture.ToString() + "' ";//Modify by AIC21/arty.yu on 20120412
                                    
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

        internal DataSet TreeGetChildItemsByParentID_WithoutSysMenu(string ParentID)
        {
            // Created by Anson Lin on 23-Jan-2006
            m_strLastError = "";
            DataSet ds = new DataSet();

            try
            {
                if (ParentID.Trim() == "")
                {
                    m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY");//"The parameter ParentID can't be empty value.";
                    return null;
                }
                else
                {
                    string strSQL = "SELECT * FROM FRAME_MODULELIST "
                          + " WHERE SYS_ID='" + GlobalDefinition.System_Name()
                          + "' AND PID='" + ParentID.Trim() + "' AND PID<>'*_*FUNCTION*_*' ORDER BY SORT";

                    using (WSC_DataConn conn = new WSC_DataConn())
                    {
                        ds = conn.ExecuteQuery(strSQL);
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return ds;
            }
        }

      
        public DataSet GetNavigatingItems(string ModuleID)
        {
            // Created by Anson Lin on 23-Jan-2006
            m_strLastError = "";
            DataSet ds = new DataSet();

            try
            {
                string strSQL = "SELECT * FROM FRAME_MODULELIST WHERE SYS_ID='" + GlobalDefinition.System_Name() + "'";
                    //+ "' AND PID<>'*_*FUNCTION*_*' ";

                if(ModuleID!="")
                    strSQL += " AND MOD_ID='" + ModuleID.Trim() + "'";

                strSQL += " ORDER BY SORT";

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


        public string Delete(string ModuleID)
        {
            // Created by Anson Lin on 23-Jan-2006
            
            //Check , Added by Anson on 29-Mar-2006
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            m_strLastError = "";
            try
            {
                if (ModuleID.Trim() == "")
                {
                    m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_VALUE_EMPTY");// "The parameter ParentID can not be empty value.";
                    return null;
                }
                else
                {
                    FileLogger.WriteLog_WSC("FRAME", "NAV_TREE", CommonEnum.LogActionType.Delete, "SUCCESS! ModuleID=" + ModuleID.Trim());

                    string strSQL = "EXEC  SP_FRAME_MENU_DEL '" + GlobalDefinition.System_Name() + "','" + ModuleID.Trim() + "'";

                    using (WSC_DataConn conn = new WSC_DataConn())
                    {
                        string strSQLSite = "DELETE FROM WSC_SITE_MODULE  WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' AND MOD_ID='" + ModuleID.Trim() + "'";
                        conn.ExecuteSqlNonQuery(strSQLSite);
                        return conn.ExecuteSqlNonQuery(strSQL);
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("FRAME", "NAV_TREE", CommonEnum.LogActionType.Error, "Error! " + ex.Message + "  -- ModuleID=" + ModuleID.Trim());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return m_strLastError;
            }
        }
        
        private bool CheckNavigatingItemExist(string ID)
        {
            // Created by Anson Lin on 23-Jan-2006
            m_strLastError = "";
            try
            {
                string strSQL = "SELECT MOD_ID FROM FRAME_MODULELIST WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' AND MOD_ID = '" + ID.Trim() + "'";
                
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


        public int GetMaxSerialOrder()
        {
            // Created by Anson Lin on 23-Jan-2006          
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                throw new Exception(Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT"));

            m_strLastError = "";
            try
            {
                string strMaxOrder = "0";
                int intMaxOrder = 0;
                string strSQL = "SELECT MAX(SORT) + 1 FROM FRAME_MODULELIST WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' " 
                    + " AND PID<>'*_*FUNCTION*_*' ";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    using (SqlDataReader dr = conn.ExecuteReader(strSQL))
                    {
                        if (dr.Read())
                            strMaxOrder = dr.GetValue(0).ToString().Trim();
                        dr.Close();
                    }
                }                

                try { intMaxOrder = int.Parse(strMaxOrder); }
                catch { }

                return intMaxOrder;
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return 0;
            }
        }

        public int GetMaxSerialOrder(string ID)
        {
            // Created by Anson Lin on 23-Jan-2006

            m_strLastError = "";
            try
            {
                string strMaxOrder = "0";
                int intMaxOrder = 0;
                string strSQL = "SELECT MAX(SORT) FROM FRAME_MODULELIST WHERE PID IN(SELECT PID FROM FRAME_MODULELIST WHERE SYS_ID='" + GlobalDefinition.System_Name()
                    + "' AND MOD_ID = '" + ID.Trim() + "' AND PID<>'*_*FUNCTION*_*')  AND PID<>'*_*FUNCTION*_*' ";
   
                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    using (SqlDataReader dr = conn.ExecuteReader(strSQL))
                    {
                        if (dr.Read())
                            strMaxOrder = dr.GetValue(0).ToString().Trim();
                        dr.Close();
                    }
                }            

                try {  intMaxOrder = int.Parse(strMaxOrder); } catch{}

                return intMaxOrder;
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return 0;
            }
        }


        public string SaveNavigatingItem(CommonEnum.EditActionType Action, string ID, string Name, string NameSC, string NameTC,
                 string Addr, string Desc, string ParentID, int Order, string MUser, string strType, string flag, string subSysId)
        {
            // Created by Anson Lin on 23-Jan-2006
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            m_strLastError = "";
            try
            {
                if (ID.Trim() == GlobalDefinition.System_Name())
                    return CultureRes.GetSysFrameResource("MSG_ERR_MOD_SAMEAS_SYS") + "[" + GlobalDefinition.System_Name() + "].";

                bool bolExist = CheckNavigatingItemExist(ID.Trim());

                string strSQL = "";

                if (Action == CommonEnum.EditActionType.New)
                {
                    if (!bolExist)
                    {
                        return CultureRes.GetSysFrameResource("MSG_ERR_SETTING_EXIST"); //"This link has already existed in database.";
                    }

                    FileLogger.WriteLog_WSC("FRAME", "NAV_TREE", CommonEnum.LogActionType.Add, "SUCCESS!  ModuleID=" + ID.Trim() + "   ModuleName=" + Name.Trim() + "   PID=" + ID.Trim());

                    strSQL = "EXEC SP_FRAME_MENU_ADD_FOR_LANGUAGE '" + GlobalDefinition.System_Name() + "','" + ID.Trim() + "',N'" + Name.Trim() + "',N'" + NameSC.Trim() + "',N'" + NameTC.Trim() + "','" + Addr.Trim() + "',N'"
                     + Desc.Trim() + "'," + Order.ToString() + ",'" + ParentID.Trim() + "','" + MUser.Trim() + "','" + strType.Trim() + "','" + flag + "','" + subSysId + "'";
                }

                else if (Action == CommonEnum.EditActionType.Update)
                {
                    if (bolExist)
                        return CultureRes.GetSysFrameResource("MSG_ERR_SETTING_NOTEXIST");//"This link does not exist in the database.";

                   FileLogger.WriteLog_WSC("FRAME", "NAV_TREE", CommonEnum.LogActionType.Update, "SUCCESS!  ModuleID=" + ID.Trim() + "   ModuleName=" + Name.Trim() + "   PID=" + ID.Trim());

                   strSQL = "EXEC SP_FRAME_MENU_UPDATE_FOR_LANGUAGE '" + GlobalDefinition.System_Name() + "','" + ID.Trim() + "',N'" + Name.Trim() + "',N'" + NameSC.Trim() + "',N'" + NameTC.Trim() + "','" + Addr.Trim() + "',N'"
                    + Desc.Trim() + "'," + Order.ToString() + ",'" + ParentID.Trim() + "','" + MUser.Trim() + "','" + strType.Trim() + "','" + flag + "','" + subSysId + "'";
                   
                }

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }  
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog_WSC("FRAME", "NAV_TREE", CommonEnum.LogActionType.Error, "Error! " + ex.Message + "  -- ModuleID=" + ID.Trim() + "   ModuleName=" + Name.Trim());
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return m_strLastError;
            }
        }


        public DataSet GetParentItems(string IgnoredID)
        {
            // Created by Anson Lin on 23-Jan-2006
            m_strLastError = "";
            DataSet ds = new DataSet();

            try
            {

                string strSQL = "SELECT MOD_ID,MOD_NAME FROM FRAME_MODULELIST "
                    + "WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' AND PID<>'*_*FUNCTION*_*'";

                string strIDs = "";

                if (IgnoredID.Trim() != "")
                {                         
                    GetIgnoredIDs(IgnoredID);

                    for (int i = 0; i < m_arrID.Count; i++)
                        strIDs += "'" + m_arrID[i].ToString() + "',";

                    m_arrID.Clear();

                    strIDs = strIDs.Substring(0, strIDs.Length - 1);  

                    strSQL += " AND MOD_ID NOT IN(" + strIDs.Trim() + ")";
                }

                strSQL += " ORDER BY MOD_NAME";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    ds = conn.ExecuteQuery(strSQL);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return null;
            }
        }

               
        private System.Collections.ArrayList m_arrID = new System.Collections.ArrayList();
        

        private void GetIgnoredIDs(string IgnoredID)
        {
            // Created by Anson Lin on 23-Jan-2006
            try
            {
                string strSQL = "SELECT MOD_ID FROM FRAME_MODULELIST "
                    + "WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' AND PID='" + IgnoredID.Trim() + "' AND PID<>'*_*FUNCTION*_*'";

                WSC_DataConn conn = new WSC_DataConn();
                DataSet ds = conn.ExecuteQuery(strSQL);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        GetIgnoredIDs(ds.Tables[0].Rows[i]["MOD_ID"].ToString().Trim());                       

                m_arrID.Add(IgnoredID.Trim());

                ds = null;
            }
            catch (Exception ex)
            {
                throw new Exception(CultureRes.GetSysFrameResource("MSG_ERR_GET_SUBITEM") + "\\r\\n" + ex.Message) ;
            }
        }

        //Add by AIC21/arty yu on 20120502
        public Hashtable GetSysSubIdByUser(string userName)
        {
            m_strLastError = "";
            Hashtable hashtableSub = new Hashtable();

            try
            {
                string strSQL = "select sys_sub_id from SYSTEM_CONFIG where CFG_NAME IN('SYS_ADMIN','SYS_USER') and sys_id = '" + GlobalDefinition.System_Name() + "' and REPLACE(REPLACE(UPPER(CFG_VALUE),' ',''), '.','')=REPLACE(REPLACE(UPPER('" + userName + "'),' ',''), '.','')";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    DataSet ds = conn.ExecuteQuery(strSQL);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string temp = ds.Tables[0].Rows[i]["sys_sub_id"].ToString().Trim();
                            if (temp != "")
                            {
                                string[] sub_id_array = temp.Split(';');
                                for (int j = 0; j < sub_id_array.Length; j++)
                                {
                                    if(!hashtableSub.ContainsKey(sub_id_array[j]))
                                    {
                                        hashtableSub.Add(sub_id_array[j], sub_id_array[j]);
                                    }
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

            return hashtableSub;
        }
        //End Add by AIC21/arty yu on 20120502
    }
}
