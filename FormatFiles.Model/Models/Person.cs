using System;

namespace FormatFiles.Model.Models
{
    public class Person : IEquatable<Person>
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public string FavoriteColor { get; set;}
        public DateTime DateofBirth { get; set; }

        public bool Equals(Person other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(LastName, other.LastName) && string.Equals(FirstName, other.FirstName) && string.Equals(Gender, other.Gender) && DateofBirth.Equals(other.DateofBirth);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = LastName?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (FirstName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Gender?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ DateofBirth.GetHashCode();
                return hashCode;
            }
        }
    }
}
