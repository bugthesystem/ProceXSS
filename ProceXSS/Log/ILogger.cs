using System;
using System.Web;

namespace ProceXSS.Log
{
    public interface ILogger
    {
        void Error(string message, Exception exception);
        void Warn(string message, HttpRequest request);
    }
}