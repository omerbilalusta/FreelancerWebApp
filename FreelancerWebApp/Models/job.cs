namespace FreelancerWebApp.Models
{
    public class job
    {
        public int Id { get; set; }

        public string Job_Title { get; set; }

        public string Job_Category { get; set; }

        public string Job_Description { get; set; }

        public int Offered_Price { get; set; }

        public int Day { get; set; }

        public string Owner_ID { get; set; }

        public string Freelancer_ID { get; set; }


        public job ()
        {
            
        }
    }
}
