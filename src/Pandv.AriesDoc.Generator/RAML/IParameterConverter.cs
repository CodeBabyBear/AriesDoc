using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Pandv.AriesDoc.Generator.RAML
{
    public interface IParameterConverter
    {
        IRAMLElement Convert(ApiParameterDescription paramter);
    }
}