using System.Web;
using ProceXSS.Sample.Mvc;
using ProceXSS.Sample.Mvc.Context;

[assembly: PreApplicationStartMethod(typeof(XSSConfig), "Start")]
namespace ProceXSS.Sample.Mvc
{
    public class XSSConfig
    {
        public static void Start()
        {
            ProceXSSModule.SetLogger(new NLogger());
            Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(ProceXSSModule));
        }
    }
}