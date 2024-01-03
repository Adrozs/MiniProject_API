using System.ComponentModel.DataAnnotations.Schema;

namespace API_Project.Models
{
    // Specify the table name as it's called "People" in ApplicationContext
    [Table("person")] 
    public class Person
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? PhoneNumber { get; set; }

        public virtual ICollection<Interest> Interests { get; set; }
        public virtual ICollection<InterestsLink> InterestsLinks { get; set; }
    }
}
