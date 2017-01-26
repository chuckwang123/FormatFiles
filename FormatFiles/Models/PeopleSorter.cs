using System.Collections.Generic;
using System.Linq;

namespace FormatFiles.Models
{
    public static class PeopleSorter
    {
        public static IEnumerable<Person> SortByGender(List<Person> people)
        {
            return people.OrderBy(o => o.Gender).ThenBy(o => o.LastName);
        }

        public static IEnumerable<Person> SortByBod(List<Person> people)
        {
            return people.OrderBy(o => o.DateofBirth);
        }

        public static IEnumerable<Person> SortByName(List<Person> people)
        {
            return people.OrderByDescending(o => o.FirstName + o.LastName);
        }
    }
}
