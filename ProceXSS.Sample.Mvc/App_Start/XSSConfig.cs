using System.Web;
using ProceXSS.Sample.Mvc;
using ProceXSS.Sample.Mvc.Context;

[assembly: PreApplicationStartMethod(typeof(XssConfig), "Start")]
namespace ProceXSS.Sample.Mvc
{
    public class XssConfig
    {
        public static void Start()
        {
            ProceXSSModule.SetLogger(new NLogger());
            Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(ProceXSSModule));
        }
    }
}