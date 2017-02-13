using System.Threading.Tasks;
using FormatFiles.Model.Interfaces;

namespace FormatFiles.Model.Models
{
    public class CommaFileParserFactory: FileParserFactory
    {
        public CommaFileParserFactory(IFactory factory) : base(factory)
        {
        }

        protected override string Type { get; set; } = "Comma";
        public override async Task WriteRecord(Person person)
        {
            using (var commaWriter = FileParser.CreateStreamWriter())
            {
                commaWriter.WriteLine(
                    $"{person.LastName},{person.FirstName},{person.Gender},{person.FavoriteColor},{person.DateofBirth:M/d/yyyy}");
            }
        }
        public override void Setup(FileParser fileParser)
        {
            this.FileParser = new FileParser(fileParser);
            OriData = FileParser.ParseFile(Type);
        }
    }
}
