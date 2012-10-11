using System;
using System.Web;
using ProceXSS.Configuration;
using ProceXSS.Interface;


namespace ProceXSS
{
    public class AntiXSSModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            InitEvents(context);
        }

        private void InitEvents(HttpApplication context)
        {
            if (ConfigurationHandler.IsActive.Equals(bool.TrueString))
            {
                context.BeginRequest += BeginRequest;
            }
        }

        #endregion

        private static readonly ProceXssConfigurationHandler ConfigurationHandler = ProceXssConfigurationHandler.GetConfig();


        #region Event Impl

        private void BeginRequest(object sender, EventArgs e)
        {
            IRequestProcessor requestProcessor = new RequestProcessor(sender as HttpApplication, ConfigurationHandler);
            requestProcessor.ProcessRequest();
        }


        #endregion Event Impl

        #region Private Register Calls

        #endregion Private Register Calls
    }
}