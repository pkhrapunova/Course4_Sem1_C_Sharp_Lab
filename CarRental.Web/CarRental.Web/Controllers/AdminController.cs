using Microsoft.AspNetCore.Mvc;
using CarRental.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly CarRentalDbContext _context;

        public AdminController(CarRentalDbContext context)
        {
            _context = context;
        }

        // Главная страница админки — список машин и заказов
        public IActionResult Index()
        {
            var cars = _context.Cars.ToList();
            var orders = _context.Orders
                .Include(o => o.Car)
                .Include(o => o.Customer)
                .ToList();

            ViewBag.Orders = orders;
            return View(cars);
        }
    }
}
