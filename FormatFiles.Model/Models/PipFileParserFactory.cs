namespace FormatFiles.Model.Models
{
    public class PipFileParserFactory : FileParserFactory
    {
        protected override string Type { get; set; } = "Pip";
        public override void WriteRecord(Person person)
        {
            using (var pipWriter = FileParser.CreateStreamWriter())
            {
                pipWriter.WriteLine(
                $"{person.LastName}|{person.FirstName}|{person.Gender}|{person.FavoriteColor}|{person.DateofBirth:M/d/yyyy}");
            }
            
        }
    }
}
