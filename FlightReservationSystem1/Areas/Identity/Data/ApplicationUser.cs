using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FlightReservationSystem1.Models;
using Microsoft.AspNetCore.Identity;

namespace FlightReservationSystem1.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [Required(ErrorMessage = "İsim alanı boş bırakılamaz")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Soyisim alanı boş bırakılamaz")]
    public string Surname { get; set; }
    public List<Reservation>? Reservations { get; set; }
}

