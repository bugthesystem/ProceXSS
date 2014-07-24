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
        private readonly IXssConfigurationHandler _configuration;
        private readonly IUrlChecker _urlChecker;
        private readonly IRequestCleaner _requestCleaner;
        private readonly IXssDetector _xssDetector;
        private readonly IIpAdressHelper _ipAdressHelper;
        private readonly ILogger _logger;

        public RequestProcessor(HttpApplication httpApplication, IXssConfigurationHandler configuration,
            IUrlChecker urlChecker, IRequestCleaner requestCleaner,
            IXssDetector xssDetector, IIpAdressHelper ipAdressHelper, ILogger logger)
        {
            _httpApplication = httpApplication;
            _configuration = configuration;
            _urlChecker = urlChecker;
            _requestCleaner = requestCleaner;
            _xssDetector = xssDetector;
            _ipAdressHelper = ipAdressHelper;
            _logger = logger;
        }


        public void ProcessRequest()
        {
            HttpRequest request = _httpApplication.Request;

            RequestValidationResult validationResult = _xssDetector.HasXssVulnerability(request);

            if (validationResult.IsValid)
            {
                return;
            }

            if (_configuration.Log.Equals(bool.TrueString))
            {
                LogXssWarning(request, validationResult);
            }

            ProcessInternal(request);

        }

        private void ProcessInternal(HttpRequest request)
        {
            switch (_configuration.Mode)
            {
                case "Redirect":
                    {
                        if (!string.IsNullOrEmpty(_configuration.RedirectUrl))
                        {
                            _httpApplication.Response.Redirect(_configuration.RedirectUrl);
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
                _requestCleaner.Clean(request.QueryString, _configuration, encoderType);
            }

            if (request.HttpMethod == "POST")
            {
                if (request.Form.Count > 0)
                {
                    _requestCleaner.Clean(request.Form, _configuration, encoderType);
                }
            }
        }

        private void LogXssWarning(HttpRequest request, RequestValidationResult validationResult)
        {
            string ipInformation = _ipAdressHelper.GetIpInformation(request);
            _logger.Warn(BuildLogMessage(ipInformation, validationResult));
        }

        private string BuildLogMessage(string ip, RequestValidationResult validationResult)
        {
            StringBuilder message = new StringBuilder();
            message.AppendFormat("Detected xss vulnerability. Time: {0}, IP:{1}, Request Part: {2}",
                DateTime.Now.ToString(CultureInfo.InvariantCulture), ip,
                validationResult.DiseasedRequestPart);

            return message.ToString();
        }
    }
}
