using NGF.Base.SSO;
using ITS.WebFramework.PermissionComponent.ServiceProxy;
using System.Web;
using System.Web.SessionState;

namespace NGF.Base.Base
{
    public class HttpHandlerBase : IHttpHandler, IReadOnlySessionState
    {
        public UserDTO UserInfo
        {
            get
            {
                return NGFSSOContext.Current.UserInfo;
            }
        }
        public virtual void ProcessRequest(HttpContext context)
        {

        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}
