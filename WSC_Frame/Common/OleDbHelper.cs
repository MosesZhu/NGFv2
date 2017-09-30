/****************************************************************************************************
 * Created by Hedda 
 * Class description: Db operater for Access,etc.
 * Creation date:  2008-12-10
****************************************************************************************************/

using System;
using System.Collections.Generic;
 
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace WSC.Common
{
    /// <summary>
    /// OleDb数据库服务类
    /// </summary>
    public class OleDbHelper : CommonEnum.Error, IDisposable
    {
        protected OleDbConnection connection;


        public OleDbHelper(string connectionString)
        {
            this.LastError = string.Empty;
            try
            {
                connection = new OleDbConnection(connectionString);
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
                using (OleDbCommand cmd = new OleDbCommand(SQL.Trim(), connection))
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
        public OleDbDataReader Reader(string SQL)
        {
            this.LastError = string.Empty;
            try
            {
                using (OleDbCommand cmd = new OleDbCommand(SQL.Trim(), connection))
                {
                    OleDbDataReader dr = cmd.ExecuteReader();
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
                using (OleDbDataAdapter da = new OleDbDataAdapter(SQL.Trim(), connection))
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
                using (OleDbDataAdapter da = new OleDbDataAdapter(SQL.Trim(), connection))
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
                using (OleDbDataReader dr = this.Reader(SQL))
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
                using (OleDbDataReader dr = this.Reader(SQL))
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

        ~OleDbHelper()
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
