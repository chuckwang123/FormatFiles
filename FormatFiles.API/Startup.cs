using System.Web.Http;
using Microsoft.Owin.Cors;
using Owin;

namespace FormatFiles.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            httpConfiguration.EnsureInitialized();
            
            app.UseCors(CorsOptions.AllowAll)
               .UseWebApi(httpConfiguration);
        }
    }
}
