using System.Threading.Tasks;
using FormatFiles.Model.Interfaces;

namespace FormatFiles.Model.Models
{
    public class SpaceFileParserFactory : FileParserFactory
    {
        public SpaceFileParserFactory(IFactory factory) : base(factory)
        {
        }

        protected override string Type { get; set; } = "Space";
        public override async Task<int> WriteRecord(Person person)
        {
            if (OriData.Contains(person)) return 0;
            using (var spaceWriter = FileParser.CreateStreamWriter())
            {
                spaceWriter.WriteLine(
                    $"{person.LastName} {person.FirstName} {person.Gender} {person.FavoriteColor} {person.DateofBirth:M/d/yyyy}");
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
