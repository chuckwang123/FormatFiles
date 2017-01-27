using System.Collections.Generic;

namespace FormatFiles.Model.Models
{
    public class Result
    {
        public List<Person> pipResult { get; set; }
        public List<Person> commaResult { get; set; }
        public List<Person> spaceResult { get; set; }
    }
}
