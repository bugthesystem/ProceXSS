using System.Reflection;

namespace ProceXSS.Helper
{
    internal static class ReflectionHelper
    {
        public static PropertyInfo MakeWritable<T>(T type) where T : class
        {
            PropertyInfo readonlyProperty = type.GetType()
              .GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);

            readonlyProperty.SetValue(type, false, null);

            return readonlyProperty;
        }
    }
}
