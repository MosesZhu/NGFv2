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

namespace WSC.Framework
{
    public sealed class HeaderLink
    {
        
        public HeaderLink()
        {}

        private string m_strLastError = "";      
        public string GetLastError
        {
            get { return m_strLastError; }
        }


        public DataSet GetValues(string p_strLinkID)
        {
            m_strLastError = "";
            DataSet ds = new DataSet();

            try
            {
                string strSQL = "SELECT * FROM FRAME_LINKDEF WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' ";

                if (p_strLinkID.Trim() != "")
                    strSQL += " AND LNAME='" + p_strLinkID.Trim() + "'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return ds;
            }
        }


      
        public string Delete(string p_strLinkName)
        {
            m_strLastError = "";

        
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                FileLogger.WriteLog_WSC("FRAME", "HEADERLINK", CommonEnum.LogActionType.Delete, "SUCCESS! LinkName=" + p_strLinkName.Trim());

                string strSQL = "DELETE FROM FRAME_LINKDEF WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' AND IS_SYS<>'Y' AND LNAME='" + p_strLinkName.Trim() + "'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return m_strLastError;
            }
        }

        private bool CheckHeaderLinkExist(string p_strLinkName)
        {
             m_strLastError = "";
             try
             {
                 string strSQL = "SELECT LNAME FROM FRAME_LINKDEF WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' AND LNAME = '" + p_strLinkName.Trim() + "'";
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
             catch(Exception ex)
             {
                 m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                 return false;  
             }
        }

            

   
        public string Update(CommonEnum.EditActionType Action, string p_strName, string p_strType, 
                 string p_strAddr,string p_strDesc)
        {         
            m_strLastError = "";

            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                bool bolExist = CheckHeaderLinkExist(p_strName.Trim());
                
                string strSQL = "";

                if (Action == CommonEnum.EditActionType.New)
                {
                    if (!bolExist)
                    {
                        return CultureRes.GetSysFrameResource("MSG_ERR_LINK_EXISTED");//"This link has already existed in database.";
                    }

                    FileLogger.WriteLog_WSC("FRAME", "HEADERLINK", CommonEnum.LogActionType.Add, "SUCCESS! LinkName=" + p_strName.Trim() + "   Address=" + p_strAddr.Trim());

                    strSQL = "EXEC SP_FRAME_HEADLINK_ADD '" + GlobalDefinition.System_Name() + "','"
                        + p_strName.Trim() + "','" + p_strType.Trim() + "','" + p_strAddr.Trim() + "','"
                        + p_strDesc.Trim() + "'";

                    //strSQL = "INSERT INTO FRAME_LINKDEF(SYS_ID,lname,ltype,laddr,ldesc) "
                    // + " VALUES('" + GlobalDefinition.System_Name() + "','" + p_strName.Trim() + "','" + p_strType.Trim() + "','"
                    // + p_strAddr.Trim() + "','" + p_strDesc.Trim() + "')";
                }
                else if (Action == CommonEnum.EditActionType.Update)
                {
                    if (bolExist)
                    {
                        return CultureRes.GetSysFrameResource("MSG_ERR_LINK_NOTEXISTED");// "This link does not exist in the database.";
                    }

                    FileLogger.WriteLog_WSC("FRAME", "HEADERLINK", CommonEnum.LogActionType.Update, "SUCCESS! LinkName=" + p_strName.Trim() + "   Address=" + p_strAddr.Trim());

                    strSQL = "EXEC  SP_FRAME_HEADLINK_UPDATE '" + GlobalDefinition.System_Name() + "','" 
                        + p_strName.Trim() + "','" + p_strType.Trim() + "','" + p_strAddr.Trim() + "','" 
                        + p_strDesc.Trim() + "'";
                    
                    //strSQL = "UPDATE FR) + "',ldesc='" + p_strDesc.Trim() + "' "
                    //    + " WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' AND LNAME='" + p_strName.Trim() + "'";
                }

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    return conn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
                return m_strLastError;
            }
        }

    }
}
