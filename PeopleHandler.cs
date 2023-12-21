using API_Project.Data;
using API_Project.Models;
using API_Project.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace API_Project
{
    public static class PeopleHandler
    {
        // Gets all people names and id's in the database
        public static IResult GetPeopleNames(ApplicationContext context)
        {
            var people = context.People
                .Select(p => new PeopleViewModel()
                {
                    Id = p.Id,
                    Name = $"{p.FirstName} {p.LastName}"
                })
                .ToList();

            if (people != null)
                return Results.NotFound("Error. No person found.");

            return Results.Json(people);
        }

        // Gets all interests and their ids in the database 
        public static IResult GetInterests(ApplicationContext context)
        {
            var interests = context.Interests
                .Select(p => new InterestViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                })
                .ToList();

            if (interests == null)
                return Results.NotFound("Error. No interests found.");

            return Results.Json(interests);
        }

        // Gets all interests from a specific person
        public static IResult GetPersonInterests(ApplicationContext context, string personId)
        {
            // Check if ID exists in the db
            if (!DbHelper.CheckValidId(context, personId))
                return Results.NotFound($"Error. Person \"{personId}\" not found.");

            // Get the person and their interests
            var person = context.People
                .Include(p => p.Interests)
                .SingleOrDefault(p => p.Id == personId);
            
            // Make sure the person has interests and that they're not null
            if (person.Interests == null || !person.Interests.Any())
                return Results.NotFound($"Error. No interests found for \"{person.Id}\".");


            List<InterestPersonViewModel> personInterests =
                person.Interests
                .Select(i => new InterestPersonViewModel()
                {
                    Title = i.Title,
                    Description = i.Description,
                })
                .ToList();

            return Results.Json(personInterests);
        }

        // Create new person in the database based on a name
        public static IResult AddPerson(ApplicationContext context, string name)
        {
            string firstName = name.Split('-')[0];
            string lastName = name.Split('-')[1];
            string id = string.Join(' ', firstName[0], firstName[1], firstName[2], lastName[0], lastName[1], lastName[2]).Trim();

            Person person = new Person()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
            };

            try
            {
                context.People.Add(person);
                context.SaveChanges();

                // Check if adding to the database was succesful.
                // return Results.Problem or maybe Results.ValidationError if unsucesful

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        // Connect a person to a new interest
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
                return Results.StatusCode(500);
            }
        }

        // Add new links for a specific person and a specific interest
        public static IResult AddInterestLink(ApplicationContext context, string personId, string interestId)
        {
            // we need to send in the link via the API "body" as a json object and then convert it to a string?
        }
    }
}
