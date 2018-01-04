using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Linq;

namespace Example
{
    public static class MvcOptionsExtensions
    {
        public static MvcOptions UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            opts.Conventions.Insert(0, new RouteConvention(routeAttribute));
            return opts;
        }

        public static MvcOptions UseCentralRoutePrefix(this MvcOptions opts, string prefix)
        {
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                opts.Conventions.Insert(0, new RouteConvention(new RouteAttribute(prefix)));
            }
            return opts;
        }
    }

    public class RouteConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _CentralPrefix;

        public RouteConvention(IRouteTemplateProvider routeTemplateProvider)
        {
            _CentralPrefix = new AttributeRouteModel(routeTemplateProvider);
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var selector in application.Controllers.SelectMany(i => i.Selectors))
            {
                selector.AttributeRouteModel = selector.AttributeRouteModel != null
                     ? AttributeRouteModel.CombineAttributeRouteModel(_CentralPrefix, selector.AttributeRouteModel)
                    : _CentralPrefix;
            }
        }
    }
}