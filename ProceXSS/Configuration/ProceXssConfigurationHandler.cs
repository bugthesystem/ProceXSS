using System.Configuration;

namespace ProceXSS.Configuration
{
    public class ProceXssConfigurationHandler : ConfigurationSection
    {
        [ConfigurationProperty("redirectUrl", IsRequired = true)]
        public string RedirectUrl
        {
            get
            {
                return this["redirectUrl"] as string;
            }
        }

        [ConfigurationProperty("isActive", IsRequired = true)]
        public string IsActive
        {
            get
            {
                return this["isActive"] as string;
            }
        }

        [ConfigurationProperty("controlRegex")]
        public string ControlRegex
        {
            get
            {
                string result = string.Empty;

                if (this["controlRegex"] != null)
                {
                    result= this["controlRegex"] as string;
                }

                return result;
            }
        }

        [ConfigurationProperty("mode", IsRequired = true)]
        public string Mode
        {
            get
            {
                return this["mode"] as string;
            }
        }

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
        public ExcludedUrlFilterCollection ExcludeUrls
        {
            get
            {
                return this["excludeUrls"] as ExcludedUrlFilterCollection;
            }
        }

        public static ProceXssConfigurationHandler GetConfig()
        {
            return ConfigurationManager.GetSection("antiXssModuleSettings") as ProceXssConfigurationHandler;
        }
    }

    public class ExcludedUrFilter : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get
            {
                return this["value"] as string;
            }
        }
    }

    public class ExcludedUrlFilterCollection : ConfigurationElementCollection
    {
        public ExcludedUrFilter this[int index]
        {
            get
            {
                return BaseGet(index) as ExcludedUrFilter;
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ExcludedUrFilter();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExcludedUrFilter)element).Name;
        }
    }
}