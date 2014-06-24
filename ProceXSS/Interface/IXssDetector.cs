using System.Web;
using ProceXSS.Struct;

namespace ProceXSS.Interface
{
    public interface IXssDetector
    {
        RequestValidationResult HasXssVulnerability(HttpRequest request);
    }
}