﻿using FlightReservationSystem1.Areas.Identity.Data;
using FlightReservationSystem1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Route = FlightReservationSystem1.Models.Route;

namespace FlightReservationSystem1.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Schedule> schedules { get; set; }
    public DbSet<Route>route { get; set; }   
    public DbSet<Reservation> reservations { get; set; }
    public DbSet<Plane> planes { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
