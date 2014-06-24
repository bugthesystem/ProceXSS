using System.Text.RegularExpressions;
using ProceXSS.Interface;

namespace ProceXSS.Infrastructure
{
    public class RegexProcessor : IRegexProcessor
    {
        public bool IsNumber(string inputvalue)
        {
            Regex isnumber = new Regex("[^0-9]");
            return !isnumber.IsMatch(inputvalue);
        }

        public bool ExecFor(Regex regex, string inputValue)
        {
            return regex.IsMatch(inputValue);
        }

        public string XssPattern
        {
            get
            {
                //Simple xss detection pattern
                return "(javascript[^*(%3a)]*(%3a|:))|(%3C*|<)[^*]?script|(document*(%2e|.))|(setInterval[^*(%28)]*(%28|\\())|(setTimeout[^*(%28)]*(%28|\\())|(alert[^*(%28)]*(%28|\\())|(((\\%3C) <)[^\n]+((\\%3E) >))";
            }
        }
    }
}
