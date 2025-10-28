using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRental.Web.Models;
using System.Threading.Tasks;
using System.Linq;

namespace CarRental.Web.Controllers
{
    public class CarsController : Controller
    {
        private readonly CarRentalDbContext _context;

        public CarsController(CarRentalDbContext context)
        {
            _context = context;
        }

        // Метод проверки: является ли текущий пользователь админом
        private bool IsAdmin()
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            return isAdmin == "true";
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var cars = await _context.Cars.ToListAsync();
            return View(cars);
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _context.Cars.FirstOrDefaultAsync(m => m.CarID == id);
            if (car == null)
                return NotFound();

            return View(car);
        }

        // GET: Cars/Create (только админ)
        public IActionResult Create()
        {
            if (!IsAdmin())
                return Forbid(); // запрет доступа обычным пользователям

            return View();
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car, IFormFile? photoFile)
        {
            if (!IsAdmin())
                return Forbid();

            if (ModelState.IsValid)
            {
                if (photoFile != null && photoFile.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await photoFile.CopyToAsync(ms);
                    car.Photo = ms.ToArray();
                }

                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsAdmin())
                return Forbid();

            if (id == null)
                return NotFound();

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
                return NotFound();

            return View(car);
        }

        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Car car, IFormFile? photoFile)
        {
            if (!IsAdmin())
                return Forbid();

            if (id != car.CarID)
                return NotFound();

            if (photoFile != null && photoFile.Length > 0)
            {
                using var ms = new MemoryStream();
                await photoFile.CopyToAsync(ms);
                car.Photo = ms.ToArray();
            }
            else
            {
                var existingCar = await _context.Cars.AsNoTracking().FirstOrDefaultAsync(c => c.CarID == id);
                car.Photo = existingCar?.Photo;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Cars.Any(e => e.CarID == car.CarID))
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsAdmin())
                return Forbid();

            if (id == null)
                return NotFound();

            var car = await _context.Cars.FirstOrDefaultAsync(m => m.CarID == id);
            if (car == null)
                return NotFound();

            return View(car);
        }

        // POST: Cars/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
                return Forbid();

            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
