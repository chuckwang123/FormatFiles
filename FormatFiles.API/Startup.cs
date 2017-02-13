using System.Web.Http;
using Microsoft.Owin.Cors;
using Owin;
using Swashbuckle.Application;

namespace FormatFiles.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            httpConfiguration.EnsureInitialized();
            httpConfiguration.EnableSwagger(x=>x.SingleApiVersion("v1","Format Files API"))
                .EnableSwaggerUi();
            app.UseCors(CorsOptions.AllowAll)
               .UseWebApi(httpConfiguration);
        }
    }
}
