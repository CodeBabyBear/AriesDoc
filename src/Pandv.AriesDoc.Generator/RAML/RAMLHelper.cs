using System;
using System.Reflection;

namespace Pandv.AriesDoc.Generator.RAML
{
    public static class RAMLHelper
    {
        public static string Indent(this string str, int depth)
        {
            var indent = depth <= 0 ? string.Empty : new string(' ', depth * 2);
            return indent + str;
        }

        public static bool IsNullable(this Type type)
        {
            return type == typeof(string) || Nullable.GetUnderlyingType(type) != null ||
                   (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
    }
}