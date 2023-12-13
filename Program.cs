using API_Project.Data;
using Microsoft.EntityFrameworkCore;

namespace API_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionstring = builder.Configuration.GetConnectionString("MyConnection");
            builder.Services.AddDbContext<ApplicationContext>(
                options => options.UseSqlServer(connectionstring));

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            app.Run();


        }
    }
}
