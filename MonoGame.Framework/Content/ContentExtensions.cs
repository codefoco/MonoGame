using System;
using System.Reflection;
using System.Linq;

namespace Microsoft.Xna.Framework.Content
{
    internal static class ContentExtensions
    {
        public static ConstructorInfo GetDefaultConstructor(this Type type)
        {
#pragma warning disable IL2070
            var attrs = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            return type.GetConstructor(attrs, null, new Type[0], null);
#pragma warning restore IL2070
        }

        public static PropertyInfo[] GetAllProperties(this Type type)
        {

            // Sometimes, overridden properties of abstract classes can show up even with 
            // BindingFlags.DeclaredOnly is passed to GetProperties. Make sure that
            // all properties in this list are defined in this class by comparing
            // its get method with that of it's base class. If they're the same
            // Then it's an overridden property.
#pragma warning disable IL2070
            const BindingFlags attrs = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            var allProps = type.GetProperties(attrs).ToList();
            var props = allProps.FindAll(p => p.GetGetMethod(true) != null && p.GetGetMethod(true) == p.GetGetMethod(true).GetBaseDefinition()).ToArray();
            return props;
#pragma warning restore IL2070
        }


        public static FieldInfo[] GetAllFields(this Type type)
        {
#pragma warning disable IL2070
            var attrs = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            return type.GetFields(attrs);
#pragma warning restore IL2070
        }

        public static bool IsClass(this Type type)
        {
            return type.IsClass;
        }
    }
}
