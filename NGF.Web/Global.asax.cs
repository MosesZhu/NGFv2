using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace NGF.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Session_Start(object sender, EventArgs e) 
        {
            string s = "";
        }

        void Application_Error(object sender, EventArgs e) 
        { 

        }
    }
}