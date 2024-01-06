using API_Project.Data;
using API_Project.DTO;
using API_Project.Models;
using API_Project.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace API_Project.Handlers
{
    public static class PeopleHandler
    {
        // Gets all/searched people names and id's w/ optional pagination
        public static IResult GetPeople(ApplicationContext context, int? page, int? results, string? search)
        {
            try
            {
                // Check if page or result is less than 1 return error message depending on which one was true
                if (page < 1 || results < 1)
                    return Results.BadRequest(page < 1
                         ? "Invalid page number."
                        :$"Invalid page size.");


                // Get all people
                List<PeopleViewModel> people = context.People
                .Select(p => new PeopleViewModel()
                {
                    Id = p.Id,
                    Name = $"{p.FirstName} {p.LastName}"
                })
                .ToList();

                if (!string.IsNullOrEmpty(search)) { }
                    people = ApplySearch(context, people, search);

                people = ApplyPagination(people, page, results);


                // Check if result is null or empty and return error message depending on if a search was made or not
                if (people == null || !people.Any())
                    return Results.NotFound(string.IsNullOrEmpty(search)
                         ?"Error no people found"
                        :$"Error. No person found whose name starts with {search}");

                return Results.Json(people);
                
            }
            catch (Exception ex)
            {
                return Utility.HandleErrors(ex);
            }
        }

       private static List<PeopleViewModel> ApplySearch(ApplicationContext context, List<PeopleViewModel> people, string? search)
       {
            people = context.People
            .Where(p => (p.FirstName + " " + p.LastName).StartsWith(search))
            .Select(p => new PeopleViewModel
            {
                Id = p.Id,
                Name = $"{p.FirstName} {p.LastName}"
            })
            .ToList();

            return people;
       }


        private static List<PeopleViewModel> ApplyPagination(List<PeopleViewModel> people, int? page, int? results)
        {
            // Set default values for pagination if no value was sent in
            if (page == null)
                page = 1;

            if (results == null)
                results = people.Count(); // People.Count so all people will be shown if no choice was made


            // Calculate the number of items to skip and show based on values sent in
            int skip = (int)((page - 1) * results);
            int take = (int)results;


            // Apply pagination and save result
            List<PeopleViewModel> peoplePaginated =
                people
                .Skip(skip)
                .Take(take)
                .ToList();

            return peoplePaginated;
        }


        public static IResult AddPerson(ApplicationContext context, PersonDto personDto)
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

                return Results.Ok($"Person {Utility.GetName(person)} added to database.");
            }
            catch (Exception ex)
            {
                return Utility.HandleErrors(ex);
            }
        }


        public static IResult ConnectPersonToInterest(ApplicationContext context, string personId, string interestId)
        {
            try
            {
                // Check if the person and interest exist
                if (!DbHelper.PersonExists(context, personId))
                    return Results.NotFound($"Error. Person \"{personId}\" not found.");

                if (!DbHelper.InterestExists(context, interestId))
                    return Results.NotFound($"Error. Interest \"{interestId}\" not found.");


                Interest interest = DbHelper.GetInterest(context, interestId);

                Person person = DbHelper.GetPersonAndInterests(context, personId);


                // Check if person already has that interest linked to them
                if (person.Interests.Contains(interest))
                {
                    return Results.Conflict($"{Utility.GetName(person)} already has the interest {interest.Title}");
                }


                // Add interest to db
                person.Interests
                .Add(interest);
                context.SaveChanges();

                return Results.Ok($"Interest {interest.Title} added to {Utility.GetName(person)}");
            }
            catch (Exception ex)
            {
                return Utility.HandleErrors(ex);
            }
        }


        public static IResult GetPersonLinks(ApplicationContext context, string personId)
        {
            try
            {
                // Check if the person exist
                if (!DbHelper.PersonExists(context, personId))
                    return Results.NotFound($"Error. Person \"{personId}\" not found.");


                List<InterestsLinkViewModel> interestLinks = 
                    context.InterestsLinks
                    .Where(il => il.Person.Id == personId)
                    .Select(il => new InterestsLinkViewModel
                    {
                        WebLink = il.WebLink
                    })
                    .ToList();


                return Results.Json(interestLinks);
            }
            catch (Exception ex)
            {
                return Utility.HandleErrors(ex);
            }
        }

        public static IResult GetPersonHierarchical(ApplicationContext context, string personId)
        {
            try
            {
                // Check if the person exist
                if (!DbHelper.PersonExists(context, personId))
                    return Results.NotFound($"Error. Person \"{personId}\" not found.");


                // Get person and all their interests and links
                Person person = context.People
                    .Include(p => p.Interests)
                    .Include(p => p.InterestsLinks)
                    .Where(p => p.Id == personId)
                    .Single();


                // Get all interests
                List<InterestPersonViewModel> personInterests =
                        person.Interests
                        .Select(i => new InterestPersonViewModel()
                        {
                            Title = i.Title,
                            Description = i.Description,
                        })
                        .ToList();

                // Get all interests links
                List<InterestsLinkViewModel> personInterestLinks =
                    person.InterestsLinks
                    .Select(il => new InterestsLinkViewModel()
                    {
                        WebLink = il.WebLink
                    })
                    .ToList();

                // Create new view model 
                PersonHierarchicalViewModel result = new PersonHierarchicalViewModel()
                {
                    Name = $"{person.FirstName} {person.LastName}",
                    Interests = personInterests,
                    InterestsLinks = personInterestLinks
                };

                return Results.Json(result);
            }
            catch (Exception ex)
            {
                return Utility.HandleErrors(ex);
            }
        }
    }
}
