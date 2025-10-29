using CarRental.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Web.Controllers
{
    public class CarsController : Controller
    {
        private readonly CarRentalDbContext _context;

        public CarsController(CarRentalDbContext context)
        {
            _context = context;
        }

        // GET: Cars
        public IActionResult Index()
        {
            return View(_context.Cars.ToList());
        }

        // GET: Cars/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var car = _context.Cars.FirstOrDefault(m => m.CarID == id);
            if (car == null) return NotFound();

            return View(car);
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("IsAdmin") == "true";
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Index");
            return View();
        }

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
            if (!IsAdmin()) return RedirectToAction("Index");
            if (id == null) return NotFound();

            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();

            return View(car);
        }

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
        public IActionResult Delete(int? id)
        {
            if (!IsAdmin()) return RedirectToAction("Index");
            if (id == null) return NotFound();

            var car = _context.Cars.FirstOrDefault(m => m.CarID == id);
            if (car == null) return NotFound();

            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index");

            var car = _context.Cars.Find(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
