/*****************************************************************************************************
Author        : Anson.Lin
Date	      : 14-Feb-2006
Description   : 
/*****************************************************************************************************/

using System;
using System.Data;
using System.Text;
using WSC.InsideLib;
using System.Xml;

namespace WSC.Common
{
    /// <summary>
    ///
    /// </summary>
    public sealed class FlowER: IDisposable 
    {
        
        public FlowER()
        { }

       
        public FlowER(string WebServiceHttpUrl)
        {
            m_strUrl = WebServiceHttpUrl.Trim(); 
        }

        private string m_strUrl = "";
        /// <summary>
        /// 
        /// </summary>
        public string WS_HttpUrl
        {
            get { return m_strUrl; }
            set { m_strUrl = value.Trim(); }
        }

       
        public string Send(string strXML, out int FormNo)
        {           
            FormNo = -1;
            string strFORM_KIND = "";
            string strErrDesc = "";

            try
            {      
                XmlDocument objXmlDoc = new XmlDocument();               
                objXmlDoc.LoadXml(strXML);               
                XmlElement objRootNode = objXmlDoc.DocumentElement;                
                strFORM_KIND = objRootNode.SelectSingleNode("FormKind").InnerText.Trim().ToUpper();              
                SOAP_FlowER_WS clsF;
                if (m_strUrl == "" || m_strUrl == null)
                    clsF = new SOAP_FlowER_WS();
                else
                    clsF = new SOAP_FlowER_WS(m_strUrl);
                    string strR = clsF.ApplyForm(strXML);  
                    objXmlDoc = new XmlDocument();
                    XmlElement objRootNodeR;
                    objXmlDoc.LoadXml(strR);
                    objRootNodeR = objXmlDoc.DocumentElement;
                    strFORM_KIND = objRootNodeR.SelectSingleNode("//FormKind").InnerText.Trim().ToUpper();
                    string strFORM_NO = objRootNodeR.SelectSingleNode("//FormNO").InnerText.Trim().ToUpper();
                    strErrDesc = objRootNodeR.SelectSingleNode("//ErrDesc").InnerText.Trim().ToUpper();                    
                    FormNo = int.Parse(strFORM_NO);    
                    if (strErrDesc == "SUCCESS" && FormNo > 0)
                        return "SUCCESS";
                    else
                        return CultureRes.GetSysFrameResource("MSG_ERR_FLOWER_SENDFORM") +　"\\r\\n" + strErrDesc;
            }
            catch (Exception ex)
            {
                WriteLog("SendFormError", CommonEnum.LogActionType.Error, strFORM_KIND.Trim(), FormNo, "Error trying to send the form to FlowER," + ex.Message.Trim(), strXML.Trim());
                return CultureRes.GetSysFrameResource("MSG_ERR_FLOWER_SENDFORM") +　"\\r\\n" + ex.Message.Trim();
            }
        }      
        public string DeleteForm(string FormKind, int FormNo)
        {
            try
            {
                SOAP_FlowER_WS clsF;
                if (m_strUrl == "")
                    clsF = new SOAP_FlowER_WS();
                else
                    clsF = new SOAP_FlowER_WS(m_strUrl);

                bool bolR = clsF.DeleteForm(FormKind, FormNo);
                if (bolR)
                {
                    return "SUCCESS";
                }
                else
                {
                    WriteLog("DeleteFormError", CommonEnum.LogActionType.Error, FormKind.Trim(), FormNo, CultureRes.GetSysFrameResource("MSG_ERR_FLOWER_DELFORM") + "\t|\tWebService", "");
                    return CultureRes.GetSysFrameResource("MSG_ERR_FLOWER_DELFORM");
                }
            }
            catch (Exception ex)
            {
                WriteLog("DeleteFormException", CommonEnum.LogActionType.Error, FormKind.Trim(), FormNo, ex.Message, "");
                return  CultureRes.GetSysFrameResource("MSG_ERR_FLOWER_DELFORM") + "\\r\\n" + ex.Message;
            }
        }


        public string HeadXML(string FormKind, string FormID,string Applicant)
        {
            StringBuilder strXML = new StringBuilder("");
            strXML.Append("<?xml version='1.0' encoding='gb2312'?>");
            strXML.Append("<root><FormKind>" + FormKind + "</FormKind><oldFlowID></oldFlowID>");
            strXML.Append("<FormNo>" + FormID.ToString() + "</FormNo>");
            strXML.Append("<Applicant>" + Applicant.Trim() + "</Applicant>");
            strXML.Append("<Filler>" + Applicant.Trim() + "</Filler>");
            return strXML.ToString();     
        }

        public string FootXML()
        {                        
            return "</root>";
        }
              
        private void WriteLog(string Module, CommonEnum.LogActionType Action, string FormKind, int FormNo, string ErrMessage, string strXML)
        {
            try
            {
                string strLogMessage = "FORM_KIND=" + FormKind + "\t\t   FORM_NO=" + FormNo.ToString() 
                    + " \r\nMsg:  " + ErrMessage + "\r\n";
                strLogMessage += "XML:  " + strXML.Trim() + "\r\n";
                FileLogger.WriteLog("FromFlowER","FlowERFormCradle", Module, Action, strLogMessage);
            }
            catch { }
        }

        #region IDisposable Members

       
        public void Dispose()
        {          
            GC.SuppressFinalize(this);
        }

       

        #endregion
    }
}
