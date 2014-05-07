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
    public class XssDetector : IXssDetector
    {
        private readonly IXssConfigurationHandler _configuration;
        private readonly IRegexProcessor _regexProcessor;
        private Regex _xssDetectRegex;

        public XssDetector(IXssConfigurationHandler configuration, IRegexProcessor regexProcessor)
        {
            _configuration = configuration;
            _regexProcessor = regexProcessor;
        }

        public XSSValidationResult HasXssVulnerability(HttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(_configuration.ControlRegex))
            {
                _xssDetectRegex = new Regex(_regexProcessor.XssPattern, RegexOptions.IgnoreCase);
            }
            else
            {
                try
                {
                    _xssDetectRegex = new Regex(HttpUtility.HtmlDecode(_configuration.ControlRegex), RegexOptions.IgnoreCase);
                }
                catch
                {
                    _xssDetectRegex = new Regex(_regexProcessor.XssPattern, RegexOptions.IgnoreCase);
                }
            }

            XSSValidationResult result = new XSSValidationResult
            {
                IsValid = true,
                DiseasedRequestPart = DiseasedRequestPart.None
            };

            if (request != null)
            {
                string queryString = request.QueryString.ToString();

                if (!string.IsNullOrEmpty(queryString) &&
                    _regexProcessor.ExecFor(_xssDetectRegex, queryString))
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
                            string message = string.Format(@"Request.Form getter called, Method :{0}, Requested Page: {1}", MethodBase.GetCurrentMethod().Name, request.Url);
                            InternalLogManager.Instance.ErrorException(message, ex);
                        }

                        throw;
                    }


                    if (!string.IsNullOrEmpty(formPostValues) && _regexProcessor.ExecFor(_xssDetectRegex, formPostValues))
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