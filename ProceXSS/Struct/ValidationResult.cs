using ProceXSS.Enums;

namespace ProceXSS.Struct
{
    internal struct ValidationResult
    {
        public DirtyRequestPart DirtyRequestPart;
        public bool IsValid;
    }
}