using API_Project.Data;
using API_Project.DTO;
using API_Project.Models;
using API_Project.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace API_Project.Handlers
{
    public static class PeopleHandler
    {
        // Gets all/searched people names and id's w/ optional pagination
        public static IResult GetPeople(ApplicationContext context, int? page, int? results, string? search)
        {
            try
            {
                if (page < 1)
                    return Results.BadRequest("Invalid page number.");

                if (results < 1)
                    return Results.BadRequest("Invalid page size.");


                // Get all people
                var people = context.People
                .Select(p => new PeopleViewModel()
                {
                    Id = p.Id,
                    Name = $"{p.FirstName} {p.LastName}"
                });


                // If a search was made 
                if (!string.IsNullOrEmpty(search))
                {
                    // Get all people matching search
                    people = context.People
                        .Where(p => (p.FirstName + " " + p.LastName).StartsWith(search))
                        .Select(p => new PeopleViewModel 
                        {
                            Id = p.Id,
                            Name = $"{p.FirstName} {p.LastName}"
                        });
                }


                // Set default values for pagination if no value was sent in
                if (page == null)
                    page = 1;

                if (results == null)
                    results = people.Count();

                // Calculate the number of items to skip and take based on values sent in
                int skip = (int)((page - 1) * results);
                int take = (int)(results);


                // Add pagination and save result
                List<PeopleViewModel> result = 
                    people
                    .Skip(skip)
                    .Take(take)
                    .ToList();


                    if (result == null || !result.Any())
                        return Results.NotFound(string.IsNullOrEmpty(search)
                            ?"Error no people found"
                            :$"Error. No person found whose name starts with {search}");

                return Results.Json(result);
                
            }
            catch (Exception ex)
            {
                return Results.Text($"An error occurred: {ex.Message}");
            }
        }


        // Add a person to the database
        public static IResult AddPerson(ApplicationContext context, string name, PersonDto personDto)
        {
            try
            {
                Person person = new Person()
                {
                    Id = Utility.CreatePersonId(personDto.FirstName, personDto.LastName), 
                    FirstName = personDto.FirstName,
                    LastName = personDto.LastName,
                    PhoneNumber = personDto.PhoneNumber,
                };
            
                context.People.Add(person);
                context.SaveChanges();

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Text($"An error occurred: {ex.Message}");
            }
        }


        // Connect a person to a new interest
        public static IResult ConnectPersonToInterest(ApplicationContext context, string personId, string interestId)
        {
            try
            {               
                // Get the interest 
                Interest interest = context.Interests
                    .Where(i => i.Id == interestId)
                    .Single();

                // Get person and their interests
                var person = context.People
                    .Where(p => p.Id == personId)
                    .Include(p => p.Interests)
                    .Single();

                // Check if person already has that interest linked to them
                if (person.Interests.Contains(interest))
                {
                    return Results.Text($"{Utility.GetName(person)} already has the interest {interest.Title}");
                }


                person
                .Interests
                .Add(interest);
                context.SaveChanges();

                return Results.Ok($"Interest {interest.Title} added to {Utility.GetName(person)}");
            }
            catch (Exception ex)
            {
                return Results.Text($"An error occurred: {ex.Message}");
            }
        }

        // Get all interest links for the selected person
        public static IResult GetPersonLinks(ApplicationContext context, string personId)
        {
            try
            {
                var interestLinks = context.People
                    .Where(p => p.Id == personId)
                    .Single()
                    .InterestsLinks
                    .Select(il => new InterestsLinkViewModels
                    {
                        WebLink = il.WebLink
                    })
                    .ToList();

                return Results.Json(interestLinks);
            }
            catch (Exception ex)
            {
                return Results.Text($"An error occurred: {ex.Message}");
            }
        }

        //public static IResult GetPersonHierarchical()
        //{

        //}
    }
}
