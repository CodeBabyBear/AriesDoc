using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class ParameterConverterV10 : ParameterConverterV08
    {
        protected IDictionary<Type, ObjectType> ramlTypes = new Dictionary<Type, ObjectType>();

        public ParameterConverterV10() : base()
        {
            typeMap.Add(typeof(object), "object");
        }

        public override string GenerateSchema(Type type)
        {
            ObjectType ot = null;
            if (IsDictionary(type))
            {
                var et = type.GetGenericArguments()[1];
                ot = new ObjectType()
                {
                    Key = et.Name + "Map",
                    Type = ConvertParamterType(et)
                };
            }
            else if (IsArrayOrEnumerable(type))
            {
                var et = GetElementType(type);
                ot = new ObjectType()
                {
                    Type = ConvertParamterType(et)
                };
                ot.Key = ot.Type + "[]";
            }
            else
            {
                ot = GetObject(type);
            }

            if (ot != null)
            {
                ramlTypes.Add(type, ot);
                ot.Key = ot.Key.Replace("`", string.Empty);
                return ot.Key;
            }
            else
            {
                return string.Empty;
            }
        }

        private ObjectType GetObject(Type type)
        {
            var ot = new ObjectType() { Key = type.Name };

            if (type.GetTypeInfo().BaseType != null)
            {
                ot.Type = ConvertParamterType(type.GetTypeInfo().BaseType);
            }

            foreach (var item in GetClassProperties(type))
            {
                var p = new PropertyType()
                {
                    Type = ConvertParamterType(item.PropertyType),
                    Key = item.Name,
                    //Required = 
                };
            }
            return ot;
        }

        private static IEnumerable<PropertyInfo> GetClassProperties(Type type)
        {
            var properties = type.GetProperties().Where(p => p.CanWrite);
            if (type.GetTypeInfo().BaseType != null && type.GetTypeInfo().BaseType != typeof(Object))
            {
                var parentProperties = type.GetTypeInfo().BaseType.GetProperties().Where(p => p.CanWrite);
                properties = properties.Where(p => parentProperties.All(x => x.Name != p.Name));
            }
            return properties;
        }

        private bool IsDictionary(Type type)
        {
            return type.GetTypeInfo().IsGenericType && (type.GetGenericTypeDefinition() == typeof(Dictionary<,>) || type.GetGenericTypeDefinition() == typeof(IDictionary<,>));
        }

        private bool IsArrayOrEnumerable(Type type)
        {
            return type.IsArray || (type.GetTypeInfo().IsGenericType &&
                                    (type.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                                     || type.GetGenericTypeDefinition() == typeof(ICollection<>)
                                     || type.GetGenericTypeDefinition() == typeof(Collection<>)
                                     || type.GetGenericTypeDefinition() == typeof(IList<>)
                                     || type.GetGenericTypeDefinition() == typeof(List<>)
                                        )
                );
        }

        private Type GetElementType(Type type)
        {
            return type.GetElementType() ?? type.GetGenericArguments()[0];
        }

        internal void ClearOtherTypes()
        {
            foreach (var item in ramlTypes.Keys)
            {
                typeMap.Remove(item);
            }
            ramlTypes.Clear();
        }

        internal IEnumerable<ObjectType> GetOtherTypes()
        {
            return ramlTypes.Values;
        }
    }
}