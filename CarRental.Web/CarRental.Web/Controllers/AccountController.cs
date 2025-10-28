using Microsoft.AspNetCore.Mvc;
using CarRental.Web.Models;

namespace CarRental.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly CarRentalDbContext _context;

        public AccountController(CarRentalDbContext context)
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
        public IActionResult Login(string phone, string passport)
        {
            var customer = _context.Customers
                .FirstOrDefault(c => c.Phone == phone && c.Passport == passport);

            if (customer != null)
            {
                HttpContext.Session.SetInt32("CustomerID", customer.CustomerID);
                HttpContext.Session.SetString("CustomerName", customer.FullName);
                HttpContext.Session.SetString("IsAdmin", customer.IsAdmin ? "true" : "false");

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Неверный телефон или серия паспорта");
            return View();
        }



        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Customer customer)
        {
            if (!ModelState.IsValid)
                return View(customer);

            customer.IsAdmin = false; // обязательно
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // удаляем данные сессии
            return RedirectToAction("Index", "Home");
        }
    }
}
