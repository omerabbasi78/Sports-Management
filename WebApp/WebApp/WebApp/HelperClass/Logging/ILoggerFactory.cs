using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebApp.HelperClass.Logging
{

    public interface ILoggerFactory
    {
        /// <summary>
        /// Create a new ILog
        /// </summary>
        /// <returns>The ILog created</returns>
        ILogger Create();
        ILogger WriteLog(LoggerFactory.LogLevel logLevel, string log);
        ILogger WriteLog(LoggerFactory.LogLevel logLevel, string log, HttpContext context, Exception ex);
    }
}
