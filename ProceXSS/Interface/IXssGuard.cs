using System.Web;
using ProceXSS.Struct;

namespace ProceXSS.Interface
{
    public interface IXssGuard
    {
        ValidateRequestResult HasVulnerability(HttpRequest request);
    }
}