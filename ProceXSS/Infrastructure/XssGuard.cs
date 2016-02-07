using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using ProceXSS.Configuration;
using ProceXSS.Enums;
using ProceXSS.Interface;
using ProceXSS.Log;
using ProceXSS.Struct;

namespace ProceXSS.Infrastructure
{
    public class XssGuard : IXssGuard
    {
        private readonly IXssConfigurationHandler _configuration;
        private readonly IRegexHelper _regexHelper;
        private readonly ILogger _logger;
        private Regex _xssDetectionRegex;

        public XssGuard(IXssConfigurationHandler configuration, IRegexHelper regexHelper, ILogger logger)
        {
            _configuration = configuration;
            _regexHelper = regexHelper;
            _logger = logger;
        }

        public ValidateRequestResult HasVulnerability(HttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(_configuration.ControlRegex))
            {
                _xssDetectionRegex = new Regex(_regexHelper.XssPattern, RegexOptions.IgnoreCase);
            }
            else
            {
                try
                {
                    _xssDetectionRegex = new Regex(HttpUtility.HtmlDecode(_configuration.ControlRegex), RegexOptions.IgnoreCase);
                }
                catch
                {
                    _xssDetectionRegex = new Regex(_regexHelper.XssPattern, RegexOptions.IgnoreCase);
                }
            }

            ValidateRequestResult result = new ValidateRequestResult
            {
                IsValid = true,
                DiseasedRequestPart = DiseasedRequestPart.None
            };

            if (request != null)
            {
                string queryString = request.QueryString.ToString();

                if (!string.IsNullOrEmpty(queryString) && _regexHelper.ExecFor(_xssDetectionRegex, queryString))
                {
                    result.IsValid = false;
                    result.DiseasedRequestPart = DiseasedRequestPart.QueryString;
                }

                if (request.HttpMethod.Equals("POST", StringComparison.InvariantCultureIgnoreCase))
                {
                    string formPostValues;

                    try
                    {
                        formPostValues = request.Form.ToString();
                    }
                    catch (Exception ex)
                    {
                        if (_configuration.Log.Equals(bool.TrueString))
                        {
                            string message = $@"Request.Form getter called, Method :{MethodBase.GetCurrentMethod().Name}, Requested Page: {request.Url}";
                            _logger.Error(message, ex);
                        }

                        throw;
                    }


                    if (!string.IsNullOrEmpty(formPostValues) && _regexHelper.ExecFor(_xssDetectionRegex, formPostValues))
                    {
                        result.IsValid = false;
                        result.DiseasedRequestPart = DiseasedRequestPart.Form;
                    }
                }
            }

            return result;
        }
    }
}