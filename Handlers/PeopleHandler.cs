using API_Project.Data;
using API_Project.DTO;
using API_Project.Models;
using API_Project.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;

namespace API_Project.Handlers
{
    public static class PeopleHandler
    {
        // Gets all/searched people names and id's 
        public static IResult GetPeople(ApplicationContext context, string? search)
        {
            try
            {
                // Check if user sent in a search query or not
                if (string.IsNullOrEmpty(search))
                {
                    // Get all people
                    List<PeopleViewModel> people = context.People
                    .Select(p => new PeopleViewModel()
                    {
                        Id = p.Id,
                        Name = $"{p.FirstName} {p.LastName}"
                    })
                    .ToList();

                    if (people == null || !people.Any())
                        return Results.NotFound("Error. No person found.");

                    return Results.Json(people);
                }
                else
                {
                    // Get all people matching search
                    List<PeopleViewModel> people = context.People
                    .Select(p => new PeopleViewModel()
                    {
                        Id = p.Id,
                        Name = p.FirstName + " " + p.LastName,
                    })
                    .Where(p => p.Name
                    .StartsWith(search))
                    .ToList();

                    Console.WriteLine(people);

                    if (people == null || !people.Any())
                        return Results.NotFound($"Error. No person found whose name starts with {search}");

                    return Results.Json(people);
                }  
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
                // TODO: Check if person already has interest with any and return already exists something
                // Maybe make a method in DbHelper to do it
                
                
                Interest interest = context.Interests
                    .Where(i => i.Id == interestId)
                    .Single();

            
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
                return Results.Text($"An error occurred: {ex.Message}");
            }
        }

        // Get all interest links for the selected person
        public static IResult GetPersonLinks(ApplicationContext context, string personId)
        {
            try
            { 
                var interestLinks = context.InterestsLinks
                    .Where(il => il.PersonId == personId)
                    .Select(il => il.WebLink)
                    .ToList();

                return Results.Json(interestLinks);
            }
            catch (Exception ex)
            {
                return Results.Text($"An error occurred: {ex.Message}");
            }
        }
    }
}
