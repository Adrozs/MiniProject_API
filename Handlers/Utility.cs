using API_Project.Models;

namespace API_Project.Handlers
{
    public class Utility
    {
        // Create ID from the first 3 letters of first- and last name
        public static string CreatePersonId(string firstName, string lastName)
        {
            return string.Join(' ', firstName[0], firstName[1], firstName[2], lastName[0], lastName[1], lastName[2]).Trim();
        }


        // Create interest ID from the first 3 letters in the title
        public static string CreateInterestId(string title)
        {
            return string.Join(' ', title[0], title[1], title[2]).Trim();
        }

        public static string GetName(Person person)
        {
            return $"{person.FirstName} {person.LastName}";
    }
}
    }
}
