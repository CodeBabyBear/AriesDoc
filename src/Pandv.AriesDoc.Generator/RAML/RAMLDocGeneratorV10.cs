using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Linq;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class RAMLDocGeneratorV10 : RAMLDocGeneratorV08
    {
        public RAMLDocGeneratorV10(IApiDescriptionGroupCollectionProvider apiDescription, IParameterConverter parameterConverter, IMethodConverter methodConverter) : base(apiDescription, parameterConverter, methodConverter)
        {
        }

        protected override void OtherHandle(RAMLDocument doc)
        {
            doc.RAMLVersion = RAMLDocument.RAMLVersion10;
            var pc = parameterConverter as ParameterConverterV10;
            if (pc == null) return;
            foreach (var item in pc.GetOtherTypes().GroupBy(i=> i.Key)
                                  .Select(g => g.First())
                                  .ToList())
            {
                doc.Types.AddElement(item);
            }
            pc.ClearOtherTypes();
        }
    }
}