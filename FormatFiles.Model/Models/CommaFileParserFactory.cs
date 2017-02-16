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
        public override async Task<int> WriteRecord(Person person)
        {
            if (OriData.Contains(person)) return 0;
            using (var commaWriter = FileParser.CreateStreamWriter())
            {
                commaWriter.WriteLine(
                    $"{person.LastName},{person.FirstName},{person.Gender},{person.FavoriteColor},{person.DateofBirth:M/d/yyyy}");
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
