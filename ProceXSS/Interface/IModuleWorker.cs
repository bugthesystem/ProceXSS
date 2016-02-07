using System.Web;

namespace ProceXSS.Interface
{
    public interface IModuleWorker
    {
        void Attach(HttpApplication httpApplication);
    }
}
