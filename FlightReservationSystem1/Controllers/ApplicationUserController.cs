using FlightReservationSystem1.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FlightReservationSystem1.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ApplicationUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ApplicationUserController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            var userList = await _userManager.Users.ToListAsync();
            return View(userList);
        }

        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }

            var users = await _userManager.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, string surname, string email, string password)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                Name = name,
                Surname = surname
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Kullanıcı başarıyla oluşturuldu, istediğiniz sayfaya yönlendirin.
                return RedirectToAction("Index");
            }

            // Eğer kullanıcı oluşturma başarısız olursa, ModelState hatalarını ekleyin.
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // Hata durumunda istediğiniz sayfaya yönlendirin.
            return View(); // Hata durumuna göre düzenleyin.
        }



    }
}
