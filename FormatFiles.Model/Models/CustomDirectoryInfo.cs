using System.IO;
using FormatFiles.Model.Interfaces;

namespace FormatFiles.Model.Models
{
    public class CustomDirectoryInfo : IDirectoryInfo
    {
        public DirectoryInfo GetParent(string path)
        {
            return Directory.GetParent(path);
        }

        public string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        public string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
