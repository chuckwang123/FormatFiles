using System.Collections.Generic;
using System.Threading.Tasks;
using FormatFiles.Model.Interfaces;

namespace FormatFiles.Model.Models
{
    public abstract class FileParserFactory
    {
        protected FileParser FileParser { get; set; }
        public List<Person> OriData { get; set; } = new List<Person>();
        public List<Person> ResultData { get; set; } = new List<Person>();

        protected abstract string Type { get; set; }

        protected FileParserFactory(IFactory factory)
        {
            FileParser = new FileParser(factory);
        }

        public abstract void Setup(FileParser fileParser);
        
        public abstract Task<int> WriteRecord(Person person);
    }
}
