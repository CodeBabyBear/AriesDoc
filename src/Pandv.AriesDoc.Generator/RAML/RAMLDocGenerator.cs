using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class RAMLDocGenerator : IDocGenerator
    {
        private readonly IApiDescriptionGroupCollectionProvider apiDescription;
        private readonly IParameterConverter parameterConverter;
        private readonly IMethodConverter methodConverter;

        public RAMLDocGenerator(IApiDescriptionGroupCollectionProvider apiDescription, IParameterConverter parameterConverter, IMethodConverter methodConverter)
        {
            this.apiDescription = apiDescription;
            this.parameterConverter = parameterConverter;
            this.methodConverter = methodConverter;
        }

        public IEnumerable<IDocument> Generate()
        {
            foreach (var group in apiDescription.ApiDescriptionGroups.Items)
            {
                yield return GenerateDocument(group);
            }
        }

        private RAMLDocument GenerateDocument(ApiDescriptionGroup group)
        {
            var doc = new RAMLDocument
            {
                Title = group.GroupName
            };

            foreach (var item in group.Items)
            {
                var key = "/" + item.RelativePath;
                var resource = doc.GetOrAddResource(key);
                SetUriPatameters(item, resource);
                SetMethod(item, resource);
            }

            return doc;
        }

        private void SetMethod(ApiDescription item, Resource resource)
        {
            resource.AddElement(methodConverter.Convert(item));
        }

        private void SetUriPatameters(ApiDescription item, Resource resource)
        {
            if (!resource.HasUriParameters)
            {
                foreach (var paramter in item.ParameterDescriptions.Where(i => i.Source == BindingSource.Path))
                {
                    resource.UriParameters.AddElement(parameterConverter.Convert(paramter));
                }
            }
        }
    }
}