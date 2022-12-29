using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FreelancerWebApp.ViewModels
{
    public class JobAddViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title can't be empty.")]
        public string Job_Title { get; set; }

        [Required(ErrorMessage = "Category should be selected.")]
        public string Job_Category { get; set; }

        [Required(ErrorMessage = "Job description should be filled.")]
        public string Job_Description { get; set; }

        [Required(ErrorMessage = "Price should be determined.")]
        public int Offered_Price { get; set; }

        [Required(ErrorMessage = "Day should be determined.")]
        public int Day { get; set; }

        public string Owner_ID { get; set; }

        public string Freelancer_ID { get; set; }

        public IFormFile? File { get; set; }

        public string? Deliver_File_Path { get; set; }


        [Required(ErrorMessage = "Image should be uploaded.")]
        public IFormFile JobImage { get; set; }

        public string Job_Photo_Path { get; set; }

        public DateTime Publish_Date { get; set; }
    }
}
