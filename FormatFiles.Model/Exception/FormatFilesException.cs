namespace FormatFiles.Model.Exception
{
    public class FormatFilesException : System.Exception
    {
        public FormatFilesException()
        {
        }

        public FormatFilesException(string message): base(message)
        {
        }

        public FormatFilesException(string message, System.Exception inner): base(message, inner)
        {
        }
    }
}
