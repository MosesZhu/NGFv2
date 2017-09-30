using NGF.Base.Enums;
using NGF.Common;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace NGF.Base.Config
{
    public static class NGFConfig
    {
        private static void InitCache()
        {
            //ConfigurationManager.AppSettings[];
            LastCacheTime = DateTime.Now;
            _cache = new Dictionary<string, string>();
            _cache[ConfigContents.AUTHORITY_MODE] = Convert.ToInt32((AuthorityModeEnum)Enum.Parse(typeof(AuthorityModeEnum), ConfigurationManager.AppSettings[ConfigContents.AUTHORITY_MODE])).ToString();
        }


        private static readonly long CACHE_SURVIVAL_TIME = 100000;
        private static DateTime LastCacheTime
        {
            get;
            set;
        }
        private static bool IsCacheOverTime()
        {
            return (DateTime.Now - LastCacheTime).Seconds > CACHE_SURVIVAL_TIME;
        }
        private static string FindCacheValue(string key, string defaultValue = null)
        {
            return ConfigurationManager.AppSettings[key];
            //return Cache[key] == null ? Cache[key] : defaultValue;
            //return Cache.FirstOrDefault(kv => kv.Key == key) == null ? Cache.FirstOrDefault(kv => kv.Key == key).Value : defaultValue;
        }

        private static Dictionary<string, string> _cache;
        public static Dictionary<string, string> Cache
        {
            get {
                if (_cache == null || IsCacheOverTime())
                {
                    InitCache();
                }

                return _cache;
            }
            set {
                _cache = value;
            }
        }

        public static NGFSystemModeEnum SystemMode
        {
            get {
                return ConfigurationManager.AppSettings[ConfigContents.NGF_SYSTEM_MODE] == null ? NGFSystemModeEnum.Mulity :
                    (NGFSystemModeEnum)Enum.Parse(typeof(NGFSystemModeEnum), ConfigurationManager.AppSettings[ConfigContents.NGF_SYSTEM_MODE]);
            }
        }

        public static NGFAuthModeEnum NGFAuthMode
        {
            get
            {
                return ConfigurationManager.AppSettings[ConfigContents.NGF_AUTH_MODE] == null ? NGFAuthModeEnum.WFK :
                    (NGFAuthModeEnum)Enum.Parse(typeof(NGFAuthModeEnum), ConfigurationManager.AppSettings[ConfigContents.NGF_AUTH_MODE]);
            }
        }

        public static string NGFSystemId
        {
            get
            {
                return ConfigurationManager.AppSettings[ConfigContents.NGF_SYSTEM_ID] == null ? string.Empty :
                    ConfigurationManager.AppSettings[ConfigContents.NGF_SYSTEM_ID];
            }
        }

        public static string NGFSystemName
        {
            get
            {
                return ConfigurationManager.AppSettings[ConfigContents.NGF_SYSTEM_NAME] == null ? string.Empty :
                    ConfigurationManager.AppSettings[ConfigContents.NGF_SYSTEM_NAME];
            }
        }

        public static string PermissionSystemId
        {
            get
            {
                return ConfigurationManager.AppSettings[ConfigContents.PERMISSION_SYSTEM_ID];
            }
        }

        public static string WfkResourceUrl
        {
            get
            {
                return ConfigurationManager.AppSettings[ConfigContents.WFK_RESOURCE_URL] == null ? "" : ConfigurationManager.AppSettings[ConfigContents.WFK_RESOURCE_URL];
            }
        }

        public static string NGFPortalHeaderInfo
        {
            get
            {
                string header = ConfigurationManager.AppSettings[ConfigContents.NGF_PORTAL_HEADER_INFO] == null ? NGFPortalTitle :
                    ConfigurationManager.AppSettings[ConfigContents.NGF_PORTAL_HEADER_INFO];

                return header;
            }
        }

        public static string NGFEnvironment
        {
            get
            {
                string env = ConfigurationManager.AppSettings[ConfigContents.NGF_ENVIRONMENT] == null ? "" :
                    ConfigurationManager.AppSettings[ConfigContents.NGF_ENVIRONMENT];

                return env;
            }
        }

        public static bool NGFEnvironmentVisible
        {
            get
            {
                return ConfigurationManager.AppSettings[ConfigContents.NGF_ENVIRONMENT_VISIBLE] != null && ConfigurationManager.AppSettings[ConfigContents.NGF_ENVIRONMENT_VISIBLE] == "true";
            }
        }

        public static string NGFPortalTitle
        {
            get
            {
                string title = ConfigurationManager.AppSettings[ConfigContents.NGF_PORTAL_TITLE] == null ? "" :
                    ConfigurationManager.AppSettings[ConfigContents.NGF_PORTAL_TITLE];
                if (string.IsNullOrEmpty(title) && NGFConfig.SystemMode == NGFSystemModeEnum.Single)
                {
                    title = NGFSystemName;
                }
                if (string.IsNullOrEmpty(title)) {
                    title = "Portal";
                }

                return title;
            }
        }

        public static string NGFPortalFooterInfo
        {
            get
            {
                return ConfigurationManager.AppSettings[ConfigContents.NGF_PORTAL_FOOTER_INFO] == null ? string.Empty :
                    ConfigurationManager.AppSettings[ConfigContents.NGF_PORTAL_FOOTER_INFO];
            }
        }

        public static int TokenOverdueMiniute
        {
            get
            {
                return Convert.ToInt32(FindCacheValue(ConfigContents.TOKEN_OVERDUE_MINIUTE));
            }
        }

        public static bool NGFNtAuth
        {
            get
            {
                return ConfigurationManager.AppSettings[ConfigContents.NGF_NT_AUTH] != null && ConfigurationManager.AppSettings[ConfigContents.NGF_NT_AUTH] == "true";
            }
        }

        public static string NGFNtProduct
        {
            get
            {
                string env = ConfigurationManager.AppSettings[ConfigContents.NGF_NT_PRODUCT] == null ? "" :
                    ConfigurationManager.AppSettings[ConfigContents.NGF_NT_PRODUCT];

                return env;
            }
        }

        public static string NGFNtOrg
        {
            get
            {
                string env = ConfigurationManager.AppSettings[ConfigContents.NGF_NT_ORG] == null ? "" :
                    ConfigurationManager.AppSettings[ConfigContents.NGF_NT_ORG];

                return env;
            }
        }

        public static bool NGFCleanVersion
        {
            get
            {
                return ConfigurationManager.AppSettings[ConfigContents.NGF_CLEAN_VERSION] != null && ConfigurationManager.AppSettings[ConfigContents.NGF_CLEAN_VERSION] == "true";
            }
        }

        public static string NGFCleanVersionDefaultDomain
        {
            get
            {
                string env = ConfigurationManager.AppSettings[ConfigContents.NGF_CLEAN_VERSION_DEFAULT_DOMAIN] == null ? "" :
                    ConfigurationManager.AppSettings[ConfigContents.NGF_CLEAN_VERSION_DEFAULT_DOMAIN];

                return env;
            }
        }
    }
}
