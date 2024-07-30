using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace BookStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Müşteri kontrolü
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(m => m.c_mail == model.Email && m.password == model.Password);

                if (customer != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, customer.c_mail),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("CstmrProfile", "Customers");
                }

                // Satıcı kontrolü
                var seller = await _context.Sellers
                    .FirstOrDefaultAsync(s => s.s_mail == model.Email && s.s_password == model.Password);

                if (seller != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, seller.s_mail),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("Profile", "Seller");
                }

                // Admin kontrolü
                var admin = await _context.Admin
                    .FirstOrDefaultAsync(a => a.AdminCode == model.Email && a.Password == model.Password);

                if (admin != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, admin.AdminCode),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // Admin giriş yaptıktan sonra Admin sayfasına yönlendir
                    return RedirectToAction("Admin", "Admin");
                }

                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.c_mail == model.Email);
                var existingSeller = await _context.Sellers.FirstOrDefaultAsync(s => s.s_mail == model.Email);

                if (existingCustomer != null || existingSeller != null)
                {
                    ModelState.AddModelError(string.Empty, "This email address is already registered.");
                    return View(model);
                }

                var customer = new customers
                {
                    Name = model.Name,
                    c_mail = model.Email,
                    password = model.Password,
                    phone_num = model.PhoneNum
                };

                _context.Add(customer);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        // GET: Account/SellerRegister
        public IActionResult SellerRegister()
        {
            return View();
        }

        // POST: Account/SellerRegister
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SellerRegister(SellerRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.c_mail == model.Email);
                var existingSeller = await _context.Sellers.FirstOrDefaultAsync(s => s.s_mail == model.Email);

                if (existingCustomer != null || existingSeller != null)
                {
                    ModelState.AddModelError(string.Empty, "This email address is already registered.");
                    return View(model);
                }

                var seller = new sellers
                {
                    s_name = model.Name,
                    s_mail = model.Email,
                    s_password = model.Password,
                    city = model.City,
                    s_phoneNum = model.PhoneNum
                };

                _context.Add(seller);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        public IActionResult Profile()
        {
            var email = User.Identity.Name;
            var seller = _context.Sellers.FirstOrDefault(s => s.s_mail == email);

            if (seller == null)
            {
                return NotFound();
            }

            return View("Profile", seller);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
