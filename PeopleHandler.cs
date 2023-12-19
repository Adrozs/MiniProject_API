using API_Project.Data;
using API_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Project
{
    public class PeopleHandler
    {
        public static List<string> GetPeopleNames(ApplicationContext context)
        {
            var people = context.People
            .Select(p => $"{p.FirstName} {p.LastName}")
            .ToList();

            return people;
        }

        public static List<string> GetInterests(ApplicationContext context)
        {
            var interests = context.Interests
                .Select(i => i.Title ) 
                .ToList();

            return interests;
        }

        // Using object for dynamic projection of the "interest" properties 
        public static List<object> GetPersonInterests(ApplicationContext context, string name)
        {
            string personId = DbHelper.GetPersonId(context, name);

            var personInterests = context.People
                .Where(p => p.Id == personId)
                .SelectMany(p => p.Interests)
                .Select(i => new
                {
                    i.Title,
                    i.Description
                })
                .ToList<object>();

            return personInterests;
        }

        public static void AddPersonInterest(ApplicationContext context, string name, string interest)
        {
            string personId = DbHelper.GetPersonId(context, name);
            string interestId = DbHelper.GetInterestId(context, interest);

            
        }
    }
}
