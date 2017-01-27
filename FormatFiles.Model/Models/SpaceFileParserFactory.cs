namespace FormatFiles.Model.Models
{
    public class SpaceFileParserFactory : FileParserFactory
    {
        protected override string Type { get; set; } = "Space";
        public override void WriteRecord(Person person)
        {
            using (var spaceWriter = FileParser.CreateStreamWriter())
            {
                spaceWriter.WriteLine(
                  $"{person.LastName} {person.FirstName} {person.Gender} {person.FavoriteColor} {person.DateofBirth:M/d/yyyy}");
            }
        }
    }
}
