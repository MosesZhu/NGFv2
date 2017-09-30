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
    public sealed class UserMaintain
    {
        public UserMaintain()
        {}

        private string m_strLastError = "";      
        public string GetLastError
        {
            get { return m_strLastError; }
        }


        public DataSet GetValues(string p_strLoginName)
        {
            m_strLastError = "";
            DataSet ds = new DataSet();

            try
            {
                string strSQL = "SELECT * FROM USER_MAINTAIN WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' ";

                if (p_strLoginName.Trim() != "")
                    strSQL += " AND LOGIN_NAME='" + p_strLoginName.Trim() + "'";

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



        public string Delete(string p_strLoginName)
        {
            m_strLastError = "";

            try
            {

                string strSQL = "DELETE FROM USER_MAINTAIN WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' AND LOGIN_NAME='" + p_strLoginName.Trim() + "'";

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

        private bool CheckLoginNameExist(string p_strLoginName)
        {
             m_strLastError = "";
             try
             {
                 string strSQL = "SELECT LOGIN_NAME FROM USER_MAINTAIN WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' AND LOGIN_NAME = '" + p_strLoginName.Trim() + "'";
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


        public string Update(CommonEnum.EditActionType Action, string p_strEmpId, string p_strActive,
                 string p_strEmpNo, string p_strLoginName, string p_strEmpName, string p_strMail, 
                 string p_strExtNo,string p_strEntryDate, string p_strSite, string p_strDept, 
                 string p_strDesc,string p_strPwd, string p_strDipName)
        {         
            m_strLastError = "";

            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                return Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT");

            try
            {
                bool bolExist = CheckLoginNameExist(p_strLoginName.Trim());
                
                string strSQL = "";

                if (Action == CommonEnum.EditActionType.New)
                {
                    if (!bolExist)
                    {
                        return CultureRes.GetSysFrameResource("MSG_ERR_LOGINNAME_EXISTED");//"This link has already existed in database.";
                    }

                    strSQL = "EXEC SP_FRAME_USER_ADD '" + GlobalDefinition.System_Name() + "','"
                        + p_strEmpId.Trim() + "','" + p_strActive.Trim() + "','" + p_strEmpNo.Trim() + "','"
                        + p_strLoginName.Trim() + "','" + p_strEmpName.Trim() + "','" + p_strMail.Trim() + "','"
                        + p_strExtNo.Trim() + "','" + p_strEntryDate.Trim() + "','" + p_strSite.Trim() + "','"
                        + p_strDept.Trim() + "','" + p_strDesc.Trim() + "','" + p_strPwd.Trim() + "','"
                        + p_strDipName.Trim() + "'";

                    //strSQL = "INSERT INTO FRAME_LINKDEF(SYS_ID,lname,ltype,laddr,ldesc) "
                    // + " VALUES('" + GlobalDefinition.System_Name() + "','" + p_strName.Trim() + "','" + p_strType.Trim() + "','"
                    // + p_strAddr.Trim() + "','" + p_strDesc.Trim() + "')";
                }
                else if (Action == CommonEnum.EditActionType.Update)
                {
                    if (bolExist)
                    {
                        return CultureRes.GetSysFrameResource("MSG_ERR_LOGINNAM_NOTEXISTED");// "This link does not exist in the database.";
                    }
                    
                    strSQL = "EXEC  SP_FRAME_USER_UPDATE '" + GlobalDefinition.System_Name() + "','"
                        + p_strEmpId.Trim() + "','" + p_strActive.Trim() + "','" + p_strEmpNo.Trim() + "','"
                        + p_strLoginName.Trim() + "','" + p_strEmpName.Trim() + "','" + p_strMail.Trim() + "','"
                        + p_strExtNo.Trim() + "','" + p_strEntryDate.Trim() + "','" + p_strSite.Trim() + "','"
                        + p_strDept.Trim() + "','" + p_strDesc.Trim() + "','" + p_strPwd.Trim() + "','"
                        + p_strDipName.Trim() + "'";
                    
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
