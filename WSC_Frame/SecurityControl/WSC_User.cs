/************************************************************************************************
**********Created by Anson Lin on 25-Mar-2006                                           *********
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
   
    public sealed class WSC_User 
    {
       private string m_strLastError = "";

       public WSC_User()
        { }

       public string GetLastError
       {
           get { return m_strLastError; }
       }

       public DataSet GetUserList(string DeptOrLoginName)
       {
           // Created by Anson Lin on 25-Mar-2006
           m_strLastError = "";
           DataSet ds = new DataSet();

           try
           {
               string strSQL = "";

               strSQL = "EXEC SP_WSC_GET_USER_LIST_SinglePara '" + DeptOrLoginName.Trim() + "','" + GlobalDefinition.System_Name() + "'";

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

      
       public DataSet GetUserList(string Dept,string LoginName)
       {
           m_strLastError = "";
           DataSet ds = new DataSet();

           try
           {               
               string strSQL = "";

               strSQL = "EXEC SP_WSC_GET_USER_LIST '" + Dept.Trim() + "','" + LoginName + "','" + GlobalDefinition.System_Name() + "'";

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

     
       public DataSet GetDepartment(string Dept)
       {
           m_strLastError = "";
           DataSet ds = new DataSet();

           try
           {               
               string strSQL = "EXEC SP_WSC_GET_DEPT '" + Dept.Trim() + "','"+GlobalDefinition.System_Name()+"'";

               using (WSC_DataConn conn = new WSC_DataConn())
               {
                   ds = conn.ExecuteQuery(strSQL);
                   return ds;
               }
           }
           catch (Exception ex)
           {
               FileLogger.WriteLog_WSC("WSC", "USER", CommonEnum.LogActionType.Error, "Error retrieving department list!  " + ex.Message);
               m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + ex.Message;
               return ds;
           }
       }

    
       public DataSet GetDepartment()
       {
           return this.GetDepartment("");
       }
    }    
}
