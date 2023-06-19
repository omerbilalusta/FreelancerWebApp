using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FreelancerWebApp.ViewModels
{
    public class CommentViewModel
    {
        [ValidateNever]
        public int Id { get; set; }
        [Required(ErrorMessage = "Comment can't be empty.")]
        public string text { get; set; }
        public DateTime Created { get; set; }
        public int rate { get; set; }
        public int JobId { get; set; }
        public int UserId { get; set; }
    }
}
