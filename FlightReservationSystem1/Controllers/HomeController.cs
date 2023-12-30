using FlightReservationSystem1.Areas.Identity.Data;
using FlightReservationSystem1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FlightReservationSystem1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult SearchFlights()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchFlights(string DepartureCity, string ArrivalCity, DateTime SelectedDate)
        {
            // Seçilen tarihte ve şehirlerde olan uçuşları filtrele
            var flights = _context.schedules
                .Include(s=>s.Plane)
                .Include(s => s.Route)
                .Where(s =>
                    s.Route.DepartureCity == DepartureCity &&
                    s.Route.ArrivalCity == ArrivalCity &&
                    s.DepartureTime.Date <= SelectedDate.Date).OrderBy(s => s.DepartureTime)
                .ToList();
            ViewBag.DepartureCity = DepartureCity;
            ViewBag.ArrivalCity = ArrivalCity;

            return View(flights);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
