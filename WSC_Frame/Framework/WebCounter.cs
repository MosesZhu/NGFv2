// Created by Anson Lin on 18-Jan-2006
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls; 

namespace WSC.Framework
{
    /// <summary>
    /// Web counter    
    /// </summary>
    public sealed class WebCounter
    {
        /// <summary>
        /// Web counter
        /// </summary>
        public WebCounter()
        {}


        /// <summary>
        /// Increases and gets the count.
        /// </summary>
        /// <returns></returns>
        public static int IncreaseCount()
        {
            try
            {
                string strCount = "";

                string strSQL = "EXEC SP_FRAME_COUNTER_INCREASE '" + GlobalDefinition.System_Name() + "' ";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    using (SqlDataReader dr = conn.ExecuteReader(strSQL))
                    {
                        if (dr.Read())
                            strCount = dr.GetValue(0).ToString().Trim();
                        dr.Close();
                    }
                    int intCount = 0;
                    try { intCount = int.Parse(strCount); }
                    catch { }

                    return intCount;
                }
            }
            catch
            {    return 0;     }
        }

        /// <summary>
        /// Retrieve the count.
        /// </summary>
        /// <returns></returns>
        public static int GetCount()
        {
            try
            {
                string strCount = "";

                string strSQL = "EXEC SP_FRAME_COUNTER_GET '" + GlobalDefinition.System_Name() + "' ";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    using (SqlDataReader dr = conn.ExecuteReader(strSQL))
                    {
                        if (dr.Read())
                            strCount = dr.GetValue(0).ToString().Trim();
                        dr.Close();
                    }
                    int intCount = 0;
                    try { intCount = int.Parse(strCount); }
                    catch { }

                    return intCount;
                }
            }
            catch
            { return 0; }
        }
    }
}
