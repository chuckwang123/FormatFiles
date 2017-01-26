using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FormatFiles.Model;
using FormatFiles.Model.Models;

namespace FormatFiles.Console
{
    public class Program
    {
        public static IEnumerable<Person> SortByGender(List<Person> people)
        {
            return people.OrderBy(o => o.Gender).ThenBy(o => o.LastName);
        }

        public static IEnumerable<Person> SortByBod(List<Person> people)
        {
            return people.OrderBy(o => o.DateofBirth);
        }

        public static IEnumerable<Person> SortByLastName(List<Person> people)
        {
            return people.OrderByDescending(o => o.LastName);
        }

        public static void OutputData(IEnumerable<Person> data)
        {
            var str = new StringBuilder();
            str.Append("LastName\tFirstName\tGender\tFavoriteColor\tDateOfBirth\n");
            foreach (var item in data)
            {
                str.Append(
                    $"{item.LastName.PadRight(15)}\t{item.FirstName.PadRight(15)}\t{item.Gender.PadRight(5)}\t{item.FavoriteColor.PadRight(8)}\t{item.DateofBirth:M/d/yyyy}\n");
            }
            System.Console.WriteLine(str);
        }

        public static void Main(string[] args)
        {

            var files = FileLister.ListFiles();
            foreach (var file in files)
            {
                System.Console.WriteLine($"The file path is {file}"); 
            }
            var fileParser = new FileParser(File.OpenText(files[0]));


            //Setup the Lists and path
            var spaceFilePath = "";
            var commaFilePath = "";
            var pipFilePath = "";
            var dataList_space = new List<Person>();
            var dataList_comma = new List<Person>();
            var dataList_pip = new List<Person>();

            //Setup the Delimitor
            foreach (var file in files)
            {
                var result= fileParser.DetermineDelimiterType(file);
                switch (result)
                {
                    case "Space":
                        spaceFilePath = file;
                        break;
                    case "Comma":
                        commaFilePath = file;
                        break;
                    case "Pip":
                        pipFilePath = file;
                        break;
                    default:
                        throw new InvalidDataException("The data is incorrect Delimited");
                }
            }

            //Parse the files
            if (!string.IsNullOrEmpty(pipFilePath))
            {
                dataList_pip = fileParser.ParseFile(pipFilePath, '|');
            }
            if (!string.IsNullOrEmpty(commaFilePath))
            {
                dataList_comma = fileParser.ParseFile(commaFilePath, ',');
            }
            if (!string.IsNullOrEmpty(spaceFilePath))
            {
                dataList_space = fileParser.ParseFile(spaceFilePath, ' ');
            }

            //output
            System.Console.WriteLine("Please enter the sort function you want. There are 3 options:");
            System.Console.WriteLine("1, Sort by Gender, enter Gender");
            System.Console.WriteLine("2, Sort by BirthDay, enter Birth");
            System.Console.WriteLine("3, Sort by LastName, enter LastName");
            System.Console.WriteLine("No Case Sensitive");

            var readLine = System.Console.ReadLine();
            var enterValue = "";
            if (readLine != null)
            {
                enterValue = readLine.ToUpper();
            }

            var pipResult = new List<Person>();
            var commaResult = new List<Person>();
            var spaceResult = new List<Person>();

            switch (enterValue)
            {
                case "GENDER":
                    pipResult= SortByGender(dataList_pip).ToList();
                    commaResult =SortByGender(dataList_comma).ToList();
                    spaceResult = SortByGender(dataList_space).ToList();
                    break;
                case "BIRTH":
                    pipResult = SortByBod(dataList_pip).ToList();
                    commaResult = SortByBod(dataList_comma).ToList();
                    spaceResult = SortByBod(dataList_space).ToList();
                    break;
                case "LASTNAME":
                    pipResult = SortByLastName(dataList_pip).ToList();
                    commaResult = SortByLastName(dataList_comma).ToList();
                    spaceResult = SortByLastName(dataList_space).ToList();
                    break;
                default:
                    System.Console.WriteLine("The value you entered is wrong, see you!!!!");
                    return;
            }
            System.Console.WriteLine("###################### Here is the result for PIP delimitor #######################");
            OutputData(pipResult);
            System.Console.WriteLine("###################### Here is the result for COMMA delimitor #######################");
            OutputData(commaResult);
            System.Console.WriteLine("###################### Here is the result for SPACE delimitor #######################");
            OutputData(spaceResult);
        }
    }
}
