using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightReservationSystem1.Areas.Identity.Data;
using FlightReservationSystem1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FlightReservationSystem1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationsController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.reservations.Include(r => r.Schedule).Include(r=>r.applicationUser);
            foreach (var reservation in applicationDbContext)
            {
                if (reservation.Schedule == null)
                {
                    reservation.Schedule = _context.schedules.Find(reservation.ScheduleId);
                }

                if (reservation.applicationUser == null)
                {
                    reservation.applicationUser =await _userManager.FindByIdAsync(reservation.UserId);
                }
            }
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.reservations == null)
            {
                return NotFound();
            }

            var reservation =await _context.reservations
                .Include(r => r.Schedule)
                .Include(r => r.applicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
 
                if (reservation.Schedule == null)
                {
                    reservation.Schedule = await _context.schedules.FindAsync(reservation.ScheduleId);
                }
                if (reservation.Schedule.Route == null)
                {
                    reservation.Schedule.Route = await _context.route.FindAsync(reservation.Schedule.RouteId);
                }
                if (reservation.Schedule.Plane == null)
                {
                    reservation.Schedule.Plane = await _context.planes.FindAsync(reservation.Schedule.PlaneId);
                }

            if (reservation.applicationUser == null)
                {
                    reservation.applicationUser = await _userManager.FindByIdAsync(reservation.UserId);
                }
            
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_userManager.Users, "Id", "Name");
            ViewData["ScheduleId"] = new SelectList(_context.schedules, "Id", "Id");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ScheduleId,SeatNumber")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                var schedule=await _context.schedules.FindAsync(reservation.ScheduleId);
                var plane = await _context.planes.FindAsync(schedule.PlaneId);
                plane.TotalSeat--;
                _context.Update(plane);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_userManager.Users, "Id", "Name",reservation.UserId);
            ViewData["ScheduleId"] = new SelectList(_context.schedules, "Id", "Id", reservation.ScheduleId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_userManager.Users, "Id", "Name", reservation.UserId);
            ViewData["ScheduleId"] = new SelectList(_context.schedules, "Id", "Id", reservation.ScheduleId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ScheduleId,SeatNumber")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_userManager.Users, "Id", "Name", reservation.UserId);
            ViewData["ScheduleId"] = new SelectList(_context.schedules, "Id", "Id", reservation.ScheduleId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.reservations
                .Include(r => r.Schedule)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.reservations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.reservations'  is null.");
            }
            var reservation = await _context.reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.reservations.Remove(reservation);
                var schedule = await _context.schedules.FindAsync(reservation.ScheduleId);
                var plane = await _context.planes.FindAsync(schedule.PlaneId);
                plane.TotalSeat++;
                _context.Update(plane);
                _context.SaveChanges();
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
          return (_context.reservations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
