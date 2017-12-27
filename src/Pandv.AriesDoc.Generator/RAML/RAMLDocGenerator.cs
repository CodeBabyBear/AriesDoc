using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class RAMLDocGenerator : IDocGenerator
    {
        private readonly IApiDescriptionGroupCollectionProvider apiDescription;

        public RAMLDocGenerator(IApiDescriptionGroupCollectionProvider apiDescription)
        {
            this.apiDescription = apiDescription;
        }

        public IEnumerable<IDocument> Generate()
        {
            foreach (var group in apiDescription.ApiDescriptionGroups.Items)
            {
                var doc = new RAMLDocument
                {
                    Title = group.GroupName
                };

                foreach (var item in group.Items)
                {
                    var resource = doc.Resources.TryGetElement<Resource>(item.RelativePath);
                    if (resource == null)
                    {
                        resource = new Resource() { Key = item.RelativePath };
                        doc.Resources.AddElement(resource);
                    }
                }

                yield return doc;
            } 
        }
    }
}