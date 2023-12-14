namespace API_Project.Models
{
    public class Person
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int phoneNumber { get; set; }


        public virtual ICollection<Interest> Interests { get; set; }
        public virtual ICollection<InterestsLink> InterestsLinks { get; set; }
    }
}
