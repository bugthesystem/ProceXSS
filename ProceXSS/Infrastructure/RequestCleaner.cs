using System.Collections.Specialized;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.Security.Application;
using ProceXSS.Configuration;
using ProceXSS.Enums;
using ProceXSS.Interface;

namespace ProceXSS.Infrastructure
{
    public sealed class RequestCleaner : IRequestCleaner
    {
        private readonly IReflector _reflector;
        private readonly IRegexProcessor _regexProcessor;
        private Regex _xssDetectRegex;

        public RequestCleaner(IReflector reflector,IRegexProcessor  regexProcessor)
        {
            _reflector = reflector;
            _regexProcessor = regexProcessor;
        }

        public void Clean(NameValueCollection collection, IXssConfigurationHandler configuration, EncoderType encoderType = EncoderType.AutoDetect)
        {
            if (string.IsNullOrWhiteSpace(configuration.ControlRegex))
            {
                _xssDetectRegex = new Regex(_regexProcessor.XssPattern, RegexOptions.IgnoreCase);
            }
            else
            {
                try
                {
                    _xssDetectRegex = new Regex(HttpUtility.HtmlDecode(configuration.ControlRegex),RegexOptions.IgnoreCase);
                }
                catch
                {
                    _xssDetectRegex = new Regex(_regexProcessor.XssPattern,RegexOptions.IgnoreCase);
                }
            }

            PropertyInfo readonlyProperty = _reflector.MakeWritable(collection);

            for (int i = 0; i < collection.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(collection[i]))
                {
                    continue;
                }

                IterateCleanUp(encoderType, collection, i);
            }

            readonlyProperty.SetValue(collection, true, null);
        }

        private void IterateCleanUp(EncoderType encoderType, NameValueCollection collection, int index)
        {
            switch (encoderType)
            {
                case EncoderType.Javascript:
                    {
                        collection[collection.Keys[index]] = Encoder.JavaScriptEncode(collection[index]);
                        break;
                    }
                case EncoderType.HtmlFragment:
                    {
                        collection[collection.Keys[index]] = _regexProcessor.IsNumber(collection[index])
                                                             ? collection[index]
                                                             : Sanitizer.GetSafeHtmlFragment(collection[index]);
                        break;
                    }
                case EncoderType.Html:
                    {
                        collection[collection.Keys[index]] = Encoder.HtmlEncode(collection[index]);
                    }
                    break;
                case EncoderType.HtmlAttribute:
                    {
                        collection[collection.Keys[index]] = Encoder.HtmlAttributeEncode(collection[index]);
                    }
                    break;
                case EncoderType.AutoDetect:
                    {
                        if (_regexProcessor.ExecFor(_xssDetectRegex, collection[index]))
                        {
                            collection[collection.Keys[index]] = Encoder.JavaScriptEncode(collection[index]);
                        }
                        break;
                    }
            }
        }
    }
}
