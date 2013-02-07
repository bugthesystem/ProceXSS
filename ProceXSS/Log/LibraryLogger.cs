using NLog;

namespace ProceXSS.Log
{
    internal static class LibraryLogger
    {
        private static readonly Logger Logger;

        static LibraryLogger()
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
