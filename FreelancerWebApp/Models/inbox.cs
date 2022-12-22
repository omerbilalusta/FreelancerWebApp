using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerWebApp.Models
{
    public class inbox
    {
        public int Id { get; set; }
        public string? last_message { get; set; }
        public int last_user_sent_id { get; set; }

        [ForeignKey("last_user_sent_id")]
        public virtual user user { get; set; }

        public inbox()
        {

        }
    }
}
