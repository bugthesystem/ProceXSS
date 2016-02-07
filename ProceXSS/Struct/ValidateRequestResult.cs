using ProceXSS.Enums;

namespace ProceXSS.Struct
{
    public class ValidateRequestResult
    {
        public DiseasedRequestPart DiseasedRequestPart { get; set; }
        public bool IsValid { get; set; }
    }
}