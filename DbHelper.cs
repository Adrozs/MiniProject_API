using API_Project.Data;
using API_Project.Models;

namespace API_Project
{
    public class DbHelper
    {
        public static string GetPersonId(ApplicationContext context, string fullName)
        {
            // Separate name into to variables
            string firstName = fullName.Split(' ')[0];
            string lastName = fullName.Split(' ')[1];

            // Get id from the name combination
            string id = context.People
                .Where(p => p.FirstName == firstName && p.LastName == lastName)
                .Select(p => p.Id)
                .Single();

            return id;
        }

        public static string GetInterestId(ApplicationContext context, string interest)
        {
            // Get id from the name combination
            string interestId = context.Interests
                .Where(i => i.Title == interest)
                .Select(i => i.Id)
                .Single();

            return interestId;
        }
    }
}
