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
            "/people/[personId]/interests - Displays all interests for specified student\n\n" +
            "Here are the available POST commands:\n" +
            "/people/[personId]/interests/[interestId] - Add an existing interest to a user\n");


            app.MapGet("/people", PeopleHandler.GetPeopleNames);
            app.MapGet("/interests", PeopleHandler.GetInterests);            
            app.MapGet("/people/{personId}/interests", PeopleHandler.GetPersonInterests);
            app.MapPost("/people/{personId}/interests/{interestId}", PeopleHandler.AddPersonInterest);
            app.MapPost("/people/{personId}/interests/{interestId}/link/", (ApplicationContext context, string personId, string interestId, jhjss link) =>
           {
               // create link data object and send in as link in the post body
           });

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
