using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class MethodConverter : IMethodConverter
    {
        public IRAMLElement Convert(ApiDescription item)
        {
            return new ArrayElement() { Key = item.HttpMethod.ToLower(), WithKey = true };
        }
    }
}
