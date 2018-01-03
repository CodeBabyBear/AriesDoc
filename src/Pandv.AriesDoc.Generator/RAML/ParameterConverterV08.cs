using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class ParameterConverterV08 : IParameterConverter
    {
        protected IDictionary<string, string> typeMap = new Dictionary<string, string>();
        private JsonSchemaGenerator schemaGenerator = new JsonSchemaGenerator();

        public ParameterConverterV08()
        {
            var str = "string";
            typeMap.Add(typeof(string).FullName, str);
            typeMap.Add(typeof(Guid).FullName, str);
            typeMap.Add(typeof(Guid?).FullName, str);
            var integer = "integer";
            typeMap.Add(typeof(int).FullName, integer);
            typeMap.Add(typeof(int?).FullName, integer);
            typeMap.Add(typeof(long?).FullName, integer);
            typeMap.Add(typeof(long).FullName, integer);
            typeMap.Add(typeof(short?).FullName, integer);
            typeMap.Add(typeof(short).FullName, integer);
            var number = "number";
            typeMap.Add(typeof(double).FullName, number);
            typeMap.Add(typeof(double?).FullName, number);
            typeMap.Add(typeof(float).FullName, number);
            typeMap.Add(typeof(float?).FullName, number);
            typeMap.Add(typeof(decimal).FullName, number);
            typeMap.Add(typeof(decimal?).FullName, number);
            var boolean = "boolean";
            typeMap.Add(typeof(bool).FullName, boolean);
            typeMap.Add(typeof(bool?).FullName, boolean);
            var date = "date";
            typeMap.Add(typeof(DateTime).FullName, date);
            typeMap.Add(typeof(DateTime?).FullName, date);
        }

        public Parameter Convert(ApiParameterDescription paramter)
        {
            return new Parameter()
            {
                Key = paramter.Name,
                ParameterType = ConvertParamterType(paramter.Type),
                Required = IsRequired(paramter).ToString().ToLower(),
                Default = GetDefaultValue(paramter)
            };
        }

        private string GetDefaultValue(ApiParameterDescription paramter)
        {
            return paramter.ParameterDescriptor is ControllerParameterDescriptor apiParam && apiParam.ParameterInfo.HasDefaultValue
                ? JsonConvert.SerializeObject(apiParam.ParameterInfo.DefaultValue)
                : string.Empty;
        }

        private bool IsRequired(ApiParameterDescription paramter)
        {
            var apiParam = paramter.ParameterDescriptor as ControllerParameterDescriptor;
            return apiParam == null ? !paramter.Type.IsNullable()
                : (!apiParam.ParameterInfo?.IsOptional).GetValueOrDefault();
        }

        public string ConvertParamterType(Type type)
        {
            if (!typeMap.ContainsKey(type.FullName))
            {
                typeMap[type.FullName] = GenerateSchema(type);
            }
            return typeMap[type.FullName];
        }

        public virtual string GenerateSchema(Type type)
        {
            return schemaGenerator.Generate(type).ToString();
        }

        public Parameter ConvertByType(Type type)
        {
            var res = new Parameter()
            {
                Key = string.Empty,
                ParameterType = ConvertParamterType(type)
            };
            res.WithKey = false;
            return res;
        }
    }
}