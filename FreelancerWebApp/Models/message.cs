using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerWebApp.Models
{
    public class message
    {
        public int Id { get; set; }
        public string? message_text { get; set; }
        public DateTime date_created { get; set; }
        public int inboxId { get; set; }
        [ForeignKey("inboxId")]
        public virtual inbox? inbox { get; set; }
        public int userId { get; set; }
        [ForeignKey("userId")]
        public virtual user? user { get; set; }


        public message()
        {
                
        }
    }
}
