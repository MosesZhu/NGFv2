using NGF.Base.Config;
using NGF.Base.SSO;
using NGF.Base.Utility;
using NGF.Model.DTO;
using NGF.Model.Entity;
using ITS.Data;
using ITS.WebFramework.PermissionComponent.ServiceProxy;
using System;
using System.Web.Services;
using WSC;

namespace NGF.Base.Base
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class PageServiceBase : WebService
    {
        public UserDTO _TempUser = null;

        public PageServiceBase()
        {
            if (!Context.Request.Url.ToString().ToUpper().Contains(UNCHECK_URL.ToUpper()) && !ValidateToken())
            {
                throw new Exception("");
            }
        }

        public PageServiceBase(UserDTO user)
        {
            _TempUser = user;
        }

        public DbSession mDb;
        public DbSession Db
        {
            get
            {
                return this.mDb ?? (this.mDb = DBUtility.Db);
            }
        }

        public DbSession mNgfDb;
        public DbSession NGFDb
        {
            get
            {
                return this.mNgfDb ?? (this.mNgfDb = DBUtility.NGFDb);
            }
        }

        private DbSession mWFKDb;
        public DbSession WFKDb
        {
            get
            {
                return this.mWFKDb ?? (this.mWFKDb = DBUtility.WFKDb);
            }
        }

        public DbSession mWSCDb;
        public DbSession WSCDb
        {
            get
            {
                return this.mWSCDb ?? (this.mWSCDb = DBUtility.WSCDb);
            }
        }

        //private UserDTO mUserInfo;
        public UserDTO UserInfo {
            get {
                if (_TempUser != null)
                {
                    return _TempUser;
                }
                if (NGFConfig.NGFAuthMode == Enums.NGFAuthModeEnum.WSC)
                {
                    UserDTO userInfo = new UserDTO()
                    {
                        User_Name = GlobalDefinition.Cookie_LoginUser
                    };
                    return userInfo;
                }
                else
                {
                    return NGFSSOContext.Current.UserInfo;
                }
            }
        }

        public string ProductName
        {
            get
            {
                return NGFSSOContext.Current.ProductName;
            }
        }

        public Guid ProductId
        {
            get
            {
                return NGFSSOContext.Current.ProductId;
            }
        }

        public Guid OrgId
        {
            get
            {
                return NGFSSOContext.Current.OrgId;
            }
        }

        public PermissionService mPermissionService;
        public PermissionService PermissionService
        {
            get {
                if (mPermissionService == null)
                {
                    mPermissionService = new PermissionService();
                    mPermissionService.Url = ITS.WebFramework.Configuration.Config.Global.PermissionServiceUrl;

                    if (UserInfo != null)
                    {
                        mPermissionService.PermissionServiceSoapValue = new PermissionServiceSoap
                        {
                            Org_Id = NGFSSOContext.Current.OrgId,
                            User_Id = UserInfo.User_ID,
                            Product_Id = NGFSSOContext.Current.ProductId
                        };
                    }
                }
                return mPermissionService;
            }
        }

        public string Language {
            get
            {
                return NGFSSOContext.Language;
            }
        }

        public static string UNCHECK_URL = "LoginService.asmx";
        public bool ValidateToken()
        {
            return TokenUtility.ValidWfkToken();
        }
    }
}
