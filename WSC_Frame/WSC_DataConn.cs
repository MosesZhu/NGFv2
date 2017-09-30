/************************************************************************************************
**********Created by Anson Lin on 3-Jan-06'                                             *********
**********Description:                                                                  *********
*************************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using WSC.Common;

namespace WSC
{
    internal sealed class WSC_DataConn : IDisposable 
	{
		private string m_strConn="";
        private string m_strLast_Error = "";

        private SqlConnection cnn;
    
   
        internal WSC_DataConn()
        {
            try
            {
                m_strConn = Security.DecryptInner(GlobalDefinition.WSC_ConnectionString);
               
                cnn = new SqlConnection(m_strConn);
                cnn.Open();
            }
            catch { m_strConn = ""; }
        }

    
        internal WSC_DataConn(bool OpenConnection)
        {
            try
            {
                m_strConn = Security.DecryptInner(GlobalDefinition.WSC_ConnectionString);
                if (OpenConnection)
                {
                    cnn = new SqlConnection(m_strConn);
                    cnn.Open();
                }
            }
            catch { m_strConn = ""; }
        }

      
        internal void Open()
        {
            try
            {
                if (cnn == null || cnn.State == System.Data.ConnectionState.Closed )
                {
                    cnn = new SqlConnection(m_strConn);
                    cnn.Open();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

       
        internal string DBConnString
        {
            get
            { return m_strConn; }
            set
            {   m_strConn = value;      }
        }

        
     
        public string ExecuteSqlNonQuery(string SQL)
        {
            //by Anson，02/04/2005
            m_strLast_Error = "";
            try
            {
                using (SqlCommand cmd = new SqlCommand(SQL.Trim(), cnn))
                {
                    cmd.ExecuteNonQuery();
                    return "SUCCESS";
                }
            }
            catch (Exception ex)
            {
              
                return CultureRes.GetSysFrameResource("MSG_ERR_DB_EXEC") + ex.Message;
            }
        }

        public SqlDataReader ExecuteReader(string SQL)
        {
            //by Anson，02/04/2005
            m_strLast_Error = "";
            
            try
            {
                using (SqlCommand cmd = new SqlCommand(SQL.Trim(), cnn))
                {
                    SqlDataReader dr = cmd.ExecuteReader();

                    return dr;
                }
            }
            catch (Exception ex)
            {
              
                return null;
            }
        }

        public DataSet ExecuteQuery(string SQL)
        {
            //by Anson，02/04/2005
            m_strLast_Error = "";
            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(SQL.Trim(), cnn))
                {
                    da.Fill(ds);

                    return ds;
                }
            }
            catch (Exception ex)
            {
              
                return ds;
            }
        }


        

        public SqlCommand CreateStoreProcedureCommand(string procName)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(procName, cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    return cmd;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        } 

      
        public SqlCommand CreateStoreProcedureCommand(string procName, SqlParameter[] prams)
        {
           
            try
            {
                using (SqlCommand cmd = new SqlCommand(procName, cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (prams != null)
                    {
                        foreach (SqlParameter parameter in prams)
                        {
                            if (parameter != null)
                            {
                                cmd.Parameters.Add(parameter);
                            }
                        }
                    }
                    return cmd;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public string GetValue(string SQL, int ColumnIndex)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(SQL.Trim(), cnn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        string strResult = "";

                        if (dr.Read())
                        {
                            strResult = dr[ColumnIndex].ToString().Trim();
                        }
                        dr.Close();

                        return strResult;
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public string GetValue(string SQL, string ColumnName)
        {
            try
            {
                string strResult = "";
                using (DataSet ds = this.ExecuteQuery(SQL))
                {
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        strResult = ds.Tables[0].Rows[0][ColumnName].ToString().Trim();
                    }
                }

                return strResult;
            }
            catch (Exception ex)
            {
                return "";
            }
        }


     
        public string Last_Error
        {
            //by Anson Lin on 11-Jan-2006
            get
            { return m_strLast_Error; }
        }


        #region IDisposable Members

        private bool disposed = false;

        /// <summary>
        /// GC
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                try
                {
                    cnn.Close();
                    cnn.Dispose();
                }
                catch
                {
                }
            }
            disposed = true;
        }

        #endregion
      
    }


}