using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerWebApp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string text { get; set; }
        public DateTime Created { get; set; }

        [ForeignKey("job")]
        [DefaultValue(1)]
        public int JobId { get; set; }
        [ValidateNever]
        public job job { get; set; }

        [ForeignKey("user")]
        [DefaultValue(1)]
        public int UserId { get; set; }
        [ValidateNever]
        public user user { get; set; }

        public Comment()
        {
        
        }
    }
}
