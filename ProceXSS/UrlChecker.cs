using ProceXSS.Configuration;
using ProceXSS.Interface;

namespace ProceXSS
{
    public class UrlChecker : IUrlChecker
    {
        private readonly ProceXssConfigurationHandler _moduleConfigurationHandler;

        public UrlChecker(ProceXssConfigurationHandler moduleConfigurationHandler)
        {
            _moduleConfigurationHandler = moduleConfigurationHandler;
        }

        public bool ExistInExcludeList(string rawUrl)
        {
            //Uri uri = new Uri(rawUrl); /*TODO: Use uri*/

            string url = rawUrl.Split('?')[0];

            bool result = false;

            for (int i = 0; i < _moduleConfigurationHandler.Exclude.Count; i++)
            {

                if (_moduleConfigurationHandler.Exclude[i].Value != url)
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