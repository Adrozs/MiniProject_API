using API_Project.Data;
using API_Project.Handlers;
using Microsoft.EntityFrameworkCore;

namespace API_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Setup
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("ApplicationContext");
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
            var app = builder.Build();

            // Introduction text and instructions on the root page
            app.MapGet("/", () => "Welcome to CATPAPI, the ChasAcademy Teaching Platform API!\n\n" +
            "How to use CATPAPI:\n" +
            "-{} brackets are where you fill out data\n" +
            "-{?} means it's optional\n" +
            "-Search works by typing ex: \"/people/Ad\" and you'll get all students whose names start with \"Ad\" .\n" +
            "-Student IDs are the first 3 letters of their first- and last names (Bilbo Baggins = BILBAG)\n" +
            "-Interest IDs are the first 3 letters (Golf = GOL)\n\n" +


            "Here are the available GET commands:\n\n" +

            "PEOPLE:\n" +

            "/people - Displays all student names and their IDs\n" +
            "/people/{search?} - Displays all students whose name starts with the search\n" +
            "/people/{personId}/hierarchical - get out all interests and all links for that student hierarchically\n" +
            "/people/page/{page?}/results/{results?}/{search?} - Get all students and choose page and how many students shown per page. Option to filter students on pagination available with search\n\n" +

            "INTERESTS:\n" +
            "/interests - Displays all interests and their IDs\n" +
            "/interests/{search?} - Displays all interests whose title starts with the search\n" +
            "/interests/page/{page?}/results/{results?}/{search?} - Get all interests and choose page and how many interests shown per page. Option to filter interests on pagination available with search\n\n" +

            "PEOPLE & INTERESTS:\n" +
            "/people/{personId}/interests - Displays all interests for the chosen student\n" +
            "/people/{personId}/interests/links - Display all interest links connected to a student\n\n\n" +


            "Here are the available POST commands:\n" +
            "/people/ - Add a new student\n" +
            "/interests - Add a new interest\n" +
            "/people/{personId}/interests/{interestId} - Connect a student to an existing interest\n" +
            "/people/{personId}/interests/{interestId}/links/ - Add a link to a student and their chosen interest\n");


            // ENDPOINTS

            // People endpoints
            app.MapGet("/people/{search?}", PeopleHandler.GetPeople);
            app.MapGet("/people/page/{page?}/results/{results?}/{search?}", PeopleHandler.GetPeople);
            app.MapGet("/people/{personId}/hierarchical", PeopleHandler.GetPersonHierarchical);
            app.MapPost("/people", PeopleHandler.AddPerson);

            // Interests endpoints
            app.MapGet("/interests/{search?}", InterestsHandler.GetInterests);
            app.MapGet("/interests/page/{page?}/results/{results?}/{search?}", InterestsHandler.GetInterests);
            app.MapPost("/interests", InterestsHandler.AddInterest);

            // People interest endpoints
            app.MapGet("/people/{personId}/interests/{search?}", InterestsHandler.GetPersonInterests);
            app.MapPost("/people/{personId}/interests/{interestId}", PeopleHandler.ConnectPersonToInterest);

            // Interests link endpoints
            app.MapGet("/people/{personId}/interests/links", PeopleHandler.GetPersonLinks);
            app.MapPost("/people/{personId}/interests/{interestId}/links/", InterestsHandler.AddInterestLink);


            app.Run();
        }
    }
}
