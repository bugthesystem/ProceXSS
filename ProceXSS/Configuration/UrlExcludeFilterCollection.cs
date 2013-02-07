using System.Configuration;

namespace ProceXSS.Configuration
{
    public class UrlExcludeFilterCollection : ConfigurationElementCollection
    {
        public UrlExcludeFilter this[int index]
        {
            get
            {
                return BaseGet(index) as UrlExcludeFilter;
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
            return new UrlExcludeFilter();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UrlExcludeFilter)element).Name;
        }
    }
}