namespace FormatFiles.Model.Models
{
    public class PipFileParserFactory : FileParserFactory
    {
        protected override string Type { get; set; } = "Pip";
    }
}
