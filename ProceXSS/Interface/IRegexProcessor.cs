using System.Text.RegularExpressions;

namespace ProceXSS.Interface
{
    public interface IRegexProcessor
    {
        bool IsNumber(string inputvalue);
        bool ExecFor(Regex regex, string inputValue);
        string XssPattern { get; }
    }
}