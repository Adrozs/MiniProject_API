using API_Project.Data;
using API_Project.DTO;
using API_Project.Models;
using API_Project.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Runtime.CompilerServices;

namespace API_Project.Handlers
{
    public class InterestsHandler
    {
        // Gets all/searched interests and their ids in the database 
        public static IResult GetInterests(ApplicationContext context, int? page, int? results, string? search)
        {
            try 
            {
                // Check if page or result is less than 1 return error message depending on which one was true
                if (page < 1 || results < 1)
                    return Results.BadRequest(page < 1
                         ? "Invalid page number."
                        : $"Invalid page size.");


                // Get all interests
                List<InterestViewModel> interests = context.Interests
                .Select(p => new InterestViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                })
                .ToList();


                // If user sent in a search query apply search
                if (!string.IsNullOrEmpty(search))
                {
                    interests = interests
                    .Where(i => i.Title.ToLower()
                    .StartsWith(search.ToLower()))
                    .ToList();
                }

                interests = ApplyPagination(interests, page, results);


                // Check if result is null or empty and return error message depending on if a search was made or not
                if (interests == null || !interests.Any())
                    return Results.NotFound(string.IsNullOrEmpty(search) 
                        ?"Error. No interests found."
                        : $"Error. No interests found whose title starts with {search}");

                return Results.Json(interests);
            }
            catch (Exception ex)
            {
                return Utility.HandleErrors(ex);
            }
        }

        private static List<InterestViewModel> ApplyPagination (List<InterestViewModel> interests, int? page, int? results)
        {
            // Set default values for pagination if no value was sent in
            if (page == null)
                page = 1;

            if (results == null)
                results = interests.Count(); // interests.Count so all interests will be shown if no choice was made


            // Calculate the number of items to skip and show based on values sent in
            int skip = (int)((page - 1) * results);
            int take = (int)results;


            // Apply pagination and save result
            List<InterestViewModel> interestsPaginated =
                interests
                .Skip(skip)
                .Take(take)
                .ToList();

            return interestsPaginated;
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

                return Results.Ok($"Interest {interest.Title} added to database.");
            }
            catch (Exception ex)
            {
                return Utility.HandleErrors(ex);
            }
        }


        public static IResult GetPersonInterests(ApplicationContext context, string personId, string? search)
        {
            try 
            { 
                if (!DbHelper.PersonExists(context, personId))
                    return Results.NotFound($"Error. Person \"{personId}\" not found.");


                Person person = DbHelper.GetPersonAndInterests(context, personId);

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
                

                // Apply search if search was made
                if (!string.IsNullOrEmpty(search))
                {
                    personInterests = personInterests
                    .Where(i => i.Title.ToLower()
                    .StartsWith(search.ToLower()))
                    .ToList();
                }

                return Results.Json(personInterests);      
            }
            catch (Exception ex)
            {
                return Utility.HandleErrors(ex);
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


                // Get person and interest separately

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

                return Results.Ok($"Link added to {Utility.GetName(person)} for interest {interest.Title}.");
            }
            catch (Exception ex)
            {
                return Utility.HandleErrors(ex);
            }
        }
    }
}
