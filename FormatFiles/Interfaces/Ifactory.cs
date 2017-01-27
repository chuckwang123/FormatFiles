using Microsoft.AspNetCore.Hosting;

namespace FormatFiles.Interfaces
{
    public interface IFactory
    {
        IHostingEnvironment HostingEnvironment { get; }
    }
}
