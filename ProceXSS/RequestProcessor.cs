using System;
using System.Globalization;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using ProceXSS.Enums;
using ProceXSS.Interface;
using ProceXSS.Log;
using ProceXSS.Struct;
using ProceXSS.Configuration;


namespace ProceXSS
{
    internal sealed class RequestProcessor : IRequestProcessor
    {
        private readonly HttpApplication _httpApplication;
        private readonly ProceXssConfigurationHandler _configurationHandler;

        public RequestProcessor(HttpApplication httpApplication, ProceXssConfigurationHandler configurationHandler)
        {
            _httpApplication = httpApplication;
            _configurationHandler = configurationHandler;
        }


        public void ProcessRequest()
        {
            if (_httpApplication.Request == null)
            {
                return;
            }

            HttpRequest request = _httpApplication.Request;

            ValidationResult validationResult = request.CheckPotentialXssAttack(_configurationHandler);

            if (validationResult.IsValid)
            {
                return;
            }

            if (_configurationHandler.Log.Equals(bool.TrueString))
            {
                LogRequestXssWarning(request, validationResult);
            }

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
                        bool urlInExcludeList = CheckUrlInExcludeList(request);

                        if (!urlInExcludeList)
                        {
                            ProcessRequestByEncode(request, EncoderType.AutoDetect);
                        }
                        break;
                    }
            }

        }

        private void ProcessRequestByEncode(HttpRequest request, EncoderType encoderType)
        {
            IRequestCleaner requestCleaner = new RequestCleaner();

            if (request.QueryString != null && request.QueryString.Count > 0)
            {
                requestCleaner.Clean(request.QueryString, _configurationHandler, encoderType);
            }

            if (request.HttpMethod == "POST")
            {
                if (request.Form != null && request.Form.Count > 0)
                {
                    requestCleaner.Clean(request.Form, _configurationHandler, encoderType);
                }
            }
        }

        private bool CheckUrlInExcludeList(HttpRequest request)
        {
            string url = request.RawUrl.Split('?')[0];

            bool result = false;

            for (int i = 0; i < _configurationHandler.ExcludeUrls.Count; i++)
            {

                if (_configurationHandler.ExcludeUrls[i].Value != url)
                {
                    continue;
                }

                result = true;
                break;
            }

            return result;
        }


        private void LogRequestXssWarning(HttpRequest request, ValidationResult validationResult)
        {
            XSSLogManager.Instance.Warn(BuildLogMessage(request, validationResult), request);
        }

        private string BuildLogMessage(HttpRequest request, ValidationResult validationResult)
        {
            string user = string.Empty;
            IPrincipal principal = Thread.CurrentPrincipal;

            if (principal != null)
            {
                IIdentity identity = principal.Identity;
                user = identity.Name;
            }
            StringBuilder message = new StringBuilder();
            message.AppendFormat("Potaential xss attcak has prevented. Time: {0}, User:{1}, IP:{2}, Dirty request part: {3}",
                DateTime.Now.ToString(CultureInfo.InvariantCulture), user
                , GetIPInformation(request),
                validationResult.DirtyRequestPart);

            return message.ToString();
        }

        private string GetIPInformation(HttpRequest request)
        {
            string ip = request.ServerVariables["HTTP_CLIENT_IP"];
            string alternateIp = request.ServerVariables["REMOTE_ADDR"];

            string result = (String.IsNullOrEmpty(ip))
                                ? (String.IsNullOrEmpty(alternateIp) ? String.Empty : alternateIp)
                                : ip;

            return result;
        }
    }
}
