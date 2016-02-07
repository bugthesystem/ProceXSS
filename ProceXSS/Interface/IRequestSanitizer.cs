using System.Collections.Specialized;
using ProceXSS.Configuration;
using ProceXSS.Enums;

namespace ProceXSS.Interface
{
    public interface IRequestSanitizer
    {
        void Clean(NameValueCollection collection, IXssConfigurationHandler configuration, EncoderType encoderType = EncoderType.AutoDetect);
    }
}
