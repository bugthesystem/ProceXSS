using NLog;

namespace ProceXSS.Log
{
    internal static class XSSLogManager
    {
        private static readonly Logger Logger;

        static XSSLogManager()
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
