using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BookStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace BookStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Admin()
        {
            var adminCode = User.Identity.Name; // Giriş yapan adminin kodunu alın
            var admin = await _context.Admin.FirstOrDefaultAsync(a => a.AdminCode == adminCode);

            if (admin == null)
            {
                return NotFound();
            }

            return View(admin); // Admin nesnesini görünüme gönder
        }

        // ManageCustomer sayfasına yönlendirme
        public async Task<IActionResult> ManageCustomer()
        {
            var customers = await _context.Customers.Where(c => !c.IsDeleted).ToListAsync();
            return View(customers);
        }

        [HttpPost]
        public async Task<IActionResult> SoftDeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            customer.IsDeleted = true;
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageCustomer));
        }

        // ManageSeller sayfasına yönlendirme
        public async Task<IActionResult> ManageSeller()
        {
            var sellers = await _context.Sellers.ToListAsync();
            return View(sellers);
        }

        // Çıkış yap ve oturumu sonlandır
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account"); // Ana sayfaya veya giriş sayfasına yönlendirme
        }
    }
}
