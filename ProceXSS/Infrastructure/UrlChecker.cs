using ProceXSS.Configuration;
using ProceXSS.Interface;

namespace ProceXSS.Infrastructure
{
    public class UrlChecker : IUrlChecker
    {
        private readonly IXssConfigurationHandler _moduleConfigurationHandler;

        public UrlChecker(IXssConfigurationHandler moduleConfigurationHandler)
        {
            _moduleConfigurationHandler = moduleConfigurationHandler;
        }

        public bool ExistInExcludeList(string rawUrl)
        {
            //Uri uri = new Uri(rawUrl); /*TODO: Use uri*/

            string url = rawUrl.Split('?')[0];

            bool result = false;

            for (int i = 0; i < _moduleConfigurationHandler.ExcludeList.Count; i++)
            {

                if (_moduleConfigurationHandler.ExcludeList[i].Value != url)
                {
                    continue;
                }

                result = true;
                break;
            }

            return result;
        }
    }
}