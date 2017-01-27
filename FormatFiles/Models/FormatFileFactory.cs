using FormatFiles.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace FormatFiles.Models
{
    public class FormatFileFactory : IFactory
    {
        public IHostingEnvironment HostingEnvironment => new CustomHostingEnv();
    }
}
