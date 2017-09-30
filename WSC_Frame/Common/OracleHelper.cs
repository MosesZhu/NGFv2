/****************************************************************************************************
 * Created by Hedda 
 * Class description: Db operater for Oracle
 * Creation date:  2008-12-10
****************************************************************************************************/
using System;
using System.Collections.Generic;
 
using System.Text;
using System.Data;
using System.Data.OracleClient;

namespace WSC.Common
{
    /// <summary>
    /// Oracle数据库服务类
    /// </summary>
    public class OracleHelper : CommonEnum.Error, IDisposable
    {
        protected OracleConnection connection;


        public OracleHelper(string connectionString)
        {
            this.LastError = string.Empty;
            try
            {
                connection = new OracleConnection(connectionString);
                connection.Open();
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
                throw ex;
            }
        }

        /// <summary>
        /// 执行（出错则通过LastError属性返回异常）
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public bool Execute(string SQL)
        {
            this.LastError = string.Empty;
            try
            {
                using (OracleCommand cmd = new OracleCommand(SQL.Trim(), connection))
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

        /// <summary>
        /// 快速读取
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public OracleDataReader Reader(string SQL)
        {
            this.LastError = string.Empty;
            try
            {
                using (OracleCommand cmd = new OracleCommand(SQL.Trim(), connection))
                {
                    OracleDataReader dr = cmd.ExecuteReader();
                    return dr;
                }
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public DataTable Query(string SQL)
        {
            this.LastError = string.Empty;
            try
            {
                using (OracleDataAdapter da = new OracleDataAdapter(SQL.Trim(), connection))
                {
                    DataTable dt = new DataTable();
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
            try
            {
                using (OracleDataAdapter da = new OracleDataAdapter(SQL.Trim(), connection))
                {
                    DataSet ds = new DataSet();
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

        /// <summary>
        /// 获取列第一行的值
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="ColumnIndex"></param>
        /// <returns></returns>
        public string GetValue(string SQL, int ColumnIndex)
        {
            this.LastError = string.Empty;
            try
            {
                using (OracleDataReader dr = this.Reader(SQL))
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

        /// <summary>
        /// 获取列第一行的值
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public string GetValue(string SQL, string ColumnName)
        {
            this.LastError = string.Empty;
            try
            {
                using (OracleDataReader dr = this.Reader(SQL))
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

        ~OracleHelper()
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
                    connection.Close();
                    connection.Dispose();
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
