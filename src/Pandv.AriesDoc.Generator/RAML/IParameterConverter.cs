using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;

namespace Pandv.AriesDoc.Generator.RAML
{
    public interface IParameterConverter
    {
        IRAMLElement Convert(ApiParameterDescription paramter);

        IRAMLElement ConvertByType(Type type);
    }
}