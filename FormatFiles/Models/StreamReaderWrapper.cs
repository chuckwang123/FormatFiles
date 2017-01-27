using System.IO;
using FormatFiles.Interfaces;

namespace FormatFiles.Models
{
    public class StreamReaderWrapper : IStreamReader, IFileStream
    {
        private StreamReader _streamReader;
        private readonly string path;

        public StreamReaderWrapper(string _path)
        {
            path = _path;
        }
        public StreamReader SetupStreamReaderWrapper()
        {
            _streamReader = new StreamReader(OpenFile(), true);

            return _streamReader;
        }

        public string ReadLine()
        {
            return _streamReader.ReadLine(); 
        }

        public string ReadtoEnd()
        {
            using (_streamReader = new StreamReader(OpenFile(), true))
            {
                _streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
                return _streamReader.ReadToEnd();
            }
                
        }

        public FileStream OpenFile()
        {
            return new FileStream(path, FileMode.Open);
        }

        public void Dispose()
        {
            _streamReader.Dispose();
        }
    }
}
