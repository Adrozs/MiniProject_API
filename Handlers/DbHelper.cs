using API_Project.Data;
using API_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Project.Handlers
{
    public class DbHelper
    {
        public static bool PersonExists(ApplicationContext context, string id)
        {
            return context.People.Any(p => p.Id == id);
        }

        public static bool InterestExists(ApplicationContext context, string id) 
        { 
            return context.Interests.Any(p => p.Id == id); 
        }

        public static Person GetPersonAndInterests(ApplicationContext context, string personId)
        {
            Person person = context.People
                .Include(p => p.Interests)
                .Where(p => p.Id == personId)
                .Single();

            return person;
        }

        public static Interest GetInterest(ApplicationContext context, string interestId)
        {
            Interest interest = context.Interests
                .Where(i => i.Id == interestId)
                .Single();

            return interest;
        }
    }
}
