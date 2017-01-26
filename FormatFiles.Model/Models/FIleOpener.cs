using System.IO;
using FormatFiles.Console.Interfaces;

namespace FormatFiles.Model.Models
{
    public class FIleOpener : IFileOpener
    {
        public TextReader OpenFile(string filePath)
        {
            return File.OpenText(filePath);
        }
    }
}
