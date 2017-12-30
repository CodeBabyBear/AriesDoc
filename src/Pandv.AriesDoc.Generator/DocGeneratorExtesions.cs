using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pandv.AriesDoc.Generator.RAML;
using System;
using System.Reflection;

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

        public static IServiceCollection AddRAMLDocGenerator(this IServiceCollection services)
        {
            return services.AddApiExplorer()
                .AddTransient<IMethodConverter, MethodConverter>()
                .AddTransient<IParameterConverter, ParameterConverter>()
                .AddTransient<IDocGenerator, RAMLDocGenerator>();
        }

        public static bool IsNullable(this Type type)
        {
            return type == typeof(string) || Nullable.GetUnderlyingType(type) != null ||
                   (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
    }
}