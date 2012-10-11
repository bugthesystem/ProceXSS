using System;
using System.Text.RegularExpressions;
using System.Web;
using ProceXSS.Configuration;
using ProceXSS.Enums;
using ProceXSS.Helper;
using ProceXSS.Struct;

namespace ProceXSS
{
    internal static class XssDetectorHttpRequestExtensions
    {
        private static Regex _potentialXssAttackRegex;

        internal static ValidationResult CheckPotentialXssAttack(this HttpRequest request, ProceXssConfigurationHandler configurationHandler)
        {
            if (string.IsNullOrWhiteSpace(configurationHandler.ControlRegex))
            {
                _potentialXssAttackRegex = new Regex(RegexHelper.POTENTIAL_XSS_ATTACK_EXPRESSION_V3, RegexOptions.IgnoreCase);
            }
            else
            {
                try
                {
                    _potentialXssAttackRegex = new Regex(HttpUtility.HtmlDecode(configurationHandler.ControlRegex), RegexOptions.IgnoreCase);
                }
                catch
                {
                    _potentialXssAttackRegex = new Regex(RegexHelper.POTENTIAL_XSS_ATTACK_EXPRESSION_V3,
                                                        RegexOptions.IgnoreCase);
                }
            }

            ValidationResult result = new ValidationResult
                                             {
                                                 IsValid = true,
                                                 DirtyRequestPart = DirtyRequestPart.None
                                             };

            if (request != null)
            {
                string queryString = request.QueryString.ToString();

                if (!string.IsNullOrEmpty(queryString) && RegexHelper.IsXSSAttcak(_potentialXssAttackRegex, queryString))
                {
                    result.IsValid = false;
                    result.DirtyRequestPart = DirtyRequestPart.QueryString;
                }

                if (request.HttpMethod.Equals("POST", StringComparison.InvariantCultureIgnoreCase))
                {
                    string formPostValues = request.Form.ToString();

                    if (!string.IsNullOrEmpty(formPostValues) && RegexHelper.IsXSSAttcak(_potentialXssAttackRegex, formPostValues))
                    {
                        result.IsValid = false;
                        result.DirtyRequestPart = DirtyRequestPart.Form;
                    }
                }
            }

            return result;
        }
    }
}