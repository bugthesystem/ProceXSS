using ProceXSS.Enums;

namespace ProceXSS.Struct
{
    public struct ValidationResult
    {
        public InfectedRequestPart InfectedRequestPart;
        public bool IsValid;
    }
}