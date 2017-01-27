using System.IO;
using FormatFiles.Model.Interfaces;

namespace FormatFiles.Model.Models
{
    public class FileLister
    {
        private IHostingEnvironment customHostingEnvironment;
        private IDirectoryInfo CustomDirectoryInfo;

        public FileLister(IFactory factory)
        {
            customHostingEnvironment = factory.CustomHostingEnvironment;
            CustomDirectoryInfo = factory.CustomDirectoryInfo;
        }

        public string[] ListFiles()
        {
            const string dataFolder = "DataFolder";
            var projectPath = CustomDirectoryInfo.GetParent(CustomDirectoryInfo.GetCurrentDirectory()).Parent;
            var directoryInfo = projectPath?.Parent;
            if (directoryInfo == null)
            {
                System.Console.WriteLine("The base folder is not exist");
                return null;
            }

            //get the folder name
            var folderPath = directoryInfo.FullName;
            var filePath = Path.Combine(folderPath, dataFolder);
            System.Console.WriteLine($"The folder path is {filePath}");

            //List the file under the folder
            return CustomDirectoryInfo.GetFiles(filePath);
        }

        public string[] ListWebFiles()
        {
            const string dataFolder = "DataFolder";
            var projectPath = customHostingEnvironment.MapPath("~/App_Data");
            var folderPath = CustomDirectoryInfo.GetParent(projectPath).Parent;
            if (folderPath == null)
            {
                System.Console.WriteLine("The base folder is not exist");
                return null;
            }

            //get the folder name
            var filePath = Path.Combine(folderPath.FullName, dataFolder);
            System.Console.WriteLine($"The folder path is {filePath}");

            //List the file under the folder
            return CustomDirectoryInfo.GetFiles(filePath);
        }
    }
}
