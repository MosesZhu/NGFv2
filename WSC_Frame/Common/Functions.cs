/*****************************************************************************************************
Author        : Anson.Lin
Date	      : April 25,2006
Description   : 
/*****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace WSC.Common
{
    /// <summary>
    /// Common functions
    /// </summary>
    public class PublicFunctions
    {
        //***************************************************************************
        #region Global functions

        /// <summary>
        ///  Gets the value from specific property
        /// </summary>
        /// <param name="Ctrl"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        static public object GetPropertyValue(Control Ctrl, string Name)
        {
            Name = Name.Trim();
            try
            {
                Type t = Ctrl.GetType();
                System.Reflection.PropertyInfo p;
                p = t.GetProperty(Name);
                return p.GetValue(Ctrl, null);

            }
            catch { return null; }
        }

        /// <summary>
        /// Sets the value to specific property
        /// </summary>
        /// <param name="Ctrl"></param>
        /// <param name="Name"></param>
        /// <param name="value"></param>
        static public void SetPropertyValue(Control Ctrl, string Name, object value)
        {
            Name = Name.Trim();
            try
            {
                Type t = Ctrl.GetType();
                System.Reflection.PropertyInfo p;
                p = t.GetProperty(Name);
                p.SetValue(Ctrl, value, null);
            }
            catch //(Exception ex)
            {
                //throw new Exception(string.Format("Can not find this property[{0}].", Name.Trim()));
                throw new Exception(string.Format(WSC.Common.CultureRes.GetSysFrameResource("MSG_ERR_PROPERTY_NOTFOUND") + "[{0}].", Name.Trim()));
            }
        }
                

        static public string eMailAddress(string UserName)
        {
            return UserName.Trim().Replace(" ", ".") + "@Qisda.com";
        }

        


        static public string ReplaceXML(string strXML)
        {
            // Anson Lin, 25-Jan-2006
            try
            {
                strXML=strXML.Replace("&","&amp;");
                strXML = strXML.Replace("<", "&lt;");
                strXML = strXML.Replace(">", "&gt;");
                strXML = strXML.Replace("\'", "&apos;");
                strXML = strXML.Replace("\"", "&quot;");

                return strXML;
            }
            catch { return ""; }
        }

        
        static public string ReplaceHtml(string strHtml)
        {
            // Anson Lin, 25-Jan-2006
            try
            {
                strHtml = strHtml.Replace("&amp;", "&");
                strHtml = strHtml.Replace("&lt;", "<");
                strHtml = strHtml.Replace("&gt;", ">");
                strHtml = strHtml.Replace("&apos;", "\'");
                strHtml = strHtml.Replace("&quot;", "\"");

                return strHtml;
            }
            catch { return ""; }
        }


        /// <summary>
        /// Replace the single quotes to double quotation marks        
        /// </summary>
        /// <param name="Value"></param>
        static public string ReplaceSingleQuotes(string Value)
        {
            // Anson Lin, 25-Jan-2006
            try
            {
                Value = Value.Replace("'", "''");
                return Value;
            }
            catch { return ""; }
        }


        /// <summary>
        /// Retrieve the mail address.
        /// Read-only property.
        /// </summary>
        /// <returns></returns>
        static public string DivMessage(CommonEnum.MessageType MsgType)
        {
            try
            {
                string strMsg = "";
                switch (MsgType)
                {
                    case CommonEnum.MessageType.Working:
                        strMsg = CultureRes.GetSysFrameResource("MSG_TIP_DIV_WORKING");
                        break;
                    case CommonEnum.MessageType.Loading:
                        strMsg = CultureRes.GetSysFrameResource("MSG_TIP_DIV_LOADING");
                        break;
                    case CommonEnum.MessageType.Processing:
                        strMsg = CultureRes.GetSysFrameResource("MSG_TIP_DIV_PROCESSING");
                        break;
                    case CommonEnum.MessageType.Saving:
                        strMsg = CultureRes.GetSysFrameResource("MSG_TIP_DIV_SAVING");
                        break;
                    case CommonEnum.MessageType.Cancelling:
                        strMsg = CultureRes.GetSysFrameResource("MSG_TIP_DIV_CANCELLING");
                        break;
                    case CommonEnum.MessageType.Deleting:
                        strMsg = CultureRes.GetSysFrameResource("MSG_TIP_DIV_DELETING");
                        break;
                    default:
                        strMsg = CultureRes.GetSysFrameResource("MSG_TIP_DIV_LOADING");
                        break;
                }
                return strMsg;
            }
            catch { return "Loading ..."; }

        }

        
       /// <summary>
       /// Format date to string
       /// </summary>
       /// <param name="dt"></param>
       /// <param name="NeedHourMinute"></param>
       /// <param name="NeedSecond"></param>
       /// <returns></returns>
        static public string FormatDateToString(DateTime dt,bool NeedHourMinute,bool NeedSecond)
        {
            try
            {
                string year = dt.Year.ToString();
                string month = dt.Month.ToString();
                string day = dt.Day.ToString();
                string hour = dt.Hour.ToString();
                string minute = dt.Minute.ToString();
                string sencond = dt.Second.ToString();

                year  = year.Length  == 2 ? "20" + year  : year;
                month = month.Length == 1 ? "0"  + month : month;
                day   = day.Length   == 1 ? "0"  + day   : day;

                string Value = year + "-" + month + "-" + day;

                if(NeedHourMinute) Value += " " + hour + ":" + minute;
                if(NeedSecond)     Value += ":" + sencond;

                return Value;
            }
            catch { return ""; }
        }
        #endregion
    }
}
