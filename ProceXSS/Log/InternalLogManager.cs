using NLog;

namespace ProceXSS.Log
{
    public static class InternalLogManager
    {
        private static readonly Logger Logger;

        static InternalLogManager()
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
