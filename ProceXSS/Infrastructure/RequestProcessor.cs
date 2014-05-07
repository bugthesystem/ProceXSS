using System;
using System.Globalization;
using System.Text;
using System.Web;
using ProceXSS.Configuration;
using ProceXSS.Enums;
using ProceXSS.Interface;
using ProceXSS.Log;
using ProceXSS.Struct;

namespace ProceXSS.Infrastructure
{
    public sealed class RequestProcessor : IRequestProcessor
    {
        private readonly HttpApplication _httpApplication;
        private readonly IXssConfigurationHandler _configurationHandler;
        private readonly IUrlChecker _urlChecker;
        private readonly IRequestCleaner _requestCleaner;
        private readonly IXssDetector _xssDetector;
        private readonly IIpAdressHelper _ipAdressHelper;

        public RequestProcessor(HttpApplication httpApplication, IXssConfigurationHandler configurationHandler, IUrlChecker urlChecker, IRequestCleaner requestCleaner, IXssDetector xssDetector,IIpAdressHelper ipAdressHelper)
        {
            _httpApplication = httpApplication;
            _configurationHandler = configurationHandler;
            _urlChecker = urlChecker;
            _requestCleaner = requestCleaner;
            _xssDetector = xssDetector;
            _ipAdressHelper = ipAdressHelper;
        }


        public void ProcessRequest()
        {
            HttpRequest request = _httpApplication.Request;

            XSSValidationResult validationResult = _xssDetector.HasXssVulnerability(request);

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


            if (request.QueryString.Count > 0)
            {
                _requestCleaner.Clean(request.QueryString, _configurationHandler, encoderType);
            }

            if (request.HttpMethod == "POST")
            {
                if (request.Form.Count > 0)
                {
                    _requestCleaner.Clean(request.Form, _configurationHandler, encoderType);
                }
            }
        }

        private void LogRequestXssWarning(HttpRequest request, XSSValidationResult validationResult)
        {
            InternalLogManager.Instance.Warn(BuildLogMessage(request, validationResult), request);
        }

        private string BuildLogMessage(HttpRequest request, XSSValidationResult validationResult)
        {

            StringBuilder message = new StringBuilder();
            message.AppendFormat("Detected potential xss attack. Time: {0}, IP:{1}, Xss detected request part: {2}",
                DateTime.Now.ToString(CultureInfo.InvariantCulture), _ipAdressHelper.GetIpInformation(request),
                validationResult.DiseasedRequestPart);

            return message.ToString();
        }
    }
}
