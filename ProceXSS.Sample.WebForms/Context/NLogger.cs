using System;
using NLog;
using ProceXSS.Log;

namespace ProceXSS.Sample.WebForms.Context
{
    public class NLogger : ILogger
    {
        private readonly Logger _logger;

        public NLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        #region Implementation of ILogger

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Debug(string message, Exception exception)
        {
            _logger.Debug(message, exception);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            _logger.Error(message, exception);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Info(string message, Exception exception)
        {
            _logger.Info(message, exception);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Warn(string message, Exception exception)
        {
            _logger.Warn(message, exception);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }

        public void Fatal(string message, Exception exception)
        {
            _logger.Fatal(message, exception);
        }
        #endregion
    }
}