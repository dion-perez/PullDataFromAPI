namespace PullDataFromAPI.Models
{
    public class Candidate
    {
        public string? id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get;set; }
        public DateTime dob { get; set; }
        public string? favouriteColour { get; set; }
    }
}
