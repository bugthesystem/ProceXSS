namespace ProceXSS.Interface
{
    public interface IUrlChecker
    {
        bool ExistInExcludeList(string url);
    }
}