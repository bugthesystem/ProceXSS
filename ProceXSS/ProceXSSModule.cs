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

        private void StartXssDetection(HttpApplication httpApplication)
        {
            IUrlChecker urlChecker = new UrlChecker(Configuration);
            IRegexProcessor regexProcessor = new RegexProcessor();
            IRequestCleaner requestCleaner = new RequestCleaner(new Reflector(), regexProcessor);
            NullLogger nullLogger = new NullLogger();
            IXssDetector xssDetector = new XssDetector(Configuration, regexProcessor,nullLogger);
            IIpAdressHelper ipAdressHelper = new IpAdressHelper();

            IRequestProcessor requestProcessor = new RequestProcessor(httpApplication, Configuration, urlChecker, requestCleaner, xssDetector, ipAdressHelper,nullLogger);

            requestProcessor.ProcessRequest();
        }
    }
}