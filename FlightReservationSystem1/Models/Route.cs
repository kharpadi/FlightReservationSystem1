using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem1.Models
{
    public class Route
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Determine Departure Location!!")]
        public string DepartureCity { get; set; }
        [Required(ErrorMessage = "Determine Arrival Location!!")]
        public string ArrivalCity { get; set; }
    }
}
