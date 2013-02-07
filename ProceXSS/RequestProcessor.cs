using System;
using System.Globalization;
using System.Text;
using System.Web;
using ProceXSS.Configuration;
using ProceXSS.Enums;
using ProceXSS.Extensions;
using ProceXSS.Interface;
using ProceXSS.Log;
using ProceXSS.Struct;

namespace ProceXSS
{
    public sealed class RequestProcessor : IRequestProcessor
    {
        private readonly HttpApplication _httpApplication;
        private readonly ProceXssConfigurationHandler _configurationHandler;
        private readonly IUrlChecker _urlChecker;

        public RequestProcessor(HttpApplication httpApplication, ProceXssConfigurationHandler configurationHandler, IUrlChecker urlChecker)
        {
            _httpApplication = httpApplication;
            _configurationHandler = configurationHandler;
            _urlChecker = urlChecker;
        }


        public void ProcessRequest()
        {
            HttpRequest request = _httpApplication.Request;

            ValidationResult validationResult = request.XssAttackExist(_configurationHandler);

            if (validationResult.IsValid)
            {
                return;
            }

            if (_configurationHandler.Log.Equals(bool.TrueString))
            {
                LogRequestXssWarning(request, validationResult);
            }

            ProcessInternal(request);

        }

        private void ProcessInternal(HttpRequest request)
        {
            switch (_configurationHandler.Mode)
            {
                case "Redirect":
                    {
                        if (!string.IsNullOrEmpty(_configurationHandler.RedirectUrl))
                        {
                            _httpApplication.Response.Redirect(_configurationHandler.RedirectUrl);
                        }
                        break;
                    }
                case "Ignore":
                    {
                        bool urlInExcludeList = _urlChecker.ExistInExcludeList(request.RawUrl);

                        if (!urlInExcludeList)
                        {
                            ExecuteCleaner(request, EncoderType.AutoDetect);
                        }
                        break;
                    }
            }
        }

        private void ExecuteCleaner(HttpRequest request, EncoderType encoderType)
        {
            IRequestCleaner requestCleaner = new RequestCleaner();

            if (request.QueryString.Count > 0)
            {
                requestCleaner.Clean(request.QueryString, _configurationHandler, encoderType);
            }

            if (request.HttpMethod == "POST")
            {
                if (request.Form.Count > 0)
                {
                    requestCleaner.Clean(request.Form, _configurationHandler, encoderType);
                }
            }
        }

        private void LogRequestXssWarning(HttpRequest request, ValidationResult validationResult)
        {
            LibraryLogger.Instance.Warn(BuildLogMessage(request, validationResult), request);
        }

        private string BuildLogMessage(HttpRequest request, ValidationResult validationResult)
        {

            StringBuilder message = new StringBuilder();
            message.AppendFormat("Detected potential xss attack. Time: {0}, IP:{1}, Xss detected request part: {2}",
                DateTime.Now.ToString(CultureInfo.InvariantCulture), request.GetIPInformation(),
                validationResult.InfectedRequestPart);

            return message.ToString();
        }
    }
}
