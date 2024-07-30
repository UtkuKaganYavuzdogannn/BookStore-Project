using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers/Profile
        public async Task<IActionResult> CstmrProfile()
        {
            var email = User.Identity.Name;
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.c_mail == email);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // Kitapların books tablosundan çekilmesi:
        public async Task<IActionResult> Shop()
        {
            var books = await _context.Books.ToListAsync();
            return View(books);
        }

        // Kitap detayını gösteren yeni action
        public async Task<IActionResult> Detail(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // Order sayfasına yönlendirme
        public async Task<IActionResult> Order(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new OrderViewModel
            {
                BookSellerId = book.fk_sid,
                BookId = book.idbooks,
                Price = book.price,
                Stock = book.stock
            };

            return View(viewModel);
        }

        // Customer Profil Bilgilerini Güncellediğimiz method
        [HttpGet]
        public IActionResult CusEditProfile()
        {
            var email = User.Identity.Name; // Giriş yapmış kullanıcının emailini al
            var customer = _context.Customers.FirstOrDefault(c => c.c_mail == email);

            if (customer == null)
            {
                return NotFound();
            }

            var model = new EditProfileCusViewModel
            {
                Name = customer.Name,
                phone_num = customer.phone_num,
                cus_city = customer.cus_city
            };

            return View(model);
        }

        // Form gönderip Cookie ile tutuğumuz mail ile Kullanıcın diğer bilgilerini güncelledik.
        [HttpPost]
        public IActionResult CusEditProfile(EditProfileCusViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = User.Identity.Name; // Giriş yapmış kullanıcının emailini al
                var customers = _context.Customers.FirstOrDefault(c => c.c_mail == email);

                if (customers == null)
                {
                    return NotFound();
                }

                customers.Name = model.Name;
                customers.phone_num = model.phone_num;
                customers.cus_city = model.cus_city;

                _context.Update(customers);
                _context.SaveChanges();

                return RedirectToAction("CstmrProfile");
            }

            return View(model);
        }

        // Siparişi tamamla
        [HttpPost]
        public async Task<IActionResult> CompleteOrder(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = User.Identity.Name;
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.c_mail == email);

                if (customer == null)
                {
                    return NotFound();
                }

                var book = await _context.Books.FirstOrDefaultAsync(b => b.idbooks == model.BookId);
                if (book == null)
                {
                    return NotFound();
                }

                if (model.Quantity > book.stock)
                {
                    ModelState.AddModelError("", "Sipariş miktarı stok miktarını aşamaz.");
                    return View("Order", model);
                }

                var order = new orders
                {
                    address = model.Address,
                    quantity = model.Quantity,
                    total_price = model.Quantity * model.Price,
                    recipient = model.Recipient,
                    order_time = DateTime.Now,
                    book_seller_id = model.BookSellerId,
                    customer_id = customer.cid,
                    fk_bookid = model.BookId // Burada fk_bookid alanını dolduruyoruz
                };

                book.stock -= model.Quantity;
                _context.Orders.Add(order);
                _context.Update(book);
                await _context.SaveChangesAsync();

                return RedirectToAction("CstmrProfile");
            }

            return View("Order", model);
        }
        // Siparişlerim (Past Orders) sayfası
        public async Task<IActionResult> PastOrder()
        {
            var email = User.Identity.Name;
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.c_mail == email);

            if (customer == null)
            {
                return NotFound();
            }

            var orders = await (from o in _context.Orders
                                join b in _context.Books on o.fk_bookid equals b.idbooks
                                join s in _context.Sellers on o.book_seller_id equals s.sid
                                where o.customer_id == customer.cid
                                select new PastOrderViewModel
                                {
                                    OrderId = o.oid,
                                    BookTitle = b.title,
                                    SellerName = s.s_name,
                                    CustomerName = customer.Name,
                                    OrderTime = o.order_time,
                                    Quantity = o.quantity,
                                    TotalPrice = o.total_price
                                }).ToListAsync();

            return View(orders);
        }
    }
}
