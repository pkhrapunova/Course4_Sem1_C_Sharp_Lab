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

        // GET: /Account/Login
        public IActionResult Login() => View();

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string phone, string passport)
        {
            var customer = _context.Customers
                .FirstOrDefault(c => c.Phone == phone && c.Passport == passport);

            if (customer == null)
            {
                ViewBag.Error = "Неверные данные для входа!";
                return View();
            }

            // Сохраняем ID пользователя в сессии (упрощённо)
            HttpContext.Session.SetInt32("CustomerID", customer.CustomerID);
            HttpContext.Session.SetString("CustomerName", customer.FullName);

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register
        public IActionResult Register() => View();

        // POST: /Account/Register
        [HttpPost]
        public IActionResult Register(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(customer);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
