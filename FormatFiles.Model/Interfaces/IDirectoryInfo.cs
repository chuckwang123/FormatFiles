using System.IO;

namespace FormatFiles.Model.Interfaces
{
    public interface IDirectoryInfo
    {
        DirectoryInfo GetParent(string path);
        string[] GetFiles(string path);
        string GetCurrentDirectory();
    }
}
