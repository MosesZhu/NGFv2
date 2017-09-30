using ITS.Data;
using System.Configuration;

namespace NGF.Base.Utility
{
    public class DBUtility
    {
        private static DbSession mDBSession;
        public static DbSession Db
        {
            get
            {
                return CreateDbSession("system");
            }
            set
            {
                mDBSession = value;
            }
        }

        private static DbSession mNgfDBSession;
        public static DbSession NGFDb
        {
            get
            {
                return CreateDbSession("ngf");
            }
            set
            {
                mNgfDBSession = value;
            }
        }

        private static DbSession mWscDBSession;
        public static DbSession WSCDb
        {
            get
            {
                return CreateDbSession("wscConnectionString");
            }
            set
            {
                mWscDBSession = value;
            }
        }

        private static DbSession mWfkDBSession;
        public static DbSession WFKDb
        {
            get
            {
                return CreateDbSession("WebFramework");
            }
            set
            {
                mWfkDBSession = value;
            }
        }

        /// <summary>
        /// Create a DB session
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns>DbSession</returns>
        public static DbSession CreateDbSession(string settingName)
        {
            //读取config文件，并解析连接字符串            
            string connString = ConfigurationManager.ConnectionStrings[settingName].ConnectionString;
            string providerName = ConfigurationManager.ConnectionStrings[settingName].ProviderName;

            ConnectionStringSettings connectionStringSettings = new ConnectionStringSettings();
            connectionStringSettings.ConnectionString = connString;
            connectionStringSettings.ProviderName = providerName;
            connectionStringSettings.Name = settingName;

            return new DbSession(connectionStringSettings);
        }
    }
}
