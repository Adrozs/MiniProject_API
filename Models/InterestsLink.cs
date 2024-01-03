using System.ComponentModel.DataAnnotations.Schema;

namespace API_Project.Models
{
    [Table("InterestLink")]
    public class InterestsLink
    {
        public int Id { get; set; }
        public string? WebLink { get; set; }

        public virtual Person Person { get; set; }
        public virtual Interest Interests { get; set; }
    }

}
