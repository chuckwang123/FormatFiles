using System.IO;

namespace FormatFiles.Interfaces
{
    public interface IStreamReader
    {
        string ReadLine();
        string ReadtoEnd();
        StreamReader SetupStreamReaderWrapper();
        void Dispose();
    }
}
