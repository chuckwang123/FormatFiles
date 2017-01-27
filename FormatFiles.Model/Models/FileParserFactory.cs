using System.Collections.Generic;

namespace FormatFiles.Model.Models
{
    public abstract class FileParserFactory
    {
        private FileParser FileParser { get; set; }
        public List<Person> OriData { get; set; } = new List<Person>();
        public List<Person> ResultData { get; set; } = new List<Person>();

        protected abstract string Type { get; set; }

        protected FileParserFactory() { }
        
        public void Setup(FileParser fileParser)
        {
            FileParser = fileParser;
            OriData = FileParser.ParseFile(Type);
        }
    }
}
