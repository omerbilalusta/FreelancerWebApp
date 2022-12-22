using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerWebApp.Models
{
    public class inbox_participants
    {
        public int Id { get; set; }
        public user user { get; set; }

        public inbox inbox { get; set; }

        public inbox_participants()
        {

        }
    }
}
