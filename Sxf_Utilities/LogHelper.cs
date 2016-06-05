using System;
using System.IO;
using log4net;
using log4net.Config;

namespace Sxf_Utilities
{
    public static class LogHelper
    {
        static LogHelper()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }

        /// <summary>
        ///     输出日志到Log4Net
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>

        #region static void WriteLog(Type t, Exception ex)
        public static void WriteLog(Type t, Exception ex)
        {
            var log = LogManager.GetLogger(t);
            log.Error("Error", ex);
        }

        #endregion

        /// <summary>
        ///     输出日志到Log4Net
        /// </summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public static void WriteLog(Type t, string msg)
        {
            var log = LogManager.GetLogger(t);
            log.Error(msg);
        }


        public static void Error(string name, string msg)
        {
            var log = LogManager.GetLogger(name);
            log.Error(msg);
        }

        public static void Info(Type t, string msg)
        {
            var log = LogManager.GetLogger(t);
            log.Info(msg);
        }

        public static void Info(string name, string msg)
        {
            var log = LogManager.GetLogger(name);
            log.Info(msg);
        }
    }
}