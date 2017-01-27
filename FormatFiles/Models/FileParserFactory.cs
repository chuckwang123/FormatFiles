using System.Collections.Generic;

namespace FormatFiles.Models
{
    public abstract class FileParserFactory
    {
        protected FileParser FileParser { get; set; }
        public List<Person> OriData { get; set; } = new List<Person>();
        public List<Person> ResultData { get; set; } = new List<Person>();

        protected abstract string Type { get; set; }

        protected FileParserFactory() { }

        public void Setup(FileParser fileParser)
        {
            FileParser = fileParser;
            OriData = FileParser.ParseFile(Type);
        }

        public abstract void WriteRecord(Person person);
    }
}
