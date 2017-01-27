namespace FormatFiles.Model.Models
{
    public class CommaFileParserFactory: FileParserFactory
    {
        protected override string Type { get; set; } = "Comma";
    }
}
