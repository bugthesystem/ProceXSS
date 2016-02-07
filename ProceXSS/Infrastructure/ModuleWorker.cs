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
    public sealed class ModuleWorker : IModuleWorker
    {
        private readonly IXssConfigurationHandler _configuration;
        private readonly IUrlChecker _urlChecker;
        private readonly IRequestSanitizer _requestSanitizer;
        private readonly IXssGuard _xssGuard;
        private readonly IIpAdressHelper _ipAdressHelper;
        private readonly ILogger _logger;

        public ModuleWorker(IXssConfigurationHandler configuration, IUrlChecker urlChecker, IRequestSanitizer requestSanitizer,
            IXssGuard xssGuard, IIpAdressHelper ipAdressHelper, ILogger logger)
        {
            _configuration = configuration;
            _urlChecker = urlChecker;
            _requestSanitizer = requestSanitizer;
            _xssGuard = xssGuard;
            _ipAdressHelper = ipAdressHelper;
            _logger = logger;
        }


        public void Attach(HttpApplication httpApplication)
        {
            HttpRequest request = httpApplication.Request;

            ValidateRequestResult validateRequestResult = _xssGuard.HasVulnerability(request);

            if (validateRequestResult.IsValid)
            {
                return;
            }

            if (_configuration.Log.Equals(bool.TrueString))
            {
                LogXssWarning(request, validateRequestResult);
            }

            ProcessInternal(request, httpApplication.Response);
        }

        private void ProcessInternal(HttpRequest request, HttpResponse response)
        {
            switch (_configuration.Mode)
            {
                case "Redirect":
                    {
                        if (!string.IsNullOrEmpty(_configuration.RedirectUrl))
                        {
                            response.Redirect(_configuration.RedirectUrl);
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
                _requestSanitizer.Clean(request.QueryString, _configuration, encoderType);
            }

            if (request.HttpMethod == "POST")
            {
                if (request.Form.Count > 0)
                {
                    _requestSanitizer.Clean(request.Form, _configuration, encoderType);
                }
            }
        }

        private void LogXssWarning(HttpRequest request, ValidateRequestResult validateRequestResult)
        {
            string ipInformation = _ipAdressHelper.GetIpInformation(request);
            _logger.Warn(BuildLogMessage(ipInformation, validateRequestResult));
        }

        private string BuildLogMessage(string ip, ValidateRequestResult validateRequestResult)
        {
            StringBuilder message = new StringBuilder();
            message.AppendFormat("Detected xss vulnerability. Time: {0}, IP:{1}, Request Part: {2}",
                DateTime.Now.ToString(CultureInfo.InvariantCulture), ip,
                validateRequestResult.DiseasedRequestPart);

            return message.ToString();
        }
    }
}
