using System.Reflection;

namespace ProceXSS.Interface
{
    public interface IReflector
    {
        PropertyInfo MakeWritable<T>(T type) where T : class;
    }
}