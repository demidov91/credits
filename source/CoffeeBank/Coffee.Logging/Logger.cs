using System;
using System.Reflection;
using log4net;

namespace Coffee.Logging
{
    public static class Logger
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Debug(string message)
        {
            Log.Debug(message);
        }

        public static void Info(string message)
        {
            Log.Info(message);
        }

        public static void Warn(string message)
        {
            Log.Warn(message);
        }

        public static void Error(string message)
        {
            Log.Error(message);
        }

        public static void Error(string message, Exception exception)
        {
            Log.Error(message, exception);
        }

        public static void FatalError(string message)
        {
            Log.Fatal(message);
        }

        public static void FatalError(string message, Exception exception)
        {
            Log.Fatal(message, exception);
        }
    }
}
