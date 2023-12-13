namespace API_Project.Models
{
    public class Interest
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Person> People { get; set; }
        public virtual ICollection<InterestsLink> InterestsLinks { get; set; }

    }
}
