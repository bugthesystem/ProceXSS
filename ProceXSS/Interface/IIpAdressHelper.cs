using System.Web;

namespace ProceXSS.Interface
{
    public interface IIpAdressHelper
    {
        string GetIpInformation(HttpRequest request);
    }
}