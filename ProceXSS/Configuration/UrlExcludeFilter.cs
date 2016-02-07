using System.Configuration;

namespace ProceXSS.Configuration
{
    public class UrlExcludeFilter : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name => this["name"] as string;

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value => this["value"] as string;
    }
}