namespace API_Project.ViewModels
{
    public class PersonHierarchicalViewModel
    {
        public string Name { get; set; }

        public List<InterestPersonViewModel> Interests { get; set; }
        public List<InterestsLinkViewModel> InterestsLinks { get; set; }
    }
}
