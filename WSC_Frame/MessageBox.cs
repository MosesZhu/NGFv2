/************************************************************************************************
**********Created by Anson Lin on    29 December, 2005                                  *********
**********Description:                                                                  *********
*************************************************************************************************/
using System;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Collections;

namespace WSC
{
    /// <summary>
    /// WebPage消息框
    /// </summary>
    public class MessageBox
    {
        private static Hashtable m_executingPages = new Hashtable();     
        public static void Show(string sMessage)
        {
         
            if (!m_executingPages.Contains(HttpContext.Current.Handler))
            {
              
                Page executingPage = HttpContext.Current.Handler as Page;
                if (executingPage != null)
                {
                    Queue messageQueue = new Queue();
                    messageQueue.Enqueue(sMessage);
                    m_executingPages.Add(HttpContext.Current.Handler, messageQueue);
                    executingPage.Unload += new EventHandler(ExecutingPage_Unload);
                }
            }
            else
            {
                Queue queue = (Queue)m_executingPages[HttpContext.Current.Handler];
                queue.Enqueue(sMessage);
            }
        }


        private static void ExecutingPage_Unload(object sender, EventArgs e)
        {
            Queue queue = (Queue)m_executingPages[HttpContext.Current.Handler];
            if (queue != null)
            {
                StringBuilder sb = new StringBuilder();
                int iMsgCount = queue.Count;
                sb.Append("<script language='javascript'>");
                string sMsg;
                while (iMsgCount-- > 0)
                {
                    sMsg = (string)queue.Dequeue();
                    sMsg = sMsg.Replace("\\n", "\n").Replace("\\\n", "\n").Replace("\\\\n", "\n").Replace("\\\\\n", "\n").Replace("\\\\\\n", "\n");
                    sMsg = sMsg.Replace("\\r", "\r").Replace("\\\r", "\r").Replace("\\\\r", "\r").Replace("\\\\\r", "\r").Replace("\\\\\\r", "\r");
                    sMsg = sMsg.Replace("\\f", "\f").Replace("\\\f", "\f").Replace("\\\\f", "\f").Replace("\\\\\f", "\f").Replace("\\\\\\f", "\f");
                    sMsg = sMsg.Replace("\\b", "\b").Replace("\\\b", "\b").Replace("\\\\b", "\b").Replace("\\\\\b", "\b").Replace("\\\\\\b", "\b");
                    sMsg = sMsg.Replace("\\t", "\t").Replace("\\\t", "\t").Replace("\\\\t", "\t").Replace("\\\\\t", "\t").Replace("\\\\\\t", "\t");
                    sMsg = sMsg.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\b", "\\b").Replace("\f", "\\f").Replace("\t", "\\t");
                    sMsg = sMsg.Replace("\"", "'");
                    sb.Append(@"alert( """ + sMsg + @""" );");
                }
                sb.Append(@"</script>");
                m_executingPages.Remove(HttpContext.Current.Handler);
                HttpContext.Current.Response.Write(sb.ToString());
                                              
            }
        }

        /// <summary>
        /// Only For Anthem
        /// </summary>
        /// <param name="msg"></param>
        public static void ShowCB(string msg)
        {
            //Created by Anson Lin on 12-Apr-2006
            msg = msg.Replace("\\n", "\n").Replace("\\\n", "\n").Replace("\\\\n", "\n").Replace("\\\\\n", "\n").Replace("\\\\\\n", "\n");
            msg = msg.Replace("\\r", "\r").Replace("\\\r", "\r").Replace("\\\\r", "\r").Replace("\\\\\r", "\r").Replace("\\\\\\r", "\r");
            msg = msg.Replace("\\f", "\f").Replace("\\\f", "\f").Replace("\\\\f", "\f").Replace("\\\\\f", "\f").Replace("\\\\\\f", "\f");
            msg = msg.Replace("\\b", "\b").Replace("\\\b", "\b").Replace("\\\\b", "\b").Replace("\\\\\b", "\b").Replace("\\\\\\b", "\b");
            msg = msg.Replace("\\t", "\t").Replace("\\\t", "\t").Replace("\\\\t", "\t").Replace("\\\\\t", "\t").Replace("\\\\\\t", "\t");
            msg = msg.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\b", "\\b").Replace("\f", "\\f").Replace("\t", "\\t"); ;
            msg = msg.Replace("\"", "'");                        
            Anthem.Manager.AddScriptForClientSideEval(string.Format("alert(\"{0}\")", msg) );
        }

        public static void ShowAndReload(string msg)
        {
            msg = msg.Replace("\\n", "\n").Replace("\\\n", "\n").Replace("\\\\n", "\n").Replace("\\\\\n", "\n").Replace("\\\\\\n", "\n");
            msg = msg.Replace("\\r", "\r").Replace("\\\r", "\r").Replace("\\\\r", "\r").Replace("\\\\\r", "\r").Replace("\\\\\\r", "\r");
            msg = msg.Replace("\\f", "\f").Replace("\\\f", "\f").Replace("\\\\f", "\f").Replace("\\\\\f", "\f").Replace("\\\\\\f", "\f");
            msg = msg.Replace("\\b", "\b").Replace("\\\b", "\b").Replace("\\\\b", "\b").Replace("\\\\\b", "\b").Replace("\\\\\\b", "\b");
            msg = msg.Replace("\\t", "\t").Replace("\\\t", "\t").Replace("\\\\t", "\t").Replace("\\\\\t", "\t").Replace("\\\\\\t", "\t");
            msg = msg.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\b", "\\b").Replace("\f", "\\f").Replace("\t", "\\t"); ;
            msg = msg.Replace("\"", "'");   

            System.Web.UI.WebControls.Literal footScript = new System.Web.UI.WebControls.Literal();
            footScript.Text = "<script type=\"text/javascript\">alert(\"" + msg + "\");location=location</script>";
            ((Page)HttpContext.Current.Handler).Controls.Add(footScript);
        }
    }
}