using FlightReservationSystem1.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightReservationSystem1.Controllers
{
    [Authorize]
    public class AuthUser : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthUser(UserManager<ApplicationUser> userManager, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;

        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize] // Bu eyleme sadece giriş yapmış kullanıcılar erişebilir
        public IActionResult UserProfile()
        {
            // Giriş yapmış kullanıcının Id'sini al
            var userId = _userManager.GetUserId(User);

            // Kullanıcıyı Id'ye göre bul
            var user = _userManager.FindByIdAsync(userId).Result;

            // ViewModel oluştur ve kullanıcı bilgilerini iletle
            var viewModel = new ApplicationUser
            {
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
            };

            return View(viewModel);
        }
        public async Task<IActionResult> UserReservations()
        {
            // Giriş yapmış kullanıcının Id'sini al
            var userId = _userManager.GetUserId(User);

            // Kullanıcının rezervasyonlarını çek
            var user = await _userManager.FindByIdAsync(userId);
            var reservations = _context.reservations.Where(r => r.UserId == userId).ToList();

            // ViewModel oluştur ve rezervasyonları iletle
            var viewModel = new ApplicationUser
            {
                Name = user.UserName,
                Surname = user.Surname,
                UserName = user.UserName,
                Email = user.Email,
                Reservations = _context.reservations
                .Include(r => r.Schedule)
                .ThenInclude(s => s.Route)
                .Where(r => r.UserId == userId)
                .ToList()

            };

            return View(viewModel);
        }

        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.reservations.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            // Kullanıcı sadece kendi rezervasyonunu silebilir
            var userId = _userManager.GetUserId(User);
            if (reservation.UserId != userId)
            {
                return Unauthorized();
            }

            _context.reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction("UserReservations", "ApplicationUser"); // Kullanıcının rezervasyonlarını görüntülediği sayfaya yönlendirme
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SearchFlights","Home");
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync(); // Kullanıcıyı oturumdan çıkart

                return RedirectToAction("Index", "Home"); // İsterseniz başka bir sayfaya yönlendirebilirsiniz
            }
            else
            {
                // Hesap silme başarısız olduysa, kullanıcıyı uyaran bir mesaj ekleyebilirsiniz
                ModelState.AddModelError(string.Empty, "Hesap silme işlemi başarısız oldu.");
                return View(); // Hata durumunda kullanıcıyı aynı sayfada tutabilir veya başka bir sayfaya yönlendirebilirsiniz
            }
        }

    }
}

