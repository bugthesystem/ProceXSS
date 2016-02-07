using System.Reflection;
using ProceXSS.Interface;

namespace ProceXSS.Infrastructure
{
    public class ReflectortionHelper : IReflectortionHelper
    {
        public PropertyInfo MakeWritable<T>(T type) where T : class
        {
            PropertyInfo readonlyProperty = type.GetType().GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);

            readonlyProperty.SetValue(type, false, null);

            return readonlyProperty;
        }
    }
}
