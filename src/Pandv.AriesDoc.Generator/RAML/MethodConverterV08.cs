using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class MethodConverterV08 : IMethodConverter
    {
        private readonly IParameterConverter parameterConverter;

        public MethodConverterV08(IParameterConverter parameterConverter)
        {
            this.parameterConverter = parameterConverter;
        }

        public IRAMLElement Convert(ApiDescription api)
        {
            var method = new Method() { Key = api.HttpMethod.ToLower() };
            SetQueryParameters(method, api);
            SetBody(method, api);
            SetResponses(method, api);
            return method;
        }

        private void SetResponses(Method method, ApiDescription api)
        {
            foreach (var responseType in api.SupportedResponseTypes)
            {
                var response = new Response() { Key = responseType.StatusCode.ToString() };

                foreach (var format in responseType.ApiResponseFormats.Select(i => i.MediaType).Distinct()
                    .Where(i => "application/json".Equals(i, StringComparison.OrdinalIgnoreCase)))
                {
                    var mimeType = new MimeType() { Key = format };
                    mimeType.Schema.AddElement(parameterConverter.ConvertByType(responseType.Type));
                    response.Body.AddElement(mimeType);
                }
                if (response.Body.HasElements)
                    method.Responses.AddElement(response);
            }
        }

        private void SetBody(Method method, ApiDescription api)
        {
            foreach (var format in api.SupportedRequestFormats.Select(i => i.MediaType).Distinct()
                .Where(i => "application/json".Equals(i, StringComparison.OrdinalIgnoreCase)))
            {
                var parameters = api.ParameterDescriptions.Where(i => i.Source == BindingSource.Body)
                    .Select(i => parameterConverter.Convert(i)).ToArray();
                var mimeType = new MimeType() { Key = format };
                foreach (var item in parameters)
                {
                    mimeType.Schema.AddElement(item);
                }
                method.Body.AddElement(mimeType);
            }
        }

        private void SetQueryParameters(Method method, ApiDescription api)
        {
            foreach (var paramter in api.ParameterDescriptions.Where(i => i.Source == BindingSource.Query || i.Source == BindingSource.ModelBinding))
            {
                method.QueryParameters.AddElement(parameterConverter.Convert(paramter));
            }
        }
    }
}