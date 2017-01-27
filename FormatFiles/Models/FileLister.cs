using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace FormatFiles.Models
{
    public static class FileLister
    {
        public static IEnumerable<string> ListFiles(IHostingEnvironment _hostingEnvironment)
        {
            var contentRootPath = _hostingEnvironment.ContentRootPath;
            const string dataFolder = "DataFolder";
            var directoryInfo = Directory.GetParent(contentRootPath);
            if (directoryInfo == null)
            {
                return null;
            }

            //get the folder name
            var folderPath = directoryInfo.FullName;
            var filePath = Path.Combine(folderPath, dataFolder);

            //List the file under the folder
            return Directory.GetFiles(filePath);
        }
    }
}
