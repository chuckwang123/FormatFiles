using FormatFiles.Model.Interfaces;

namespace FormatFiles.Model.Models
{
    public class SpaceFileParserFactory : FileParserFactory
    {
        public SpaceFileParserFactory(IFactory factory) : base(factory)
        {
        }

        protected override string Type { get; set; } = "Space";
        public override void WriteRecord(Person person)
        {
            using (var spaceWriter = FileParser.CreateStreamWriter())
            {
                spaceWriter.WriteLine(
                  $"{person.LastName} {person.FirstName} {person.Gender} {person.FavoriteColor} {person.DateofBirth:M/d/yyyy}");
            }
        }

        public override void Setup(FileParser fileParser)
        {
            this.FileParser = new FileParser(fileParser);
            OriData = FileParser.ParseFile(Type);
        }
    }
}
