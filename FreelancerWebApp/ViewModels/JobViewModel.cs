using Microsoft.AspNetCore.Http;

namespace FreelancerWebApp.ViewModels
{
    public class JobViewModel
    {
        public int Id { get; set; }

        public string Job_Title { get; set; }

        public string Job_Category { get; set; }

        public string Job_Description { get; set; }

        public int Offered_Price { get; set; }

        public int Day { get; set; }

        public string Owner_ID { get; set; }

        public string Freelancer_ID { get; set; }

        
        public IFormFile? File { get; set; }
        public string? Deliver_File_Path { get; set; }

        public IFormFile? JobImage { get; set; }
        public string? Job_Photo_Path { get; set; }

        public DateTime Publish_Date { get; set; }
    }
}
