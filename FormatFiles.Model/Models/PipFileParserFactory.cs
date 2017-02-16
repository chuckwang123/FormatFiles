using System.Threading.Tasks;
using FormatFiles.Model.Interfaces;

namespace FormatFiles.Model.Models
{
    public class PipFileParserFactory : FileParserFactory
    {
        public PipFileParserFactory(IFactory factory) : base(factory)
        {
        }

        protected override string Type { get; set; } = "Pip";
        public override async Task<int> WriteRecord(Person person)
        {
            if (OriData.Contains(person)) return 0;
            using (var pipWriter = FileParser.CreateStreamWriter())
            {
                pipWriter.WriteLine(
                $"{person.LastName}|{person.FirstName}|{person.Gender}|{person.FavoriteColor}|{person.DateofBirth:M/d/yyyy}");
            }
            return 1;
        }

        public override void Setup(FileParser fileParser)
        {
            this.FileParser = new FileParser(fileParser);
            OriData = FileParser.ParseFile(Type);
        }
    }
}
