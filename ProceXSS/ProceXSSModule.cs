using System;
using System.Web;
using ProceXSS.Configuration;
using ProceXSS.Infrastructure;
using ProceXSS.Interface;
using ProceXSS.Log;

namespace ProceXSS
{
    public class ProceXSSModule : IHttpModule
    {
        private static ILogger _logger;
        private static readonly IXssConfigurationHandler Configuration = XssConfigurationHandler.GetConfig();

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            InitEvents(context);
        }

        private void InitEvents(HttpApplication context)
        {
            if (Configuration.IsActive.Equals(bool.TrueString))
            {
                context.BeginRequest += BeginRequest;
            }
        }

        private void BeginRequest(object sender, EventArgs e)
        {
            StartXssDetection(sender as HttpApplication);
        }

        private void StartXssDetection(HttpApplication application)
        {
            IUrlChecker urlChecker = new UrlChecker(Configuration);
            IRegexHelper regexHelper = new RegexHelper();
            IRequestSanitizer requestSanitizer = new RequestSanitizer(new ReflectionHelper(), regexHelper);
            ILogger nullLogger = _logger ?? (_logger = new NullLogger());
            IXssGuard xssGuard = new XssGuard(Configuration, regexHelper, nullLogger);
            IIpAdressHelper ipAdressHelper = new IpAdressHelper();

            IModuleWorker moduleWorker = new ModuleWorker(Configuration, urlChecker, requestSanitizer, xssGuard, ipAdressHelper, nullLogger);
            moduleWorker.Attach(application);
        }

        public static void SetLogger(ILogger logger)
        {
            _logger = logger;
        }
    }
}