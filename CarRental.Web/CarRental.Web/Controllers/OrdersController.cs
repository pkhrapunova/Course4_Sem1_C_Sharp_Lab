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

        // =================== СПИСОК ===================
        public async Task<IActionResult> Index()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            bool isAdmin = HttpContext.Session.GetString("IsAdmin") == "true";

            if (customerId == null)
                return RedirectToAction("Login", "Account");

            var query = _context.Orders
                .Include(o => o.Car)
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .AsQueryable();

            if (!isAdmin)
                query = query.Where(o => o.CustomerID == customerId);

            return View(await query.ToListAsync());
        }

        // =================== СОЗДАНИЕ ===================
        public IActionResult Create()
        {
            bool isAdmin = HttpContext.Session.GetString("IsAdmin") == "true";

            var cars = _context.Cars
                .Where(c => c.Status != "Занята")
                .Select(c => new
                {
                    c.CarID,
                    Display = $"{c.Make} ({c.CarNumber}) — {c.PricePerHour}₽/ч"
                })
                .ToList();

            ViewData["CarID"] = new SelectList(cars, "CarID", "Display");

            if (isAdmin)
            {
                ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            try
            {
                Console.WriteLine("POST Create вызван");
                Console.WriteLine($"Полученные данные: CarID={order.CarID}, CustomerID={order.CustomerID}, Hours={order.Hours}, OrderDate={order.OrderDate}");

                // Для обычного пользователя подставляем CustomerID
                if (HttpContext.Session.GetString("IsAdmin") != "true")
                {
                    order.CustomerID = HttpContext.Session.GetInt32("CustomerID") ?? 0;
                    Console.WriteLine($"Присвоен CustomerID={order.CustomerID} из сессии");
                }

                if (!ModelState.IsValid)
                {
                    Console.WriteLine("ModelState невалиден");
                    foreach (var key in ModelState.Keys)
                    {
                        var state = ModelState[key];
                        foreach (var error in state.Errors)
                        {
                            Console.WriteLine($"Ошибка поля {key}: {error.ErrorMessage}");
                        }
                    }

                    // Восстановление select-листов
                    var cars = _context.Cars
                        .Where(c => c.Status != "Занята")
                        .Select(c => new { c.CarID, Display = $"{c.Make} ({c.CarNumber}) — {c.PricePerHour}₽/ч" })
                        .ToList();
                    ViewData["CarID"] = new SelectList(cars, "CarID", "Display", order.CarID);

                    if (HttpContext.Session.GetString("IsAdmin") == "true")
                    {
                        ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", order.CustomerID);
                    }

                    return View(order);
                }

                // Добавляем заказ
                order.OrderDate = order.OrderDate == default ? DateTime.Now : order.OrderDate;
                Console.WriteLine($"OrderDate установлена: {order.OrderDate}");

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Заказ добавлен с ID={order.OrderID}");

                return RedirectToAction("Confirm", new { id = order.OrderID });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Исключение: {ex.Message}");
                Console.WriteLine(ex.StackTrace);

                return View(order); // Возвращаем форму с текущими данными
            }
        }


        // =================== ПОДТВЕРЖДЕНИЕ ===================
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
