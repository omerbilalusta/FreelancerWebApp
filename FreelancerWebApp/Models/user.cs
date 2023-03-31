using System.ComponentModel;

namespace FreelancerWebApp.Models
{
    
    public class user
    {
        public int Id { get; set; }
        public string? user_name { get; set; }
        public string? user_email { get; set; }
        public int Money { get; set; } = 0;

        public user()
        {

        }
    }
}
