using NGF.Base.Base;
using ITS.WebFramework.PermissionComponent.ServiceProxy;

namespace NGF.Demo.Web
{
    public class ItemBusiness : BusinessBase
    {
        public UserDTO getUserInfo()
        {
            return UserInfo;
        }
    }
}