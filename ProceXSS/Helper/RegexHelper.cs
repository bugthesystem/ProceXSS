using System.Text.RegularExpressions;

namespace ProceXSS.Helper
{
    internal static class RegexHelper
    {
        public const string POTENTIAL_XSS_ATTACK_EXPRESSION_V3 = "(javascript[^*(%3a)]*(%3a|:))|(%3C*|<)[^*]?script|(document*(%2e|.))|(setInterval[^*(%28)]*(%28|\\())|(setTimeout[^*(%28)]*(%28|\\())|(alert[^*(%28)]*(%28|\\())|(((\\%3C) <)[^\n]+((\\%3E) >))";


        public static bool IsXSSAttcak(Regex regex, string inputValue)
        {
            return regex.IsMatch(inputValue);
        }
    }
}
