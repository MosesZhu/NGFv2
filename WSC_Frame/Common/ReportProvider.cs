/*****************************************************************************************************
Author        : Hedda
Date	      : 20060302
Description   : 
/*****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Specialized;
//using CrystalDecisions.Shared;

namespace WSC.Common
{
    /// <summary>
    /// 报表服务类   
    /// by Hedda
    /// </summary>
    public sealed class ReportProvider
    {
        #region 水晶报表，Evolution版取消
        /*

        #region 静态属性
        /// <summary>
        /// 报表显示页的WEB路径
        /// </summary>
        /// <returns></returns>
        private static string ReportCommonWebPath
        {
            get
            {
                try
                {
                    return GlobalDefinition.SystemWebPath + "SysFrame/ReportViewer.aspx";
                }
                catch
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 用来传送报表参数的全局标识
        /// </summary>
        /// <returns></returns>
        public static string ReportParamSessionName
        {
            get
            {
                try
                {
                    //return  GlobalDefinition.System_Name() + "_TempReportParam";
                    return GlobalDefinition.Cookie_LoginUser.Replace(" ", "") + "_" + GlobalDefinition.System_Name() + "_TempReportParam";
                    //return GlobalDefinition.Cookie_LoginUser.Replace(" ","") + "_" + GlobalDefinition.System_Name() + "_TempReportParam_" + Guid.NewGuid().ToString();
                }
                catch
                {
                    return "";
                }
            }
        }
        #endregion

        #region 已注释，注入参数项类

        ///// <summary>
        ///// 供报表使用的注入参数项类
        ///// Create by Hedda 20060302
        ///// </summary>
        //[Serializable()]
        //public sealed class ReprotInjectParamItem
        //{
        //    #region 属性

        //    private string _name;
        //    private string _value;

        //    /// <summary>
        //    /// 参数项的名称
        //    /// </summary>
        //    public string Name
        //    {
        //        get
        //        {
        //            return _name;
        //        }
        //        set
        //        {
        //            _name = value;
        //        }
        //    }

        //    /// <summary>
        //    /// 参数项的值
        //    /// </summary>
        //    public string Value
        //    {
        //        get
        //        {
        //            return _value;
        //        }
        //        set
        //        {
        //            _value = value;
        //        }
        //    }

        //    #endregion

        //    /// <summary>
        //    /// 初始化注入参数项
        //    /// </summary>
        //    public ReprotInjectParamItem()
        //    {
        //    }

        //    /// <summary>
        //    /// 初始化注入参数项
        //    /// </summary>
        //    /// <param name="strName">参数名</param>
        //    /// <param name="strValue">参数值</param>
        //    public ReprotInjectParamItem(string strName, string strValue)
        //    {
        //        this.Name = strName;
        //        this.Value = strValue;
        //    }
        //}

        #endregion

        #region 报表参数类

        /// <summary>
        /// 生成水晶报表必须的参数类（ReportCommon.aspx 通过Session[ReportParamSessionName]接受此类型的参数，接受后自动清除Session）
        /// Create by Hedda 20060301
        /// </summary>
        [Serializable()]
        public class ReportParam
        {
            #region 属性
            private string _reportFileName;
            private DataTable _reportDateSourceTable;
            private NameValueCollection _reportInjectParams;

            /// <summary>
            /// 报表文件的物理全路径
            /// </summary>
            public string ReportFileName
            {
                get
                {
                    return _reportFileName;
                }
                set
                {
                    _reportFileName = value;
                }
            }

            /// <summary>
            /// 报表数据源表
            /// </summary>
            public DataTable ReportDataSourceTable
            {
                get
                {
                    return _reportDateSourceTable;
                }
                set
                {
                    _reportDateSourceTable = value;
                }
            }

            /// <summary>
            /// 需要注入到报表显示的参数集合，存储在此处，只有需要时再转换返回
            /// </summary>
            protected NameValueCollection ReportInjectParams
            {
                get
                {
                    return _reportInjectParams;
                }
                set
                {
                    _reportInjectParams = value;
                }
            }

            #endregion

            /// <summary>
            /// 创建报表参数对象
            /// </summary>
            public ReportParam()
            {

            }

            /// <summary>
            /// 创建对象时初始化
            /// </summary>
            /// <param name="strReportFileName">报表模板文件名</param>
            /// <param name="dtReportDataSourceTable">报表数据源表</param>
            public ReportParam(string strReportFileName, DataTable dtReportDataSourceTable)
            {
                this.ReportFileName = GlobalDefinition.ReportTemplatePhysicalPath + strReportFileName;
                this.ReportDataSourceTable = dtReportDataSourceTable;
                this.ReportInjectParams = new NameValueCollection(); //创建并设置为空
            }

            /// <summary>
            /// 创建对象时初始化
            /// </summary>
            /// <param name="strReportFileName">报表模板文件名</param>
            /// <param name="dtReportDataSourceTable">报表数据源表</param>
            /// <param name="alReprotInjectParams">需要注入到报表中显示的参数，必须为ReprotInjectParamItem实例的ArrayList集合</param>
            public ReportParam(string strReportFileName, DataTable dtReportDataSourceTable, NameValueCollection alReprotInjectParams)
            {
                this.ReportFileName = GlobalDefinition.ReportTemplatePhysicalPath + strReportFileName;
                this.ReportDataSourceTable = dtReportDataSourceTable;
                this.ReportInjectParams = alReprotInjectParams;
            }

            #region ReportInjectParams注入参数的操作

            /// <summary>
            /// 添加一项参数到ReprotInjectParams属性中
            /// </summary>
            /// <param name="strParamName">参数名，报表中参数域必须与之同名</param>
            /// <param name="strParamValue">参数值</param>
            public void AddInjectParam(string strParamName, string strParamValue)
            {
                //添加到参数集属性中
                this.ReportInjectParams.Add(strParamName, strParamValue);
            }

            /// <summary>
            /// 获取报表Viewer能够接受的注入参数集
            /// </summary>
            /// <returns></returns>
            public ParameterFields GetInjectParamFileds()
            {
                try
                {
                    ParameterFields tempInjectParamFileds = new ParameterFields(); //要返回的对象

                    //轮巡参数
                    for (int i = 0; i < this.ReportInjectParams.Count; i++)
                    {
                        string tempKey = this.ReportInjectParams.Keys[i].Trim(); //Key
                        string tempValue = this.ReportInjectParams[tempKey].Trim(); //Value

                        ParameterField tempParamField = new ParameterField(); //参数域（一项）
                        tempParamField.Name = tempKey; //参数域名字
                        ParameterDiscreteValue tempParamValue = new ParameterDiscreteValue(); //参数值对象
                        tempParamValue.Value = tempValue; //参数值
                        tempParamField.CurrentValues.Add(tempParamValue); //把参数值封装到参数域中才能传递（目前只封装一个值，可以封装多个值）

                        //参数域项组装已成功，添加到注入参数集
                        tempInjectParamFileds.Add(tempParamField);
                    }

                    //完成，返回
                    return tempInjectParamFileds;
                }
                catch (Exception ex)
                {
                    WSC.Common.FileLogger.WriteLog("Report", "CrystalReport_Error", "Report", CommonEnum.LogActionType.Error,
                                                   "Error@ReportParam:\r\nEx: [" + ex.Message + "].");
                    return null;
                }
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// 传递参数并弹出窗口显示报表
        /// </summary>
        public static void ShowReport(ReportParam rp)
        {
            try   //Report Logging.
            {
                //得到当前页的引用
                Page currentPage = HttpContext.Current.Handler as Page;
                //添加参数到Session以便报表显示时读取
                if (currentPage.Session[ReportParamSessionName] != null) //若仍存在，先移除
                {
                    currentPage.Session.Remove(ReportParamSessionName);
                }
                currentPage.Session.Add(ReportParamSessionName, rp);
                //if (HttpContext.Current.Cache[ReportParamSessionName] != null) //若仍存在，先移除
                //{
                //    HttpContext.Current.Cache.Remove(ReportParamSessionName);
                //}
                //HttpContext.Current.Cache.Add(ReportParamSessionName, rp, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(60), System.Web.Caching.CacheItemPriority.High, null);

                //弹出这个页面，显示报表
                //width=800,height=550,top=100,left=100,menubar=no,toolbar=no,location=no,directories=no,status=no,resizable=yes,scrollbars=yes
                //currentPage.RegisterStartupScript("report", "<script>window.open('" + GlobalDefinition.ReportCommonWebPath + "','_blank','width=800,height=550,top=100,left=100,menubar=no,toolbar=no,location=no,directories=no,status=no,resizable=yes,scrollbars=yes');</script>");  //Remarked by Anson on 3-Mar-2006
                currentPage.ClientScript.RegisterStartupScript(currentPage.GetType(), "Report", "<script language='javascript'>window.open('" + ReportCommonWebPath + "','_blank','width=800,height=550,top=100,left=100,menubar=no,toolbar=no,location=no,directories=no,status=no,resizable=yes,scrollbars=yes');</script>");


                WSC.Common.FileLogger.WriteLog("Report", "CrystalReport", "Report", CommonEnum.LogActionType.Report,
                       "The user[" + GlobalDefinition.Cookie_LoginUser + "] generates the data to Crystal Report.\r\nPath: [" + rp.ReportFileName + "].");
            }
            catch (Exception ex)
            {
                WSC.Common.FileLogger.WriteLog("Report", "CrystalReport_Error", "Report", CommonEnum.LogActionType.Error,
       "Failed to generate the Report.\r\nEx: [" + ex.Message + "].");

            }
        }

        /// <summary>
        /// 传递参数并弹出窗口显示报表
        /// </summary>
        public void ShowCrystalReport(ReportParam rp)
        {
            //Added by Anson on 20070815
            try   //Report Logging.
            {
                //得到当前页的引用
                Page currentPage = HttpContext.Current.Handler as Page;
                //添加参数到Session以便报表显示时读取
                if (currentPage.Session[ReportParamSessionName] != null) //若仍存在，先移除
                {
                    currentPage.Session.Remove(ReportParamSessionName);
                }
                currentPage.Session.Add(ReportParamSessionName, rp);
                //if (HttpContext.Current.Cache[ReportParamSessionName] != null) //若仍存在，先移除
                //{
                //    HttpContext.Current.Cache.Remove(ReportParamSessionName);
                //}
                //HttpContext.Current.Cache.Add(ReportParamSessionName, rp, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(60), System.Web.Caching.CacheItemPriority.High, null);

                //弹出这个页面，显示报表
                //width=800,height=550,top=100,left=100,menubar=no,toolbar=no,location=no,directories=no,status=no,resizable=yes,scrollbars=yes
                //currentPage.RegisterStartupScript("report", "<script>window.open('" + GlobalDefinition.ReportCommonWebPath + "','_blank','width=800,height=550,top=100,left=100,menubar=no,toolbar=no,location=no,directories=no,status=no,resizable=yes,scrollbars=yes');</script>");  //Remarked by Anson on 3-Mar-2006
                currentPage.ClientScript.RegisterStartupScript(currentPage.GetType(), "Report", "<script language='javascript'>window.open('" + ReportCommonWebPath + "','_blank','width=800,height=550,top=100,left=100,menubar=no,toolbar=no,location=no,directories=no,status=no,resizable=yes,scrollbars=yes');</script>");


                WSC.Common.FileLogger.WriteLog("Report", "CrystalReport", "Report", CommonEnum.LogActionType.Report,
                       "The user[" + GlobalDefinition.Cookie_LoginUser + "] generates the data to Crystal Report.\r\nPath: [" + rp.ReportFileName + "].");
            }
            catch (Exception ex)
            {
                WSC.Common.FileLogger.WriteLog("Report", "CrystalReport_Error", "Report", CommonEnum.LogActionType.Error,
       "Failed to generate the Report.\r\nEx: [" + ex.Message + "].");

            }
        }
        */
        #endregion

        #region Excel报表

        #region VerifyRenderingInServerForm重载举例
        /*
        public override void VerifyRenderingInServerForm(Control control)
        {
            //禁止检测是否在runat中，强制使导出继续
            if (this.EnableEventValidation)
            {
                base.VerifyRenderingInServerForm(control);
            }
        }
        */
        #endregion

        /// <summary>
        /// 导出Excel报告
        /// 注意：在调用本方法时确定GridView中是否存在模板列。
        /// 若存在，请指定 @Page EnableEventValidation ="false" 属性，并在代码中重载 VerifyRenderingInServerForm 方法，令其中不做任何事；
        /// 若不存在，直接调用此方法即可
        /// </summary>
        /// <param name="grd">要导出的Grid的对象</param>
        /// <param name="bindGrid">Grid对应的绑定委托</param>
        /// <param name="columnIndexHide">Grid中需要隐藏的列的索引</param>
        public static void ShowExcelReport(System.Web.UI.WebControls.GridView grd, EventHandler bindGrid, int[] columnIndexHide)
        {
            //获取当前页面的引用
            Page currentPage = HttpContext.Current.Handler as Page;

            string strFileName = "", strFilePath = "";
            //保存旧信息以便恢复
            int nCur = grd.PageIndex;
            int nSize = grd.PageSize;

            try
            {
                #region 准备文件IO

                strFileName = DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".xls";
                strFileName = strFileName.Replace(":", "");
                strFilePath = GlobalDefinition.TempFilePhysicalPath + strFileName;

                //创建文件对象
                System.IO.FileStream fs = new System.IO.FileStream(strFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                //文件流的书写器，指定编码
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8);
                sw.BaseStream.Seek(0, System.IO.SeekOrigin.End);
                //HTML书写器
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);

                #endregion

                #region 为导出修改状态并保存旧状态

                //改变分页
                grd.AllowPaging = false;
                //隐藏要隐藏的列
                foreach (int i in columnIndexHide)
                {
                    grd.Columns[i].Visible = false;
                }

                bindGrid(currentPage, EventArgs.Empty);

                #endregion

                //将HTML流输出到文件IO
                grd.RenderControl(hw);

                //书写完成
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            finally //确保状态恢复及资源释放
            {
                #region 恢复状态

                //恢复隐藏的列
                foreach (int i in columnIndexHide)
                {
                    grd.Columns[i].Visible = true;
                }
                //恢复分页
                grd.AllowPaging = true;
                grd.PageIndex = nCur;
                grd.PageSize = nSize;

                bindGrid(currentPage, EventArgs.Empty);

                #endregion
            }

            //打开Excel报告
            currentPage.Response.Write("<script language='JavaScript'>window.open('" + GlobalDefinition.TempFileWebPath + strFileName.Trim() + "','_blank');</script>");
        }

        /// <summary>
        /// 导出Excel报告
        /// 注意：在调用本方法时确定GridView中是否存在模板列。
        /// 若存在，请指定 @Page EnableEventValidation ="false" 属性，并在代码中重载 VerifyRenderingInServerForm 方法，令其中不做任何事；
        /// 若不存在，直接调用此方法即可
        /// </summary>
        /// <param name="grd">要导出的Grid的对象</param>
        /// <param name="bindGrid">Grid对应的绑定委托</param>
        public static void ShowExcelReport(System.Web.UI.WebControls.GridView grd, EventHandler bindGrid)
        {
            ShowExcelReport(grd, bindGrid, new int[0]);
        }

        /// <summary>
        /// 导出Excel报告
        /// 注意：在调用本方法时确定DataGrid中是否存在模板列。
        /// 若存在，请指定 @Page EnableEventValidation ="false" 属性，并在代码中重载 VerifyRenderingInServerForm 方法，令其中不做任何事；
        /// 若不存在，直接调用此方法即可
        /// </summary>
        /// <param name="grd">要导出的Grid的对象</param>
        /// <param name="bindGrid">Grid对应的绑定委托</param>
        /// <param name="columnIndexHide">Grid中需要隐藏的列的索引</param>
        public static void ShowExcelReport(System.Web.UI.WebControls.DataGrid grd, EventHandler bindGrid, int[] columnIndexHide)
        {
            //获取当前页面的引用
            Page currentPage = HttpContext.Current.Handler as Page;

            string strFileName = "", strFilePath = "";
            //保存旧信息以便恢复
            int nCur = grd.CurrentPageIndex;
            int nSize = grd.PageSize;

            try
            {
                #region 准备文件IO

                strFileName = DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".xls";
                strFileName = strFileName.Replace(":", "");
                strFilePath = GlobalDefinition.TempFilePhysicalPath + strFileName;

                //创建文件对象
                System.IO.FileStream fs = new System.IO.FileStream(strFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                //文件流的书写器，指定编码
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8);
                sw.BaseStream.Seek(0, System.IO.SeekOrigin.End);
                //HTML书写器
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);

                #endregion

                #region 为导出修改状态并保存旧状态

                //改变分页
                grd.AllowPaging = false;
                //隐藏要隐藏的列
                foreach (int i in columnIndexHide)
                {
                    grd.Columns[i].Visible = false;
                }

                bindGrid(currentPage, EventArgs.Empty);

                #endregion

                //将HTML流输出到文件IO
                grd.RenderControl(hw);

                //书写完成
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            finally //确保状态恢复及资源释放
            {
                #region 恢复状态

                //恢复隐藏的列
                foreach (int i in columnIndexHide)
                {
                    grd.Columns[i].Visible = true;
                }
                //恢复分页
                grd.AllowPaging = true;
                grd.CurrentPageIndex = nCur;
                grd.PageSize = nSize;

                bindGrid(currentPage, EventArgs.Empty);

                #endregion
            }

            //打开Excel报告
            currentPage.Response.Write("<script language='JavaScript'>window.open('" + GlobalDefinition.TempFileWebPath + strFileName.Trim() + "','_blank');</script>");
        }

        /// <summary>
        /// 导出Excel报告
        /// 注意：在调用本方法时确定GridView中是否存在模板列。
        /// 若存在，请指定 @Page EnableEventValidation ="false" 属性，并在代码中重载 VerifyRenderingInServerForm 方法，令其中不做任何事；
        /// 若不存在，直接调用此方法即可
        /// </summary>
        /// <param name="grd">要导出的Grid的对象</param>
        /// <param name="bindGrid">Grid对应的绑定委托</param>
        public static void ShowExcelReport(System.Web.UI.WebControls.DataGrid grd, EventHandler bindGrid)
        {
            ShowExcelReport(grd, bindGrid, new int[0]);
        }


        /// <summary>
        /// 导出DataSet为Excel报告
        /// </summary>       
        /// <param name="Data">Data source</param>
        /// <param name="ColumnFields">Fields in db table,multi-column divided by ';'</param>
        /// <param name="ColumnNames">The field names shown in excel,multi-column divided by ';'</param>
        /// <param name="isCallback">Callback or not</param>
        public static void ShowExcelReport(DataSet Data, string ColumnFields, string ColumnNames, bool isCallback)
        {
            //Created by Anson on 2007-08-14

            try
            {
                //获取当前页面的引用
                Page currentPage = HttpContext.Current.Handler as Page;

                string strFileName = "", strFilePath = "";

                #region 生成文件名
                strFileName = DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".xls";
                strFileName = strFileName.Replace(":", "");
                strFilePath = GlobalDefinition.TempFilePhysicalPath + strFileName;
                #endregion

                //创建文件对象
                using (System.IO.FileStream fs = new System.IO.FileStream(strFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    //文件流的书写器，指定编码
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8))
                    {
                        //sw.BaseStream.Seek(0, System.IO.SeekOrigin.End);              

                        //Retrieve data
                        string strResult = GenerateExcel(Data, ColumnFields, ColumnNames);
                        sw.WriteLine(strResult);

                        //书写完成
                        sw.Flush();
                        sw.Close();
                    }
                    fs.Close();
                }

                //打开Excel报告
                string strScript = "window.open('" + GlobalDefinition.TempFileWebPath + strFileName.Trim() + "','_blank')";
                if (!isCallback)
                    currentPage.Response.Write("<script language='JavaScript'>" + strScript + "</script>");
                else
                    Anthem.Manager.AddScriptForClientSideEval(strScript);

                try   //Report Logging.
                {
                    WSC.Common.FileLogger.WriteLog("Report", "ExcelReport", CommonEnum.LogActionType.Report,
                           "The user[" + GlobalDefinition.Cookie_LoginUser + "] generates the data to Excel Report with name ." + strFileName);
                }
                catch { }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Generate html table string
        /// Created by Anson on 2007-08-14
        /// </summary>
        /// <param name="Data">Data source</param>
        /// <param name="ColumnFields">Fields in db table,multi-column divided by ';'</param>
        /// <param name="ColumnNames">The field names shown in excel,multi-column divided by ';'</param>
        /// <returns></returns>
        private static string GenerateExcel(DataSet Data, string ColumnFields, string ColumnNames)
        {
            try
            {
                if (Data == null)
                    throw new Exception("Data cannot be null!");

                string[] arrColField = ColumnFields.Split(';');
                string[] arrColName = ColumnNames.Split(';');
                if (arrColField.Length < 1 || arrColName.Length < 1 || arrColField.Length != arrColName.Length)
                    throw new Exception("Columns cannot be empty value!");

                /*
                  <table border='1' bordercolor='darkgray' style='font-size: 9pt; font-family: arial'>
                    <tr>
                        <td style='font-weight: bold; color: white; background-color: #99ccff'>Cell 1 Name</td>
                        <td style='font-weight: bold; color: white; background-color: #99ccff'>Cell 2 Name</td>
                    </tr>
                    <tr>
                        <td>Cell 1.1</td>
                        <td>Cell 1.2</td>
                    </tr>
                    <tr>
                        <td>Cell 2.1</td>
                        <td>2007/02/22 12:22</td>
                    </tr>
                  </table>
                 */

                StringBuilder sb = new StringBuilder("");

                sb.Append("<table border='1' bordercolor='darkgray' style='font-size: 9pt; font-family: Arial' cellpadding='2' cellspacing='1'>");

                sb.Append("<tr>");    //tr Begin Header row
                foreach (string ColName in arrColName)
                {
                    try
                    {
                        sb.Append("<td style='font-weight: bold; color: white; background-color: #99ccff'>" + ColName.Trim() + "</td>");

                    }
                    catch { throw new Exception("Column name does not exist in the dataset."); }
                }
                sb.Append("</tr>");

                foreach (DataRow dr in Data.Tables[0].Rows)
                {
                    sb.Append("<tr>");    //tr Begin
                    foreach (string ColField in arrColField)
                    {
                        try
                        {
                            sb.Append("<td>");
                            sb.Append(dr[ColField].ToString());
                            sb.Append("</td>");
                        }
                        catch { throw new Exception("Column name does not exist in the dataset."); }
                    }

                    sb.Append("</tr>");   //tr End
                }

                sb.Append("</table>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
