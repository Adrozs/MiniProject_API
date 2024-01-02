using API_Project.Data;

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


        public static string GetInterestId(ApplicationContext context, string interest)
        {
            // Get id from the name
            string interestId = context.Interests
                .Where(i => i.Title == interest)
                .Select(i => i.Id)
                .Single();

            return interestId;
        }
    }
}
