using System.Web;
using ProceXSS.Web.App_Start;

[assembly: PreApplicationStartMethod(typeof(XSSConfig), "Start")]
namespace ProceXSS.Web.App_Start {
    public class XSSConfig {
        public static void Start() {
            // Register our module
            Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(ProceXSSModule));
        }
    }
}