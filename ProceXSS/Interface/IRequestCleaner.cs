using System.Collections.Specialized;
using ProceXSS.Configuration;
using ProceXSS.Enums;

namespace ProceXSS.Interface
{
    internal interface IRequestCleaner
    {
        void Clean(NameValueCollection collection, ProceXssConfigurationHandler configurationHandler, EncoderType encoderType = EncoderType.AutoDetect);
    }
}
