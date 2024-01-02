using API_Project.Data;
using API_Project.Handlers;
using Microsoft.EntityFrameworkCore;

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
            app.MapGet("/", () => "Welcome to CATPAPI, the ChasAcademy Teaching Platform API!\n\n" +
            "How to use CATPAPI:\n" +
            "-{} brackets are where you fill out data\n" +
            "-{search?} means it's optional\n" +
            "-Search works by typing ex: \"/people/Ad\" and you'll get all students whose names start with \"Ad\" .\n" +
            "-Student IDs are the first 3 letters of their first- and last names (Bilbo Baggins = BILBAG)\n" +
            "-Interest IDs are the first 3 letters (Golf = GOL)\n\n" +
            "Here are the available GET commands:\n" +  
            "/people - Displays all student names and their IDs\n" +
            "/people/{search} - Displays all students whose name starts with the search\n" +
            "/interests - Displays all interests and their IDs\n" +
            "/interests/{search} - Displays all interests whose title starts with the search\n" +
            "/people/{personId}/interests - Displays all interests for the chosen student\n" +
            "/people/{personId}/interests/links - Display all interest links connected to a student\n\n" +
            "Here are the available POST commands:\n" +
            "/people/ - Add a new student\n" +
            "/interests - Add a new interest\n" +
            "/people/{personId}/interests/{interestId} - Connect a student to an existing interest\n" +
            "/people/{personId}/interests/{interestId}/links/ - Add a link to a student and their chosen interest\n");

            // People commands
            app.MapGet("/people/{search?}", PeopleHandler.GetPeople);
            app.MapGet("/people/page/{page?}/results/{results?}/{search?}", PeopleHandler.GetPeople);
            app.MapPost("/people", PeopleHandler.AddPerson);

            // Interests commands
            app.MapGet("/interests/{search?}", InterestsHandler.GetInterests);
            app.MapPost("/interests", InterestsHandler.AddInterest);

            // People interest commands
            app.MapGet("/people/{personId}/interests/{search?}", InterestsHandler.GetPersonInterests);
            app.MapPost("/people/{personId}/interests/{interestId}", PeopleHandler.ConnectPersonToInterest);

            // Interests link commands
            app.MapGet("/people/{personId}/interests/links", PeopleHandler.GetPersonLinks);
            app.MapPost("/people/{personId}/interests/{interestId}/links/", InterestsHandler.AddInterestLink);
            
            app.Run();


            // TO DO
            // [x] Get all people in the database

            // [x] Get all interest connected to a specific person

            // [x] Connect a person to a new interest

            // [x] Add new links for a specific person and a specific interest

            // [x] Get all links that are connected to a specific person

            // EXTRA CHALLENGES
            // [] Give the option for the one calling the API and asking for a person to get out all interests
            // and all links for that person directly in a hierarchical JSON-file

            // [x] Give the option for the one calling the API to filter what they get back, like a search.
            // For example if we send "to" when getting all people in the database we should get back everyone that
            // has a "to" in their name, like "Tobias or "Tomas". 
            // This you can create for all calls (anrop) if you want.
            // [x] Add search to more methods?


            // [x] Create paginering of the calls (anrop). When we call for example people we maybe get the first
            // 100 people and have to call more time to get more people. 
            // Here it could be nice that the call decides how many people we get in a call, so we can choose to get
            // say 10 people if we just want that.


        }
    }
}
