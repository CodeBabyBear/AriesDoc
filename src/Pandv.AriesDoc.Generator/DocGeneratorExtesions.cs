using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Pandv.AriesDoc.Generator
{
    public static class DocGeneratorExtesions
    {
        public static IServiceCollection AddApiExplorer(this IServiceCollection services)
        {
            services.TryAddSingleton<IApiDescriptionGroupCollectionProvider, ApiDescriptionGroupCollectionProvider>();
            services.TryAddEnumerable(ServiceDescriptor.Transient<IApiDescriptionProvider, DefaultApiDescriptionProvider>());
            return services;
        }

        public static MvcOptions SetApiExplorerVisible(this MvcOptions options)
        {
            options.Conventions.Add(new ApiExplorerConvention());
            return options;
        }
    }
}