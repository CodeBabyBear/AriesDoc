using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Pandv.AriesDoc.Generator.RAML;

namespace Pandv.AriesDoc.Generator
{
    public interface IComments
    {
        void SetCommentToMethod(ApiDescription api, Method method);
        void SetCommentToUriParameters(ArrayElement uriParameters, ApiDescription item);
    }
}
