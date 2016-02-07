using System.Reflection;

namespace ProceXSS.Interface
{
    public interface IReflectortionHelper
    {
        PropertyInfo MakeWritable<T>(T type) where T : class;
    }
}