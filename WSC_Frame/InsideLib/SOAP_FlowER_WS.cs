using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Web.Services.Protocols;
using System.Web.Services;
using System.Configuration;
/*****************************************************************************************************
Author        : Anson.Lin
Date	      : April 25,2006
Description   : 
/*****************************************************************************************************/
namespace WSC.InsideLib
{
    [System.Web.Services.WebServiceBindingAttribute(Name = "SOAP_FlowER", Namespace = "http://BQSEIP/FLOWER/FORMS/WS/")]	
    internal class SOAP_FlowER_WS : System.Web.Services.Protocols.SoapHttpClientProtocol
    {
       	private const string m_SOAPACTION_Apply      = "http://BQSEIP/FLOWER/FORMS/WS/ApplyForm"; 
		private const string m_SOAPACTION_Delete     = "http://BQSEIP/FLOWER/FORMS/WS/DeleteForm"; 
		
        /// <summary>
        /// FlowER interface for form data sending and deletion.
        /// Created by Anson Lin on 14-Feb-2006
        /// </summary>
		[System.Diagnostics.DebuggerStepThroughAttribute()]
        public SOAP_FlowER_WS()
		{
            this.Url = GlobalDefinition.FlowER_WebServiceAddr;
		}

        [System.Diagnostics.DebuggerStepThroughAttribute()]
        public SOAP_FlowER_WS(string p_strUrl)
        {
            this.Url = p_strUrl;
        }

		/// <summary>
		/// Send form data to FlowER.
		/// </summary>
		[System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(m_SOAPACTION_Apply, Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public string ApplyForm(string p_strXmlStream) 
		{
			object[] results = this.Invoke("ApplyForm", new object[] {   p_strXmlStream  });
			return ((string)(results[0]));
		}

		/// <summary>
		/// Delete form.
		/// </summary>
		[System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(m_SOAPACTION_Delete, Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public bool DeleteForm(string p_strFormKind,int p_intFormNO)
		{
			object[] results = this.Invoke("DeleteForm", new object[] { p_strFormKind, p_intFormNO });
			return ((bool)(results[0]));
		}
    }
}
