using System.Web;
using ProceXSS.Struct;

namespace ProceXSS.Interface
{
    public interface IXssDetector
    {
        XSSValidationResult HasXssVulnerability(HttpRequest request);
    }
}