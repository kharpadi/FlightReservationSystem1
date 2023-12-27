using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightReservationSystem1.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Plane")]
        [Required]
        public int PlaneId { get; set; }
        [ForeignKey("Route")]
        [Required]
        public int RouteId { get; set; }
        public Plane? Plane { get; set; }
        public Route? Route { get; set; }
        [Required(ErrorMessage = "Determine your departure time!!")]
        public DateTime DepartureTime { get; set; }
    }
}
