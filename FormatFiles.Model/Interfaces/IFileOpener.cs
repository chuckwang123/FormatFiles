using System.IO;

namespace FormatFiles.Console.Interfaces
{
    public interface IFileOpener
    {
        TextReader OpenFile(string filePath);
    }
}
