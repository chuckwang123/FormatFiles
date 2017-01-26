using System.IO;

namespace FormatFiles.Model.Models
{
    public static class FileLister
    {
        public static string[] ListFiles()
        {
            const string dataFolder = "DataFolder";
            var projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;
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
            return Directory.GetFiles(filePath);
        }
    }
}
