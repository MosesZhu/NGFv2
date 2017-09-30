#region Last Revised: 2008-07-18
/****************************************************************************************************
 * Created by Anson 
 * Class description:
 * Creation date:  2008-07-08
 ====================================================================================================
 * Update history:
 *        2008-07-18:  Update the Constructor function by Anson
 * 
====================================================================================================
 * Update history:
 *        2008-09   :  Fix a GC bug by Hedda
 * 
 ****************************************************************************************************/
#endregion
using System;
using System.Data;
using System.Data.SqlClient;

namespace WSC.Common
{
    public class SQLHelper : WSC.Common.CommonEnum.Error, IDisposable
    {      
        protected SqlConnection cnn;

        public SQLHelper(string ConnectionString)
        {
            this.LastError = string.Empty;
            try
            {
                cnn = new SqlConnection(ConnectionString);
                cnn.Open();               
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
                throw ex;
            }
        }              


        public bool Execute(string SQL)
        {            
            try
            {
                this.LastError = string.Empty;
                using (SqlCommand cmd = new SqlCommand(SQL.Trim(), cnn))
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
                return false;
            }
        }      
        public SqlDataReader Reader(string SQL)
        {            
            try
            {
                this.LastError = string.Empty;
                using (SqlCommand cmd = new SqlCommand(SQL.Trim(), cnn))
                {
                    SqlDataReader dr = cmd.ExecuteReader();                    
                    return dr;
                }
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
                return null;
            }
        }
        public DataTable Query(string SQL)
        {
            this.LastError = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(SQL.Trim(), cnn))
                {
                    da.Fill(dt);                   
                    return dt;
                }
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
                return null;   
            }
        }
        public DataSet Query(string SQL, object inputNull)
        {
            this.LastError = string.Empty;
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
                this.LastError = ex.Message;
                return null;
            }
        } 
        public string GetValue(string SQL, int ColumnIndex)
        {            
            try
            {
                this.LastError = string.Empty;

                using (SqlDataReader dr = this.Reader(SQL))
                {
                    if (dr.Read())
                    {
                        return dr[ColumnIndex].ToString().Trim();
                    }
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
                return string.Empty;
            }
        }    
        public string GetValue(string SQL, string ColumnName)
        {
            try
            {
                this.LastError = string.Empty;

                using (SqlDataReader dr = this.Reader(SQL))
                {
                    if (dr.Read())
                    {
                        return dr[ColumnName].ToString().Trim();
                    }
                    return string.Empty;
                }               
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
                return string.Empty;
            }


        }


        //JFK Add 20090623
        /// <summary>
        /// 
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="parameters"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {

            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            using (SqlCommand command = new SqlCommand(storedProcName, cnn))
            {
                command.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter parameter in parameters)
                {
                    if (parameter != null)
                    {
                        if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                        {
                            parameter.Value = DBNull.Value;
                        }
                        command.Parameters.Add(parameter);
                    }
                }

                adapter.SelectCommand = command;
                adapter.Fill(dataSet, tableName);
                command.Dispose();
                adapter.Dispose();
            }
            return dataSet;
        }

        ~SQLHelper()
        {
            this.Dispose(false);
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

