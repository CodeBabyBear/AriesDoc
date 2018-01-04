using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pandv.AriesDoc.Generator.RAML;
using System.IO;
using System.Linq;

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

        public static IServiceCollection AddRAMLDocGeneratorV08(this IServiceCollection services)
        {
            return services.AddApiExplorer()
                .AddTransient<IMethodConverter, MethodConverterV08>()
                .AddTransient<IParameterConverter, ParameterConverterV08>()
                .AddTransient<IDocGenerator, RAMLDocGeneratorV08>();
        }

        public static IServiceCollection AddRAMLDocGeneratorV10(this IServiceCollection services)
        {
            return services.AddApiExplorer()
                .AddTransient<IMethodConverter, MethodConverterV10>()
                .AddSingleton<IParameterConverter, ParameterConverterV10>()
                .AddTransient<IDocGenerator, RAMLDocGeneratorV10>();
        }

        public static IWebHost GeneratorDoc(this IWebHost host, string docFolder, string baseUri = "")
        {
            var generator = host.Services.GetRequiredService<IDocGenerator>();
            var index = 0;
            generator.Generate()
                .ToList()
                .ForEach(i =>
                {
                    if (string.IsNullOrWhiteSpace(i.Title))
                    {
                        i.Title = $"API{index++}";
                        i.BaseUri = baseUri;
                    }
                    File.WriteAllText(Path.Combine(docFolder, $"{i.Title}.raml"), i.Serialize());
                });
            return host;
        }
    }
}