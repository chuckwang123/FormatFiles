using System.IO;

namespace FormatFiles.Interfaces
{
    public interface IFileOpener
    {
        TextReader OpenFile(string filePath);
    }
}
