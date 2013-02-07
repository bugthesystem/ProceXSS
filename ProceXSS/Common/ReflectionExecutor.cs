using System.Reflection;

namespace ProceXSS.Common
{
    public static class ReflectionExecutor
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
