using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class ParameterConverterV10 : ParameterConverterV08
    {
        protected IDictionary<string, ObjectType> ramlTypes = new Dictionary<string, ObjectType>();

        public ParameterConverterV10() : base()
        {
            var date = "datetime";
            typeMap[typeof(DateTime).FullName] = date;
            typeMap[typeof(DateTime?).FullName] = date;
            typeMap.Add(typeof(object).FullName, "object");
        }

        public override string GenerateSchema(Type type)
        {
            ObjectType ot = null;
            if (IsDictionary(type))
            {
                ot = GetMap(type);
            }
            else if (IsArrayOrEnumerable(type))
            {
                ot = GetArray(type);
            }
            else if (type.GetTypeInfo().IsEnum)
            {
                ot = GetEnum(type);
            }
            else
            {
                ot = GetObject(type);
            }

            if (ot != null)
            {
                return ot.Key;
            }
            else
            {
                return string.Empty;
            }
        }

        private ObjectType GetMap(Type type)
        {
            ObjectType ot;
            var k = ConvertParamterType(type.GetGenericArguments()[0]);
            var v = ConvertParamterType(type.GetGenericArguments()[1]);
            ot = new ObjectType()
            {
                Key = $"{k}_{v}_Map",
                Type = $"object"
            };
            ot.AddPropertyType(new PropertyType()
            {
                Type = k,
                Key = "Key",
            });
            ot.AddPropertyType(new PropertyType()
            {
                Type = v,
                Key = "Value",
            });
            AddType(type, ot);
            return ot;
        }

        private ObjectType GetArray(Type type)
        {
            ObjectType ot;
            var et = ConvertParamterType(GetElementType(type));
            ot = new ObjectType()
            {
                Type = et + "[]",
                Key = et + "_Array"
            };
            AddType(type, ot);
            return ot;
        }

        private ObjectType GetEnum(Type type)
        {
            ObjectType ot = new ObjectType()
            {
                Key = type.Name + "Enum",
                Type = "string"
            };
            foreach (var item in type.GetEnumNames())
            {
                ot.Enum.AddElement(new StringElement() { Key = $"- {item}", Value = string.Empty });
            }

            AddType(type, ot);
            return ot;
        }

        private void AddType(Type type, ObjectType ot)
        {
            AddType(type.FullName, ot);
        }

        internal void AddType(string key, ObjectType ot)
        {
            ramlTypes.Add(key, ot);
            ot.Key = ot.Key.Replace("`", string.Empty);
            typeMap[key] = ot.Key;
        }

        private ObjectType GetObject(Type type)
        {
            var ot = new ObjectType() { Key = type.Name };
            AddType(type, ot);
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
                ot.AddPropertyType(p);
            }
            return ot;
        }

        private static IEnumerable<PropertyInfo> GetClassProperties(Type type)
        {
            var properties = type.GetProperties().Where(p => p.CanWrite);
            if (type.GetTypeInfo().BaseType != null && type.GetTypeInfo().BaseType != typeof(object))
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