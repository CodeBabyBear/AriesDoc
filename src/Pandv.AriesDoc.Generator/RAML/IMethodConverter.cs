using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Pandv.AriesDoc.Generator.RAML
{
    public interface IMethodConverter
    {
        IRAMLElement Convert(ApiDescription item);
    }
}