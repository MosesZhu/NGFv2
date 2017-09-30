using System;
using System.Collections.Generic;
 
using System.Text;
using System.Text.RegularExpressions;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;

namespace WSC.Common
{
   public class Logger
    {


        private static ILog _defaultLogger;
        private static string LoggerName
        {
            get
            {
                System.Diagnostics.StackFrame stackFrame = new System.Diagnostics.StackFrame(2);
                System.Reflection.MethodBase method = stackFrame.GetMethod();
                Type declaringType = method.DeclaringType;
                string name = method.Name;
                return string.Format("{0}\\{1}()", declaringType.FullName.Replace(".", "_"), name);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static ILog Instance
        {
            get
            {
                if (_defaultLogger == null)
                {

                    _defaultLogger = GetLogger("CurrentLog");
                }
                return _defaultLogger;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logFileName"></param>
        /// <returns></returns>
        public static ILog GetLogger(string logFileName)
        {
            Regex regex = new Regex("^[a-zA-Z][:]\\\\");
            string file = regex.IsMatch(logFileName) ? logFileName : string.Format("logs\\{0}.txt", logFileName);
            RollingFileAppender rollingFileAppender = new RollingFileAppender();
            rollingFileAppender.Name = string.Format("{0}RollingFileAppender", logFileName);
            rollingFileAppender.File = file;
            rollingFileAppender.AppendToFile = true;
            rollingFileAppender.LockingModel = new FileAppender.MinimalLock();
            rollingFileAppender.DatePattern = "yyyyMMdd";
            rollingFileAppender.RollingStyle = RollingFileAppender.RollingMode.Date;
            rollingFileAppender.MaxSizeRollBackups = 100;
            rollingFileAppender.Layout = new PatternLayout("%-5p %d %5rms %-25.100c{1} - %m%n");
            rollingFileAppender.ActivateOptions();
            BasicConfigurator.Configure(rollingFileAppender);
            return LogManager.GetLogger("");
        }

    }
}
