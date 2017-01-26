using System.IO;
using FormatFiles.Interfaces;

namespace FormatFiles.Models
{
    public class FIleOpener : IFileOpener
    {
        public TextReader OpenFile(string filePath)
        {
            return File.OpenText(filePath);
        }
    }
}
