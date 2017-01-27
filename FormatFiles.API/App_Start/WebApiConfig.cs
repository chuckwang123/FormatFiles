using System.Web.Http;
using FormatFiles.Model.Models;

namespace FormatFiles.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Formatters.Insert(0, new BrowserJsonFormatter(config.Formatters.JsonFormatter.SerializerSettings));
        }
    }
}
