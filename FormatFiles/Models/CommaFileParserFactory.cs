using System;

namespace FormatFiles.Models
{
    public class CommaFileParserFactory: FileParserFactory
    {
        protected override string Type { get; set; } = "Comma";
        public override void WriteRecord(Person person)
        {
            var commaWriter = FileParser.CreateStreamWriter();
            commaWriter.WriteLine(
                    $"{person.LastName},{person.FirstName},{person.Gender},{person.FavoriteColor},{person.DateofBirth:M/d/yyyy}");
        }
    }
}
