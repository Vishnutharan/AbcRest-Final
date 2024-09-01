using System.ComponentModel.DataAnnotations;

namespace AbcRest_Final.Model
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Range(1, 100)]
        public int Persons { get; set; }

        [Required]
        [Phone] 
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Time { get; set; }

        public string Note { get; set; }
    }
}

