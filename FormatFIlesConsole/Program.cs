using FormatFiles.Model.Models;

namespace FormatFiles.Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("Please enter a numeric argument.");
                System.Console.WriteLine("Usage: Factorial <num>");
                return;
            }
            var sortWay = args[0];
            var bootStrapper = new BootStrapper();
            bootStrapper.Sort(sortWay);
        }
    }
}
