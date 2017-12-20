using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.Text;

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
            throw new NotImplementedException();
        }
    }
}
