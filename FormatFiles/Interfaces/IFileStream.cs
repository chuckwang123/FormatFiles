using System.IO;

namespace FormatFiles.Interfaces
{
    public interface IFileStream
    {
        FileStream OpenFile();
    }
}
