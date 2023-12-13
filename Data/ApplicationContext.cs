using API_Project.Models;
using Microsoft.EntityFrameworkCore;
namespace API_Project.Data
{
    public class ApplicationContext : DbContext
    {
        DbSet<Person> person { get; set; }
        DbSet<Interest> interest { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    }
}
