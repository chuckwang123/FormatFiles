using System.Collections.Generic;

namespace FormatFiles.Model.Models
{
    public class BaseFileFormat
    {
        public FileParser FileParser { get; set; }
        public List<Person> OriData { get; set; } = new List<Person>();
        public List<Person> ResultData { get; set; } = new List<Person>();
    }
}
