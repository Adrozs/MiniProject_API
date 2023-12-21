using API_Project.Data;
using API_Project.Models;
using API_Project.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace API_Project
{
    public class PeopleHandler
    {
        // Gets all people names and id's in the database
        public static List<PeopleViewModel> GetPeopleNames(ApplicationContext context)
        {
            var people = context.People
                .Select(p => new PeopleViewModel()
                {
                    Id = p.Id,
                    Name = $"{p.FirstName} {p.LastName}"
                })
                .ToList();
            
            return people;
        }

        // Gets all interests in the database and their ids
        public static List<InterestViewModel> GetInterests(ApplicationContext context)
        {
            var interests = context.Interests
                .Select(p => new InterestViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                })
                .ToList();

            return interests;
        }

        // Gets all interests from a specific person
        public static List<InterestPersonViewModel> GetPersonInterests(ApplicationContext context, string personId)
        {
            if(DbHelper.CheckValidId(context, personId))
            {
                var personInterests = context.People
               .Where(p => p.Id == personId)
               .Single()
               .Interests
               .Select(i => new InterestPersonViewModel()
               {
                   Title = i.Title,
                   Description = i.Description
               })
               .ToList();

                return personInterests;
            }

            return null;
           
        }

        // Create new person in the database based on a name
        public static IResult AddPerson(ApplicationContext context, string name)
        {
            string firstName = name.Split('-')[0];
            string lastName = name.Split('-')[1];
            string id = string.Join(' ', firstName[0], firstName[1], firstName[2], lastName[0], lastName[1], lastName[2]).Trim();

            Person person = new Person();
            person.Id = id;
            person.FirstName = firstName;
            person.LastName = lastName;

            context.People.Add(person);
            context.SaveChanges();

            // Check if adding to the database was succesful.
            // return Results.Problem or maybe Results.ValidationError if unsucesful

            return Results.Ok();
        }

        public static IResult AddPersonInterest(ApplicationContext context, string personId, string interestId)
        {
            // Check if person already has interest with any and return already exists something

            var interest = context.Interests
                .Where(i => i.Id == interestId)
                .Single();

            try
            {
                context.People
                    .Where(p => p.Id == personId)
                    .Include(p => p.Interests)
                    .Single()
                    .Interests
                    .Add(interest);

                context.SaveChanges();

                return Results.Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Results.StatusCode(500);
            }
        }
    }
}
