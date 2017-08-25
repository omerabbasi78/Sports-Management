using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.HelperClass.Logging
{
    public static class LoggerFactory
    {
        #region Members

        static ILoggerFactory _currentLogFactory = null;

        #endregion

        #region Public Methods
        public enum LogLevel
        {
            DEBUG,
            ERROR,
            FATAL,
            INFO,
            WARN
        }

        private static ILog logger = LogManager.GetLogger(typeof(LoggerFactory));
        static LoggerFactory()
        {
            XmlConfigurator.Configure();
        }
        public static void WriteLog(LogLevel logLevel, string log)
        {
            if (logLevel.Equals(LogLevel.DEBUG))
            {
                logger.Debug(log);
            }
            else if (logLevel.Equals(LogLevel.ERROR))
            {
                logger.Error(log);
            }
            else if (logLevel.Equals(LogLevel.FATAL))
            {
                logger.Fatal(log);
            }
            else if (logLevel.Equals(LogLevel.INFO))
            {
                logger.Info(log);
            }
            else if (logLevel.Equals(LogLevel.WARN))
            {
                logger.Warn(log);
            }

        }
        public static void WriteLog(LogLevel logLevel, string log, HttpContext context, Exception ex)
        {
            log = log + Environment.NewLine + "Rawurl : " + context.Request.RawUrl + Environment.NewLine + "URL : " + context.Request.Url.ToString() + Environment.NewLine +
                  "Message : " + ex.Message + Environment.NewLine + "InnerException : " + ex.InnerException + Environment.NewLine + "Stacktrace : " + ex.StackTrace;
            if (logLevel.Equals(LogLevel.DEBUG))
            {
                logger.Debug(log);
            }
            else if (logLevel.Equals(LogLevel.ERROR))
            {
                logger.Error(log);
            }
            else if (logLevel.Equals(LogLevel.FATAL))
            {
                logger.Fatal(log);
            }
            else if (logLevel.Equals(LogLevel.INFO))
            {
                logger.Info(log);
            }
            else if (logLevel.Equals(LogLevel.WARN))
            {
                logger.Warn(log);
            }

        }
        /// <summary>
        /// Set the  log factory to use
        /// </summary>
        /// <param name="logFactory">Log factory to use</param>
        public static void SetCurrent(ILoggerFactory logFactory)
        {
            _currentLogFactory = logFactory;
        }

        /// <summary>
        /// Createt a new Log
        /// </summary>
        /// <returns>Created ILog</returns>
        public static ILogger CreateLog()
        {
            return (_currentLogFactory != null) ? _currentLogFactory.Create() : null;
        }

        #endregion
    }
}