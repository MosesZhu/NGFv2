using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NGF.Web.Startup))]
namespace NGF.Web
{
    public partial class Startup {

        public void Configuration(IAppBuilder app) {
            //ConfigureAuth(app);
            //app.Run(context => {
            //    context.Response.ContentType = "text/plain";
            //    return context.Response.WriteAsync("Hi Moses");
            //});
        }

    }
}
