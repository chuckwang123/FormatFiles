using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormatFiles.Model.Models
{
    public static class Helper
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
    }
}
