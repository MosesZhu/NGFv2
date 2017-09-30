/*****************************************************************************************************
Author        : Anson.Lin
Date	      : Feb 9,2006
Description   : 
/*****************************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using WSC.Common;

namespace WSC.SecurityControl
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class WSC_PermissionQuery
    {
        /// <summary>
        /// 
        /// </summary>
        internal WSC_PermissionQuery() { }

        private string m_strLastError = "";     
        internal string GetLastError
        {
            get { return m_strLastError; }
        }

     
        internal DataSet TreeGetFirstTier(string UserName)
        {           
            DataSet ds = new DataSet();

            try
            {
                string strSQL = "EXEC  SP_WSC_SECURITY_QRY_FirstTier '" + GlobalDefinition.System_Name() 
                    + "','" + UserName + "' ";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    ds = conn.ExecuteQuery(strSQL);
                    return ds;
                }
            }
            catch (Exception ex)
            {
               
                throw new Exception("Error trying to get first tier data. \\nDescription: " + ex.Message);
            }
        }

       
        internal DataSet TreeGetSecondTier(string UserName)
        {
            m_strLastError = "";
            try
            {
                DataSet ds;
                string strSQL = "EXEC  SP_WSC_SECURITY_QRY_SecondTier '" + GlobalDefinition.System_Name()
                    + "','" + UserName + "' ";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    ds = conn.ExecuteQuery(strSQL);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                m_strLastError = ex.Message;
                throw new Exception("Error trying to get second tier data. \\nDescription: " + ex.Message);
            }
        }

        internal DataSet TreeGetThirdTier(string Flag, string ID)
        {
            m_strLastError = "";
            try
            {
                DataSet ds;
                string strSQL = "EXEC SP_WSC_SECURITY_QRY_ThirdTier '" + GlobalDefinition.System_Name()
                    + "','" + Flag + "','" + ID + "'";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    ds = conn.ExecuteQuery(strSQL);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                m_strLastError = ex.Message;
                throw new Exception("Error trying to get third tier data. \\nDescription: " + ex.Message);
            }
        }


        internal string DeleteUserAccount(string UserName)
        {
            m_strLastError = "";
            try
            {
                string strSQL = "EXEC SP_WSC_DELETE_USER '" + GlobalDefinition.System_Name()
                    + "','" + UserName + "'";

                string strR = "";
                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    strR = conn.ExecuteSqlNonQuery(strSQL);
                }
                if(strR!="SUCCESS")
                    throw new Exception(CultureRes.GetSysFrameResource("MSG_ERR_USER_DELETE") + strR);//"Error trying to delete User account. \\nDescription: "
                else
                    return "SUCCESS";
            }
            catch (Exception ex)
            {
                m_strLastError = ex.Message;
                throw new Exception(CultureRes.GetSysFrameResource("MSG_ERR_USER_DELETE") + ex.Message);
            }
        }
    }
}
