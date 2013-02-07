using System;
using System.Web;
using ProceXSS.Configuration;
using ProceXSS.Interface;

namespace ProceXSS
{
    public class AntiXSSModule : IHttpModule
    {
        private static readonly ProceXssConfigurationHandler ModuleConfigurationHandler = ProceXssConfigurationHandler.GetConfig();

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            InitEvents(context);
        }

        private void InitEvents(HttpApplication context)
        {
            if (ModuleConfigurationHandler.IsActive.Equals(bool.TrueString))
            {
                context.BeginRequest += BeginRequest;
            }
        }

        private void BeginRequest(object sender, EventArgs e)
        {
            RegisterBeginRequestEventHandler(sender);
        }

        private void RegisterBeginRequestEventHandler(object sender)
        {
            IUrlChecker urlChecker=new UrlChecker(ModuleConfigurationHandler);
            IRequestProcessor requestProcessor = new RequestProcessor(sender as HttpApplication, ModuleConfigurationHandler,urlChecker);

            requestProcessor.ProcessRequest();
        }
    }
}