using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Models;
using System;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [Authorize]
    public class SellerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SellerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Seller/Profile
        public async Task<IActionResult> Profile()
        {
            var email = User.Identity.Name;
            var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.s_mail == email);

            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        // GET: Seller/EditProfile
        [HttpGet]
        public IActionResult EditProfile()
        {
            var email = User.Identity.Name; // Giriş yapmış kullanıcının emailini al
            var seller = _context.Sellers.FirstOrDefault(s => s.s_mail == email);

            if (seller == null)
            {
                return NotFound();
            }

            var model = new EditProfileViewModel
            {
                s_name = seller.s_name,
                s_phoneNum = seller.s_phoneNum,
                city = seller.city
            };

            return View(model);
        }
        // Form gönderip Cookie ile tutuğumuz mail ile Kullanıcın diğer bilgilerini güncelledik.
        [HttpPost]
        public IActionResult EditProfile(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = User.Identity.Name; // Giriş yapmış kullanıcının emailini al
                var seller = _context.Sellers.FirstOrDefault(s => s.s_mail == email);

                if (seller == null)
                {
                    return NotFound();
                }

                seller.s_name = model.s_name;
                seller.s_phoneNum = model.s_phoneNum;
                seller.city = model.city;

                _context.Update(seller);
                _context.SaveChanges();

                return RedirectToAction("Profile");
            }

            return View(model);
        }

        //Mağazam sayfası görüntüleme:
        [HttpGet]
        public async Task<IActionResult> Store()
        {
            var email = User.Identity.Name;
            var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.s_mail == email);

            if (seller == null)
            {
                return NotFound();
            }

            var books = await _context.Books.Where(b => b.fk_sid == seller.sid).ToListAsync();

            return View(books);
        }
        // BookUpdate GET Aksiyonu
        public async Task<IActionResult> BookUpdate(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new BooksUpdateViewModel
            {
                Id = book.idbooks,
                Title = book.title,
                Author = book.author,
                Price = book.price,
                Stock = book.stock,
                Photo = book.photo
            };

            return View(viewModel);
        }

        // BookUpdate POST Aksiyonu
        [HttpPost]
        public async Task<IActionResult> BookUpdate(BooksUpdateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var book = await _context.Books.FindAsync(viewModel.Id);
                if (book == null)
                {
                    return NotFound();
                }

                book.title = viewModel.Title;
                book.author = viewModel.Author;
                book.price = viewModel.Price;
                book.stock = viewModel.Stock;
                book.photo = viewModel.Photo;

                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Store)); // Başarılı güncelleme sonrası Store aksiyonuna yönlendirme
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Veritabanı güncellenirken bir hata oluştu.");
                }
            }
            return View(viewModel);
        }
        // Kitabı silmek için DeleteBook method'u ekledik

        [HttpPost]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Store));
        }
        //Kitap eklemek için Controller:
        public IActionResult Products()
        {
            ViewData["Title"] = "Ürün Ekle";
            var model = new AddBooksViewModel(); // Yeni bir model oluştur
            return View(model); // Modeli görünümle birlikte geç
        }

        // Satış görüntüleme:

        public async Task<IActionResult> Sales()
        {
            var email = User.Identity.Name;
            var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.s_mail == email);

            if (seller == null)
            {
                return NotFound();
            }

            var sales = await (from o in _context.Orders
                               join b in _context.Books on o.fk_bookid equals b.idbooks
                               join c in _context.Customers on o.customer_id equals c.cid
                               where o.book_seller_id == seller.sid
                               select new PastOrderViewModel
                               {
                                   OrderId = o.oid,
                                   BookTitle = b.title,
                                   SellerName = seller.s_name,
                                   CustomerName = c.Name,
                                   OrderTime = o.order_time,
                                   Quantity = o.quantity,
                                   TotalPrice = o.total_price 
                               }).ToListAsync();

            return View(sales);
        }
    }
}




 

