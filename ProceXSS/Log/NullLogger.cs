using System;
using System.Web;

namespace ProceXSS.Log
{
    internal class NullLogger : ILogger
    {
        public void Error(string message, Exception exception)
        {
            
        }

        public void Warn(string message, HttpRequest request)
        {
            
        }
    }
}