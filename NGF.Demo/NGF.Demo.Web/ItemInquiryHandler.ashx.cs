using Cube.Base.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CubeDemo.Web
{
    /// <summary>
    /// ItemInquiryHandler 的摘要说明
    /// </summary>
    public class ItemInquiryHandler : HttpHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string action = context.Request.Form["action"];
            if (action.Equals("GetUserInfo", StringComparison.CurrentCultureIgnoreCase))
            {
                GetUserInfo();
            }
        }

        private void GetUserInfo()
        {
            HttpContext.Current.Response.Write("{'user_name':'" + UserInfo.User_Name + "'}");

            HttpContext.Current.Response.End();
        }
    }
}