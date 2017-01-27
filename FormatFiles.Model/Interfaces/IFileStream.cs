using System.IO;

namespace FormatFiles.Model.Interfaces
{
    public interface IFileStream
    {
        FileStream OpenFile();
    }
}
