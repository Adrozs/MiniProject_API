using System.ComponentModel.DataAnnotations.Schema;

namespace API_Project.Models
{
    // Specify the table name as it's called "Interests" in ApplicationContext
    [Table("interest")]
    public class Interest
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Person> People { get; set; }
        public virtual ICollection<InterestsLink> InterestsLinks { get; set; }

    }
}
