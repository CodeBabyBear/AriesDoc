using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;

namespace Pandv.AriesDoc.Generator.RAML
{
    public interface IParameterConverter
    {
        Parameter Convert(ApiParameterDescription paramter);

        Parameter ConvertByType(Type type);
    }
}