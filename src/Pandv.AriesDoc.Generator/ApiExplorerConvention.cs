using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Pandv.AriesDoc.Generator
{
    public class ApiExplorerConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            application.ApiExplorer.IsVisible = true;
        }
    }
}