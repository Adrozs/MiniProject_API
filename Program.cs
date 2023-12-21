using API_Project.Data;
using API_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("ApplicationContext");
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
            var app = builder.Build();

            // Introduction text and instructions on the root page
            app.MapGet("/", () => "Welcome to Canvas, the ChasAcademy teaching platform!\n\n" +
            "Here are the available GET commands:\n" +  
            "/people - Displays all student names\n" +
            "/interests - Displays all interests\n" +
            "/interests/people/[personId] - Displays all interests for specified student\n\n" +
            "Here are the available POST commands:\n" +
            "/people/{personId}/interests/{interestId} - Add an existing interest to a user\n");

            // Get all people in the database
            app.MapGet("/people", (ApplicationContext context) =>
            {
                var people = PeopleHandler.GetPeopleNames(context);

                if (people == null)
                    return Results.NotFound();
                
                return Results.Ok(people);
            });

            // Get all interest in the database
            app.MapGet("/interests", (ApplicationContext context) =>
            {
                var interests = PeopleHandler.GetInterests(context);

                if (interests == null)
                    return Results.NotFound();

                return Results.Ok(interests);
            });

            app.MapGet("/interests/people/{personId}", PeopleHandler.GetPersonInterests);

            // Connect a person to a new interest
            app.MapPost("/people/{personId}/interests/{interestId}", (ApplicationContext context, string personId, string interestId) =>
            {
                return PeopleHandler.AddPersonInterest(context, personId, interestId);
            });

            // Add new links for a specific person and a specific interest
            //app.MapPost("/people/{personId}/interests/{interestId}/link/", (ApplicationContext context, string personId, string interestId, jhjss link) =>
            //{
            //    // create link data object and send in as link in the post body
            //});

            app.Run();


            // TO DO
            // [x] Get all people in the database

            // [x] Get all interest connected to a specific person

            // [] Connect a person to a new interest

            // [] Add new links for a specific person and a specific interest

            // EXTRA CHALLENGES
            // [] Give the option for the one calling the API and asking for a person to get out all interests
            // and all links for that person directly in a heirical JSON-file

            // [] Give the option for the one calling the API to filter what they get back, like a search.
            // For example if we send "to" when getting all people in the database we should get back everyone that
            // has a "to" in their name, like "Tobias or "Tomas". 
            // This you can create for all calls (anrop) if you want.

            // [] Create paginering of the calls (anrop). When we call for example people we maybe get the first
            // 100 people and have to call more time to get more people. 
            // Here it could be nice that the call decides how many people we get in a call, so we can choose to get
            // say 10 people if we just want that.


        }
    }
}
