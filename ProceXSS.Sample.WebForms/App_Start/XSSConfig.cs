using System.Web;
using ProceXSS.Sample.WebForms;
using ProceXSS.Sample.WebForms.Context;

[assembly: PreApplicationStartMethod(typeof(XSSConfig), "Start")]
namespace ProceXSS.Sample.WebForms
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