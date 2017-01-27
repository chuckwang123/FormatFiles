using FormatFiles.Model.Interfaces;

namespace FormatFiles.Model.Models
{
    public class FormatFileFactory : IFactory
    {
        public IStreamReader CustomStreamReader => new StreamReaderWrapper();
        public IFileStream CustomeFileStream => new StreamReaderWrapper();
        public IHostingEnvironment CustomHostingEnvironment => new CustomHostingEnvironment();
        public IDirectoryInfo CustomDirectoryInfo => new CustomDirectoryInfo();
    }
}
