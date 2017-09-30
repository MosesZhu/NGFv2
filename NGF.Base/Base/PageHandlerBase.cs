using Cube.Common;
using Cube.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace Cube.Base
{
    public class PageHandlerBase : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            ResultDTO result = new ResultDTO();
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string functionName = RequestUtility.GetQueryString<string>("func");
            
            if (!String.Equals(functionName, "login", StringComparison.CurrentCultureIgnoreCase)
                && string.IsNullOrEmpty(RequestUtility.GetQueryString<string>(SessionContents.SSO_TOKEN))
                && string.IsNullOrEmpty(RequestUtility.GetHeader<string>(SessionContents.SSO_TOKEN)))
            {
                result.success = false;
                result.errorcode = ErrorCode.NO_SSO_INFO;
                context.Response.Write(serializer.Serialize(result));
                context.Response.End();
            }
            
            result = (ResultDTO)GetType().GetMethod(functionName).Invoke(this, new object[0]);


            context.Response.Write(serializer.Serialize(result));
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
