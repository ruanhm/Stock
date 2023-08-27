using Microsoft.Extensions.Logging.Configuration;
using NLog;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Stock.Common
{
    public class LogHelper
    {
        private static readonly string Prefix = "Log_";

        private static Logger GetLogger(string logName)
        {

            return LogManager.GetLogger(Prefix + logName);
        }

        public static void Trace(object message, Exception ex = null, [CallerMemberName] string CallerMemberName = "")
        {
            var logName = new StackFrame(1).GetMethod().DeclaringType.FullName + "." + CallerMemberName;
            var Logger = GetLogger(logName); ;
            if (ex == null)
            {
                Logger.Trace(message.ToString());
            }
            else
            {
                Logger.Trace(message.ToString() + "|" + ex.ToString());
            }
        }
        public static void Debug(object message, Exception ex = null, [CallerMemberName] string CallerMemberName = "")
        {
            var logName = new StackFrame(1).GetMethod().DeclaringType.FullName + "." + CallerMemberName;
            var Logger = GetLogger(logName); ;
            if (ex == null)
            {
                Logger.Debug(message.ToString());
            }
            else
            {
                Logger.Debug(message.ToString() + "|" + ex.ToString());
            }
        }
        public static void Info(object message, Exception ex = null, [CallerMemberName] string CallerMemberName = "")
        {

            var logName = new StackFrame(1).GetMethod().DeclaringType.FullName + "." + CallerMemberName;
            var Logger = GetLogger(logName); ;
            if (ex == null)
            {
                Logger.Info(message.ToString());
            }
            else
            {
                Logger.Info(message.ToString() + "|" + ex.ToString());
            }
        }
        public static void Warn(object message, Exception ex = null, [CallerMemberName] string CallerMemberName = "")
        {
            var logName = new StackFrame(1).GetMethod().DeclaringType.FullName + "." + CallerMemberName;
            var Logger = GetLogger(logName); ;
            if (ex == null)
            {
                Logger.Warn(message.ToString());
            }
            else
            {
                Logger.Warn(message.ToString() + "|" + ex.ToString());
            }
        }
        public static void Error(object message, Exception ex = null, [CallerMemberName] string CallerMemberName = "")
        {
            var logName = new StackFrame(1).GetMethod().DeclaringType.FullName + "." + CallerMemberName;
            var Logger = GetLogger(logName); ;
            if (ex == null)
            {
                Logger.Error(message.ToString());
            }
            else
            {
                Logger.Error(message.ToString() + "|" + ex.ToString());
            }
        }

    }
}
