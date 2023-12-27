using FlightReservationSystem1.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightReservationSystem1.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        [Required]
        public string UserId { get; set; }
        [ForeignKey("Schedule")]
        [Required]
        public int ScheduleId { get; set; }
        public ApplicationUser? applicationUser { get; set; }
        public Schedule? Schedule { get; set; }
        [Required]
        public string SeatNumber { get; set; }
    }
}
