using System.ComponentModel.DataAnnotations.Schema;

namespace API_Project.Models
{
    [Table("InterestLink")]
    public class InterestsLink
    {
        public int Id { get; set; }
        public string? WebLink { get; set; }

        [ForeignKey("Person")]
        public string PersonId { get; set; }
        public Person Person { get; set; }

        [ForeignKey("Interest")]
        public string InterestId { get; set; }
        public Interest Interest { get; set; }
    }

}
