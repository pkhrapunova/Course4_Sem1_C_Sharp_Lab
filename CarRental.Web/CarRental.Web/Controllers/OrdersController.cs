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

        // GET: Orders (для администратора)
        public async Task<IActionResult> Index()
        {
            var orders = _context.Orders
                .Include(o => o.Car)
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate);

            return View(await orders.ToListAsync());
        }

        // GET: Orders/Create
        // форма оформления заказа (пользователь выбирает машину)
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
                _context.Add(order);
                await _context.SaveChangesAsync();

                // при желании можно обновить статус машины на "Забронирована"
                var car = await _context.Cars.FindAsync(order.CarID);
                if (car != null)
                {
                    car.Status = "Забронирована";
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index", "Cars");
            }

            ViewData["CarID"] = new SelectList(_context.Cars, "CarID", "Make", order.CarID);
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", order.CustomerID);
            return View(order);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.Car)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.OrderID == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            ViewData["CarID"] = new SelectList(_context.Cars, "CarID", "Make", order.CarID);
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", order.CustomerID);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.OrderID) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CarID"] = new SelectList(_context.Cars, "CarID", "Make", order.CarID);
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", order.CustomerID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.Car)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.OrderID == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
