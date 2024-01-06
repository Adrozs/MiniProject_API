﻿using API_Project.Data;
using API_Project.DTO;
using API_Project.Models;
using API_Project.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API_Project.Handlers
{
    public class InterestsHandler
    {
        // Gets all/searched interests and their ids in the database 
        public static IResult GetInterests(ApplicationContext context, string? search)
        {
            try 
            { 
                // Check if user sent in a search query or not
                if (string.IsNullOrEmpty(search))
                {
                    // Get all interests if there was no search
                    var interests = context.Interests
                    .Select(p => new InterestViewModel()
                    {
                        Id = p.Id,
                        Title = p.Title,
                    })
                    .ToList();

                    if (interests == null || !interests.Any())
                        return Results.NotFound("Error. No interests found.");

                    return Results.Json(interests);
                }
                else
                {
                    // Get selected interests if there was a search
                    List<InterestViewModel> interests = context.Interests
                    .Select(i => new InterestViewModel()
                    {
                        Id = i.Id,
                        Title = i.Title,
                    })
                    .Where(i => i.Title
                    .StartsWith(search))
                    .ToList();

                    Console.WriteLine(interests);

                    if (interests == null || !interests.Any())
                        return Results.NotFound($"Error. No interests found whose title starts with {search}");

                    return Results.Json(interests);
                }
            }
            catch (Exception ex)
            {
                return Results.Text($"An error occurred: {ex.Message}");
            }
        }


        public static IResult AddInterest(ApplicationContext context, InterestDto interestDto)
        {
            try
            {
                Interest interest = new Interest()
                {
                    Id = Utility.CreateInterestId(interestDto.Title),
                    Title = interestDto.Title,
                    Description = interestDto.Description,
                };

                context.Interests.Add(interest);
                context.SaveChanges();

                return Results.StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return Results.Text($"An error occurred: {ex.Message}");
            }
        }


        public static IResult GetPersonInterests(ApplicationContext context, string personId, string? search)
        {
            try 
            { 
                if (!DbHelper.PersonExists(context, personId))
                    return Results.NotFound($"Error. Person \"{personId}\" not found.");

                Person person = context.People
                        .Include(p => p.Interests)
                        .Where(p => p.Id == personId)
                        .Single();


                // Make sure the person has interests and that they're not null
                if (person.Interests == null || !person.Interests.Any())
                    return Results.NotFound($"Error. No interests found for \"{person.Id}\".");


                // Checks if there wasn't a search made an gets all interests for the person
                if (string.IsNullOrEmpty(search))
                {
                    
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
                // If a search was made get all interests for the person matching the search
                else
                {
                    List<InterestPersonViewModel> personInterests =
                    person.Interests
                    .Select(i => new InterestPersonViewModel()
                    {
                        Title = i.Title,
                        Description = i.Description,
                    })
                    .Where(i => i.Title
                    .StartsWith(search))
                    .ToList();

                    return Results.Json(personInterests);
                }
            }
            catch (Exception ex)
            {
                return Results.Text($"An error occurred: {ex.Message}");
            }
        }


        public static IResult AddInterestLink(ApplicationContext context, string personId, string interestId, InterestLinkDto interestLink)
        {
            try
            {
                // Check if the person and interest exist in the db
                if (!DbHelper.PersonExists(context, personId))
                    return Results.NotFound($"Error. Person \"{personId}\" not found.");

                if (!DbHelper.InterestExists(context, interestId))
                    return Results.NotFound($"Error. Interest \"{interestId}\" not found.");


                // Get person and interest

                Person person = 
                    context.People
                    .Where(p => p.Id == personId)
                    .Single();

                Interest interest = 
                    context.Interests
                    .Where(i => i.Id == interestId)
                    .Single();


                // Add link and the objects to the InterestLink table
                context.InterestsLinks
                    .Add(new InterestsLink()
                    {
                        WebLink = interestLink.WebLink,
                        Person = person,
                        Interests = interest

                    });
                    context.SaveChanges();

                return Results.StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return Results.Text($"An error occurred: {ex.Message}");
            }
        }
    }
}
