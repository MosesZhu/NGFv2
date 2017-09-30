namespace NGF.Common
{   
    public class SessionContents
    {
        public static readonly string SESSION_ID = "SessionID";
        public static readonly string SSO_TOKEN = "SSOToken";
        public static readonly string DEBUG_MODE = "DebugMode";
        public static readonly string SIGN = "Sign";
        public static readonly string PERMISSION_SERVICE_URL = "PermissionServiceUrl";
    }

    public class ConfigContents
    {
        public static readonly string AUTHORITY_MODE = "AuthorityMode";
        public static readonly string NGF_SYSTEM_MODE = "NGFSystemMode";
        public static readonly string NGF_AUTH_MODE = "NGFAuthMode"; 
        public static readonly string NGF_SYSTEM_ID = "NGFSystemId";
        public static readonly string PERMISSION_SYSTEM_ID = "PermissionSystemId";
        public static readonly string NGF_SYSTEM_NAME = "NGFSystemName";
        public static readonly string NGF_PORTAL_TITLE = "NGFPortalTitle";
        public static readonly string NGF_PORTAL_HEADER_INFO = "NGFPortalHeaderInfo";
        public static readonly string NGF_ENVIRONMENT = "NGFEnvironment";
        public static readonly string NGF_ENVIRONMENT_VISIBLE = "NGFEnvironmentVisible";
        public static readonly string NGF_PORTAL_FOOTER_INFO = "NGFPortalFooterInfo";
        public static readonly string TOKEN_OVERDUE_MINIUTE = "NGFTokenOverdueMiniute";
        public static readonly string WFK_RESOURCE_URL = "WfkResourceUrl";
        public static readonly string NGF_NT_AUTH = "NGFNtAuth";
        public static readonly string NGF_NT_PRODUCT = "NGFNtProduct";
        public static readonly string NGF_NT_ORG = "NGFNtOrg";
        public static readonly string NGF_CLEAN_VERSION = "NGFCleanVersion";
        public static readonly string NGF_CLEAN_VERSION_DEFAULT_DOMAIN = "NGFCleanVersionDefaultDomain";
    }

    public class ErrorCode
    {
        public static readonly string NO_SSO_INFO = "E0001";
        public static readonly string USER_AUTH_FAILED = "E0002";
    }
}
