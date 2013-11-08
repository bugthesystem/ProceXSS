using NLog;

namespace ProceXSS.Log
{
    internal static class XssLogger
    {
        private static readonly Logger Logger;

        static XssLogger()
        {
            LogManager.ThrowExceptions = true;
            Logger = LogManager.GetCurrentClassLogger();

        }

        public static Logger Instance
        {
            get
            {
                return Logger;
            }
        }
    }
}
