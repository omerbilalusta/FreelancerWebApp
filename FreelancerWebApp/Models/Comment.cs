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
        public int rate { get; set; }

        [ForeignKey("job")]
        [DefaultValue(1)]
        public int JobId { get; set; }
        [ValidateNever]
        public job job { get; set; }

        [ForeignKey("ownerUser")]
        [DefaultValue(1)]
        public int ownerUserId { get; set; }
        [ValidateNever]
        public user ownerUser { get; set; }
        public int freelancerUserID { get; set; }
        public Comment()
        {
        
        }
    }
}
