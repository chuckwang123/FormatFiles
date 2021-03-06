﻿using System.IO;

namespace FormatFiles.Model.Interfaces
{
    public interface IStreamReader
    {
        string ReadLine();
        string ReadtoEnd();
        StreamReader SetupStreamReaderWrapper();
        void Dispose();
        void SetPath(string _path);
    }
}
