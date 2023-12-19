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

            app.MapGet("/", () => "Welcome to Canvas, the ChasAcademy teaching platform!\n\n" +
            "Here are the available commands:\n" +
            "/people - Displays all student names\n" +
            "/interests - Displays all interests\n" +
            "/interests/[full name] - Displays all interests for specified student\n" +
            "/interests/[full name] [interest] [link] - Adds the sent in link to the interest for the selected student");

            // Get all people in the database
            app.MapGet("/people", (ApplicationContext context) =>
            {
                var people = PeopleHandler.GetPeopleNames(context);

                if (people == null)
                    return Results.NoContent();
                
                return Results.Ok(people);
            });

            // Get all interest in the database
            app.MapGet("/interests", (ApplicationContext context) =>
            {
                var interests = PeopleHandler.GetInterests(context);

                if (interests == null)
                    return Results.NoContent();

                return Results.Ok(interests);
            });

            // Get all interest connected to a specific person
            app.MapGet("/interests/{name}", (ApplicationContext context, string name) =>
            {
                var interests = PeopleHandler.GetPersonInterests(context, name);

                if (interests == null)
                    return Results.NoContent();

                return Results.Ok(interests);
            });

            // Connect a person to a new interest
            app.MapPost("/interests/{name} {interest}", (ApplicationContext context, string name, string interest) =>
            {

            });

            // Add new links for a specific person and a specific interest
            app.MapPost("/interests/{name} {interest} {link}", (ApplicationContext context, string name, string interest, string link) =>
            {

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
