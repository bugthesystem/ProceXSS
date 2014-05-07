namespace ProceXSS.Configuration
{
    public interface IXssConfigurationHandler
    {
        string RedirectUrl { get; }

      
        string IsActive { get; }


        string ControlRegex { get; }


        string Mode { get; }

        string Log { get; }

        UrlExcludeFilterCollection ExcludeList { get; }
    }
}