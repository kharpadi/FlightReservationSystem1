using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem1.Models
{
    public class Plane
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name field cannot be left empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Select Plane Type")]
        public string Type { get; set; }
        public int TotalSeat { get; set; }
    }
}
