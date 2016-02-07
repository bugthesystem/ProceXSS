using System.Configuration;

namespace ProceXSS.Configuration
{
    public class XssConfigurationHandler : ConfigurationSection, IXssConfigurationHandler
    {
        [ConfigurationProperty("redirectUrl", IsRequired = true)]
        public string RedirectUrl => this["redirectUrl"] as string;

        [ConfigurationProperty("isActive", IsRequired = true)]
        public string IsActive => this["isActive"] as string;

        [ConfigurationProperty("controlRegex")]
        public string ControlRegex
        {
            get
            {
                string result = string.Empty;

                if (this["controlRegex"] != null)
                {
                    result = this["controlRegex"] as string;
                }

                return result;
            }
        }

        [ConfigurationProperty("mode", IsRequired = true)]
        public string Mode => this["mode"] as string;

        [ConfigurationProperty("log")]
        public string Log
        {
            get
            {
                string configValue = this["log"] as string;

                if (!string.IsNullOrEmpty(configValue))
                {
                    if (configValue.Equals(bool.TrueString))
                        return bool.TrueString;
                }

                return bool.FalseString;
            }
        }

        [ConfigurationProperty("excludeUrls")]
        public UrlExcludeFilterCollection ExcludeList => this["excludeUrls"] as UrlExcludeFilterCollection;

        public static XssConfigurationHandler GetConfig()
        {
            return ConfigurationManager.GetSection("antiXssModuleSettings") as XssConfigurationHandler;
        }
    }
}
