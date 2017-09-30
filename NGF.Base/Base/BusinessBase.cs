using NGF.Base.SSO;
using ITS.WebFramework.PermissionComponent.ServiceProxy;

namespace NGF.Base.Base
{
    public class BusinessBase
    {
        public UserDTO _TempUser = null;
        public BusinessBase()
        {
        }

        public BusinessBase(UserDTO user)
        {
            this._TempUser = user;
        }

        public UserDTO UserInfo
        {
            get
            {
                if (_TempUser != null)
                {
                    return _TempUser;
                }
                return NGFSSOContext.Current.UserInfo;
            }
        }
    }
}
