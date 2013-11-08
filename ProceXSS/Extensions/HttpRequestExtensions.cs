using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using ProceXSS.Common;
using ProceXSS.Configuration;
using ProceXSS.Enums;
using ProceXSS.Log;
using ProceXSS.Struct;

namespace ProceXSS.Extensions
{
    public static class HttpRequestExtensions
    {
        private static Regex POTENTIAL_XSS_ATTACK_REGEX;

        public static ValidationResult XssAttackExist(this HttpRequest request, ProceXssConfigurationHandler configurationHandler)
        {
            if (string.IsNullOrWhiteSpace(configurationHandler.ControlRegex))
            {
                POTENTIAL_XSS_ATTACK_REGEX = new Regex(RegexExecutor.POTENTIAL_XSS_ATTACK_EXPRESSION_V3, RegexOptions.IgnoreCase);
            }
            else
            {
                try
                {
                    POTENTIAL_XSS_ATTACK_REGEX = new Regex(HttpUtility.HtmlDecode(configurationHandler.ControlRegex), RegexOptions.IgnoreCase);
                }
                catch
                {
                    POTENTIAL_XSS_ATTACK_REGEX = new Regex(RegexExecutor.POTENTIAL_XSS_ATTACK_EXPRESSION_V3,
                                                        RegexOptions.IgnoreCase);
                }
            }

            ValidationResult result = new ValidationResult
                                             {
                                                 IsValid = true,
                                                 MaliciousRequestPart = MaliciousRequestPart.None
                                             };

            if (request != null)
            {
                string queryString = request.QueryString.ToString();

                if (!string.IsNullOrEmpty(queryString) &&
                    RegexExecutor.IsXSSAttcak(POTENTIAL_XSS_ATTACK_REGEX, queryString))
                {
                    result.IsValid = false;
                    result.MaliciousRequestPart = MaliciousRequestPart.QueryString;
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
                        if (configurationHandler.Log.Equals(bool.TrueString))
                        {
                            string message = string.Format(@"Request.Form getter called, Method :{0},
                                                            Request Page: {1}", MethodBase.GetCurrentMethod().Name,
                                                           request.Url);
                            XssLogger.Instance.ErrorException(message, ex);
                        }

                        throw;
                    }


                    if (!string.IsNullOrEmpty(formPostValues) &&
                        RegexExecutor.IsXSSAttcak(POTENTIAL_XSS_ATTACK_REGEX, formPostValues))
                    {
                        result.IsValid = false;
                        result.MaliciousRequestPart = MaliciousRequestPart.Form;
                    }
                }
            }

            return result;
        }

        public static string GetIPInformation(this HttpRequest request)
        {
            string ip = request.ServerVariables["HTTP_CLIENT_IP"];
            string alternateIP = request.ServerVariables["REMOTE_ADDR"];

            string result = (String.IsNullOrEmpty(ip))
                                ? (String.IsNullOrEmpty(alternateIP) ? String.Empty : alternateIP)
                                : ip;

            return result;
        }
    }
}