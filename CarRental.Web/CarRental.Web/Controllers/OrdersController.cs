using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarRental.Web.Models;

namespace CarRental.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly CarRentalDbContext _context;

        public OrdersController(CarRentalDbContext context)
        {
            _context = context;
        }

        // GET: Orders (просмотр всех заказов — для администратора)
        //public async Task<IActionResult> Index()
        //{
        //    var orders = await _context.Orders
        //        .Include(o => o.Car)
        //        .Include(o => o.Customer)
        //        .OrderByDescending(o => o.OrderDate)
        //        .ToListAsync();

        //    return View(orders);
        //}
        public async Task<IActionResult> Index()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
                return RedirectToAction("Login", "Account");

            var orders = await _context.Orders
                .Include(o => o.Car)
                .Where(o => o.CustomerID == customerId)
                .ToListAsync();

            return View(orders);
        }

        // GET: Orders/Create (оформление заказа)
        public IActionResult Create(int? carId)
        {
            ViewData["CarID"] = new SelectList(_context.Cars, "CarID", "Make", carId);
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderDate = DateTime.Now;
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // переход на страницу подтверждения
                return RedirectToAction(nameof(Confirm), new { id = order.OrderID });
            }

            ViewData["CarID"] = new SelectList(_context.Cars, "CarID", "Make", order.CarID);
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", order.CustomerID);
            return View(order);
        }

        // GET: Orders/Confirm/id (страница подтверждения)
        public async Task<IActionResult> Confirm(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Car)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}
