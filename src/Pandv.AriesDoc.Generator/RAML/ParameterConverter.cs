﻿using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class ParameterConverter : IParameterConverter
    {
        private IDictionary<Type, string> typeMap = new Dictionary<Type, string>();
        private JsonSchemaGenerator schemaGenerator = new JsonSchemaGenerator();

        public ParameterConverter()
        {
            typeMap.Add(typeof(string), "string");
            var integer = "integer";
            typeMap.Add(typeof(int), integer);
            typeMap.Add(typeof(int?), integer);
            typeMap.Add(typeof(long?), integer);
            typeMap.Add(typeof(long), integer);
            typeMap.Add(typeof(short?), integer);
            typeMap.Add(typeof(short), integer);
            var number = "number";
            typeMap.Add(typeof(double), number);
            typeMap.Add(typeof(double?), number);
            typeMap.Add(typeof(float), number);
            typeMap.Add(typeof(float?), number);
            typeMap.Add(typeof(decimal), number);
            typeMap.Add(typeof(decimal?), number);
            var boolean = "boolean";
            typeMap.Add(typeof(bool), boolean);
            typeMap.Add(typeof(bool?), boolean);
            var date = "date";
            typeMap.Add(typeof(DateTime), date);
            typeMap.Add(typeof(DateTime?), date);
        }

        public IRAMLElement Convert(ApiParameterDescription paramter)
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
            if (!typeMap.ContainsKey(type))
            {
                typeMap[type] = "|" + Environment.NewLine + schemaGenerator.Generate(type).ToString();
            }
            return typeMap[type];
        }
    }
}