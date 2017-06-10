using System.ComponentModel.DataAnnotations;

namespace OdeToFood.Data.DomainClasses
{
    public class Review
    {
        public int Id { get; set; }

        [Range(1, 10)]
        public int Rating { get; set; } //Rating moet een getal van 1 tot en met 10 zijn

        public string Body { get; set; }

        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }

        [Required]
        public string ReviewerName { get; set; }
    }
}
