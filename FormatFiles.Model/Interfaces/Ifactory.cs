﻿
namespace FormatFiles.Model.Interfaces
{
    public interface IFactory
    {
        IStreamReader CustomStreamReader { get; }
        IFileStream CustomeFileStream { get; }
        IHostingEnvironment CustomHostingEnvironment { get; }
        IDirectoryInfo CustomDirectoryInfo { get; }
    }
}
