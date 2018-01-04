using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class MethodConverterV10 : MethodConverterV08
    {
        public MethodConverterV10(IParameterConverter parameterConverter) : base(parameterConverter)
        {
        }

        protected override void SetResponse(ApiResponseType responseType, Response response, string format)
        {
            var mimeType = new MimeTypeV10() { Key = format };
            mimeType.Type.Value = parameterConverter.ConvertByType(responseType.Type).ParameterType;
            response.Body.AddElement(mimeType);
        }

        protected override void SetBody(Method method, string format, Parameter[] parameters)
        {
            var mimeType = new MimeTypeV10() { Key = format };
            if (parameters.Length == 1)
            {
                mimeType.Type.Value = parameters[0].ParameterType;
            }
            else
            {
                var ot = new ObjectType() { Key = Guid.NewGuid().ToString() + "Request" };
                foreach (var item in parameters)
                {
                    var p = new PropertyType()
                    {
                        Type = item.ParameterType,
                        Key = item.Key,
                        //Required =
                    };
                    ot.AddPropertyType(p);
                }
                (parameterConverter as ParameterConverterV10)?.AddType(ot.Key, ot);
                mimeType.Type.Value = ot.Key;
            }
            method.Body.AddElement(mimeType);
        }
    }
}