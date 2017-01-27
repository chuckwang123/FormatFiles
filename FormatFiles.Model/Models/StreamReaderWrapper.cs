using System.IO;
using System.Text;
using FormatFiles.Model.Interfaces;

namespace FormatFiles.Model.Models
{
    public class StreamReaderWrapper : IStreamReader
    {
        private StreamReader _streamReader;

        public StreamReaderWrapper() { }
        public StreamReader SetupStreamReaderWrapper(string path)
        {
            _streamReader = new StreamReader(path, Encoding.UTF8);
            return _streamReader;
        }

        public string ReadLine()
        {
            return _streamReader.ReadLine();
        }

        public string ReadtoEnd()
        {
            _streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            return _streamReader.ReadToEnd();
        }
    }
}
