using System.Reflection;

namespace ProceXSS.Interface
{
    public interface IReflectionHelper
    {
        PropertyInfo MakeWritable<T>(T type) where T : class;
    }
}