using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightReservationSystem1.Areas.Identity.Data;
using FlightReservationSystem1.Models;
using Microsoft.AspNetCore.Authorization;

namespace FlightReservationSystem1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SchedulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Schedules
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.schedules.Include(s => s.Plane).Include(s => s.Route);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.schedules == null)
            {
                return NotFound();
            }

            var schedule = await _context.schedules
                .Include(s => s.Plane)
                .Include(s => s.Route)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
        public IActionResult Create()
        {
            ViewData["PlaneId"] = new SelectList(_context.planes, "Id", "Name");
            ViewData["RouteId"] = new SelectList(_context.route, "Id", "ArrivalCity");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlaneId,RouteId,DepartureTime")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlaneId"] = new SelectList(_context.planes, "Id", "Name", schedule.PlaneId);
            ViewData["RouteId"] = new SelectList(_context.route, "Id", "ArrivalCity", schedule.RouteId);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.schedules == null)
            {
                return NotFound();
            }

            var schedule = await _context.schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            ViewData["PlaneId"] = new SelectList(_context.planes, "Id", "Name", schedule.PlaneId);
            ViewData["RouteId"] = new SelectList(_context.route, "Id", "ArrivalCity", schedule.RouteId);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlaneId,RouteId,DepartureTime")] Schedule schedule)
        {
            if (id != schedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.Id))
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
            ViewData["PlaneId"] = new SelectList(_context.planes, "Id", "Name", schedule.PlaneId);
            ViewData["RouteId"] = new SelectList(_context.route, "Id", "ArrivalCity", schedule.RouteId);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.schedules == null)
            {
                return NotFound();
            }

            var schedule = await _context.schedules
                .Include(s => s.Plane)
                .Include(s => s.Route)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.schedules == null)
            {
                return Problem("Entity set 'ApplicationDbContext.schedules'  is null.");
            }
            var schedule = await _context.schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.schedules.Remove(schedule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
          return (_context.schedules?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
