// Created by Anson Lin on 3-Feb-2006
// Modified by Anson Lin on 3-Jun-2005 
// Last updated by Anson on 13-Jan-2006
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace WSC.Common
{
    /// <summary>
    /// 文件上传服务类
    /// </summary>
    public sealed class UploadFile
    {
        private string m_strLastError = "";


        public UploadFile()
        {
        }


        public string GetLastError
        {
            get { return m_strLastError; }
        }
        public string GetPicFile(string UploadType, string ID)
        {
            m_strLastError = "";
            try
            {
                string strSQL = "EXEC SP_UPLOAD_GET_FILE_PIC '" + GlobalDefinition.System_Name() + "','" + UploadType.Trim() + "','" + ID.Trim() + "'";
                string strR = "";

                using (WSC_DataConn cnn = new WSC_DataConn())
                {
                    using (DataSet ds = cnn.ExecuteQuery(strSQL))
                    {
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            strR = GlobalDefinition.UploadFileWebPath(UploadType) + ds.Tables[0].Rows[0]["FILE_NAME"].ToString();
                        }
                    }
                }
                return strR;
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + "\\r\\n" + ex.Message;
                return "";
            }
        }
        public DataSet GetPicFiles(string UploadType, string ID)
        {
            m_strLastError = "";
            DataSet ds = new DataSet();
            try
            {
                string strSQL = "EXEC SP_UPLOAD_GET_FILE_PIC '" + GlobalDefinition.System_Name() + "','" + UploadType.Trim() + "','" + ID.Trim() + "'";

                using (WSC_DataConn cnn = new WSC_DataConn())
                {
                    ds = cnn.ExecuteQuery(strSQL);
                }

                return ds;
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + "\\r\\n" + ex.Message;
                return ds;
            }
        }
        public DataSet GetFiles(string UploadType, string ID)
        {
            DataSet ds = new DataSet();
            m_strLastError = "";
            try
            {
                string strSQL = "EXEC SP_UPLOAD_GET_FILE '" + GlobalDefinition.System_Name() + "','" + UploadType.Trim() + "','" + ID.Trim() + "',0";

                using (WSC_DataConn cnn = new WSC_DataConn())
                {
                    ds = cnn.ExecuteQuery(strSQL);
                }

                return ds;
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + "\\r\\n" + ex.Message;
                return ds;
            }
        }
        public DataSet GetFiles(string UploadType, string ID, int Seq)
        {
            m_strLastError = "";
            DataSet ds = new DataSet();
            try
            {
                string strSQL = "EXEC SP_UPLOAD_GET_FILE '" + GlobalDefinition.System_Name() + "','" + UploadType.Trim() + "','" + ID.Trim() + "'," + Seq.ToString();

                using (WSC_DataConn cnn = new WSC_DataConn())
                {
                    ds = cnn.ExecuteQuery(strSQL);
                }

                return ds;
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + "\\r\\n" + ex.Message;
                return ds;
            }
        }

        /// <summary>
        /// 以HTML链结的形式获取已上传的图片
        /// </summary>
        /// <param name="UploadType"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetFilesHtml(string UploadType, string ID)
        {
            m_strLastError = "";
            try
            {
                using (DataSet ds = GetFiles(UploadType, ID))
                {
                    string strRtn = "";

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        string strWebPath = GlobalDefinition.UploadFileWebPath(UploadType);
                        if (strWebPath == "" || strWebPath == null)
                        {
                            m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_UPLOAD_LACATE"); // "Can not locate the file path.";
                            throw new Exception(m_strLastError);
                        }

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            strRtn += "<A target=_blank href=" + strWebPath + ds.Tables[0].Rows[i]["FILE_NAME"].ToString().Trim() + ">"
                                + ds.Tables[0].Rows[i]["FILE_TAG"].ToString().Trim() + "</A>&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                    }

                    return strRtn.Substring(0, strRtn.LastIndexOf("</A>") + 4);
                }
            }
            catch (Exception ex) { m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_UPLOAD_GENHTML") + "\\r\\n" + ex.Message; return ""; }
        }
        public string GetFilesHtml(string UploadType, string ID, int Seq)
        {
            m_strLastError = "";
            try
            {
                using (DataSet ds = GetFiles(UploadType, ID, Seq))
                {
                    string strRtn = "";

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        string strWebPath = GlobalDefinition.UploadFileWebPath(UploadType);
                        if (strWebPath == "" || strWebPath == null)
                        {
                            m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_UPLOAD_LACATE"); // "Can not locate the file path";
                            throw new Exception(m_strLastError);
                        }

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            //将附件都列在此处
                            strRtn += "<A target=_blank href=" + strWebPath + ds.Tables[0].Rows[i]["FILE_NAME"].ToString().Trim() + ">"
                                + ds.Tables[0].Rows[i]["FILE_TAG"].ToString().Trim() + "</A>&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                    }

                    return strRtn.Substring(0, strRtn.LastIndexOf("</A>") + 4);
                }
            }
            catch (Exception ex) { m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_UPLOAD_GENHTML") + "\\r\\n" + ex.Message; return ""; }
        }
        public string Upload(string ID, string UploadType, string FileName, string UserName, out string NewFileName)
        {
            // by Anson Lin on 19-Feb-2006
            NewFileName = "";
            m_strLastError = "";
            try
            {
                using (WSC_DataConn cnn = new WSC_DataConn())
                {
                    using (SqlCommand cmd = cnn.CreateStoreProcedureCommand("SP_UPLOAD_FILE"))
                    {
                        cmd.Parameters.Add("@SYS_ID", SqlDbType.NVarChar, 50);
                        cmd.Parameters["@SYS_ID"].Value = GlobalDefinition.System_Name();
                        cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 100);
                        cmd.Parameters["@ID"].Value = ID;
                        cmd.Parameters.Add("@TYPE", SqlDbType.NVarChar, 50);
                        cmd.Parameters["@TYPE"].Value = UploadType;
                        cmd.Parameters.Add("@FILE_NAME", SqlDbType.NVarChar, 100);
                        cmd.Parameters["@FILE_NAME"].Value = FileName;
                        cmd.Parameters.Add("@USER_NAME", SqlDbType.NVarChar, 50);
                        cmd.Parameters["@USER_NAME"].Value = UserName;

                        cmd.Parameters.Add("@NEW_NAME", SqlDbType.NVarChar, 500);
                        cmd.Parameters["@NEW_NAME"].Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        NewFileName = cmd.Parameters["@NEW_NAME"].Value.ToString().Trim();

                    }
                }

                try
                {
                    FileLogger.WriteLog("Upload", "UploadFile", "Upload", CommonEnum.LogActionType.Info,
                          GlobalDefinition.Cookie_LoginUser + "  |  ID: " + ID.Trim() + "  |  Type: " + UploadType + "  |  Name: " + FileName.Trim() + "  |  New file name: " + NewFileName.Trim());
                }
                catch { }

                if (NewFileName != "" && NewFileName != null)
                    return "SUCCESS";
                else
                    return CultureRes.GetSysFrameResource("MSG_ERR_UPLOAD"); // "Error trying to upload a file!";
            }
            catch (Exception ex)
            {
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + "\\r\\n" + ex.Message;
                FileLogger.WriteLog("Upload", "Error", "UploadFile", CommonEnum.LogActionType.Error, ex.Message);
                return CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + "\\r\\n" + ex.Message;
            }
        }

        public string DeleteFile(string UploadType, string ID, int Seq)
        {
            m_strLastError = "";
            try
            {
                if (Seq <= 0)
                    return CultureRes.GetSysFrameResource("MSG_ERR_UPLOAD_DELETE");

                using (WSC_DataConn cnn = new WSC_DataConn())
                {
                    string strPath = "";
                    strPath = GlobalDefinition.UploadFilePhysicalPath(UploadType);

                    string strSQL = "EXEC SP_UPLOAD_GET_FILE '" + GlobalDefinition.System_Name() + "','" + UploadType + "','" + ID + "'," + Seq;
                    using (DataSet ds = cnn.ExecuteQuery(strSQL))
                    {
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                try
                                {
                                    System.IO.File.Delete(strPath + ds.Tables[0].Rows[i]["FILE_NAME"].ToString());
                                }
                                catch
                                {
                                }
                            }
                    }

                    strSQL = "EXEC SP_UPLOAD_DEL_FILE '" + GlobalDefinition.System_Name() + "','" + UploadType.Trim() + "','" + ID.Trim() + "'," + Seq.ToString();

                    try
                    {
                        FileLogger.WriteLog("Upload", "Delete", "DeleteFile", CommonEnum.LogActionType.Info,
                              GlobalDefinition.Cookie_LoginUser + "  |  ID: " + ID.Trim() + "  |  Type: " + UploadType + "  |  Seq: " + Seq.ToString().Trim() + "\r\n" + "SQL = " + strSQL);
                    }
                    catch { }

                    return cnn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog("Upload", "Error", "Delete", CommonEnum.LogActionType.Error, ex.Message);
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + "\\r\\n" + ex.Message;
                return CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + "\\r\\n" + ex.Message;
            }
        }


        public string DeleteFile(string UploadType, string ID)
        {
            m_strLastError = "";
            try
            {
                using (WSC_DataConn cnn = new WSC_DataConn())
                {
                    string strPath = "";
                    strPath = GlobalDefinition.UploadFilePhysicalPath(UploadType);

                    string strSQL = "EXEC SP_UPLOAD_GET_FILE '" + GlobalDefinition.System_Name() + "','" + UploadType + "','" + ID + "',0";
                    using (DataSet ds = cnn.ExecuteQuery(strSQL))
                    {
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                System.IO.File.Delete(strPath + ds.Tables[0].Rows[i]["FILE_NAME"].ToString());
                            }
                    }

                    strSQL = "EXEC SP_UPLOAD_DEL_FILE '" + GlobalDefinition.System_Name() + "','" + UploadType.Trim() + "','" + ID.Trim() + ",0";

                    try
                    {
                        FileLogger.WriteLog("Upload", "Delete", "DeleteFile", CommonEnum.LogActionType.Info,
                              GlobalDefinition.Cookie_LoginUser + "  |  ID: " + ID.Trim() + "  |  Type: " + UploadType + "\r\n" + "SQL = " + strSQL);
                    }
                    catch { }

                    return cnn.ExecuteSqlNonQuery(strSQL);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog("Upload", "Error", "Delete", CommonEnum.LogActionType.Error, ex.Message);
                m_strLastError = CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + "\\r\\n" + ex.Message;
                return CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + "\\r\\n" + ex.Message;
            }
        }

        static public string InitializeUploadedFiles(string ControlID, string UploadType, string ID)
        {
            try
            {
                using (Page executingPage = HttpContext.Current.Handler as Page)
                {
                    if (executingPage != null)
                    {
                        UploadFile clsFU = new UploadFile();
                        string ReturnHtml = clsFU.GetFilesHtml(UploadType, ID);

                        Control ctrl = executingPage.FindControl(ControlID);

                        HtmlGenericControl genC = new HtmlGenericControl("DIV");
                        genC.ID = ControlID.Trim() + "_divUploadedFiles";

                        genC.InnerHtml = ReturnHtml;
                        ctrl.Controls.Add(genC);

                        return "SUCCESS";
                    }
                    return "No page object.";
                }
            }
            catch (Exception ex) { return CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + "\\r\\n" + ex.Message; }
        }

        static public string InitializeUploadedFiles(Control ctrl, string UploadType, string ID)
        {
            try
            {
                using (Page executingPage = HttpContext.Current.Handler as Page)
                {
                    if (executingPage != null)
                    {
                        UploadFile clsFU = new UploadFile();
                        string ReturnHtml = clsFU.GetFilesHtml(UploadType, ID);

                        //Control ctrl = executingPage.FindControl(ControlID);

                        HtmlGenericControl genC = new HtmlGenericControl("DIV");
                        genC.ID = ctrl.ClientID.Trim() + "_divUploadedFiles";

                        genC.InnerHtml = ReturnHtml;
                        ctrl.Controls.Add(genC);

                        return "SUCCESS";
                    }
                    return "No page object.";
                }
            }
            catch (Exception ex) { return CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + "\\r\\n" + ex.Message; }
        }

    }
}