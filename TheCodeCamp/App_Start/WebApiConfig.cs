using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.Web.Http;
using Microsoft.Web.Http.Routing;
using Microsoft.Web.Http.Versioning;
using Newtonsoft.Json.Serialization;

namespace TheCodeCamp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            AutofacConfig.Register();

            // Add API Versioning.
            config.AddApiVersioning(cfg =>
            {
                #region Diffrent Versioning Methonds.
                /* 
                     -> This is the default versioning. It will take version number as QueryString.
                     * cfg.DefaultApiVersion = new ApiVersion(1, 1);
                     * cfg.AssumeDefaultVersionWhenUnspecified = true;
                     * cfg.ReportApiVersions = true;
                 
                     -> This will look into "X-Version" in header to get the version information.
                     * cfg.ApiVersionReader = new HeaderApiVersionReader("X-Version");
                 
                     -> URL based Vesrioning.
                     -> Here it dosenot read versioning from anywhere we have to mention in each contoller's version in it's RoutePrefix.
                     * cfg.ApiVersionReader = new UrlSegmentApiVersionReader();
                 */
                #endregion;

                //This will take miltiple version names at same time.
                cfg.ApiVersionReader = ApiVersionReader.Combine
                (
                    new HeaderApiVersionReader("X-Version"),
                    new HeaderApiVersionReader("Version"),
                    new QueryStringApiVersionReader("X-Version"),
                    new QueryStringApiVersionReader("Version")
                );
            });

            // Custom Implimentation to convert Camel Case.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            config.MapHttpAttributeRoutes();

            /* 
                -> If we are using versioning in URI then we need to create our apiVersion variable.
                * var contrainResolver = new DefaultInlineConstraintResolver() { ConstraintMap = { ["apiVersion"] = typeof(ApiVersionRouteConstraint) }                };
                * config.MapHttpAttributeRoutes(contrainResolver); 
            */

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
