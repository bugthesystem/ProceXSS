using ProceXSS.Enums;

namespace ProceXSS.Struct
{
    public struct ValidationResult
    {
        public MaliciousRequestPart MaliciousRequestPart;
        public bool IsValid;
    }
}