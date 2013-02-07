using System.Configuration;

namespace ProceXSS.Configuration
{
    public class UrlExcludeFilter : ConfigurationElement
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
}