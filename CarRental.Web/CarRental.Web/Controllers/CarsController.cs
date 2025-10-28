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

			var car = await _context.Cars
				.FirstOrDefaultAsync(m => m.CarID == id);
			if (car == null)
				return NotFound();

			return View(car);
		}

		// GET: Cars/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Cars/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Car car, IFormFile? photoFile)
		{
			if (photoFile != null && photoFile.Length > 0)
			{
				using var ms = new MemoryStream();
				await photoFile.CopyToAsync(ms);
				car.Photo = ms.ToArray();
			}

			if (ModelState.IsValid)
			{
				_context.Add(car);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			return View(car);
		}

		// GET: Cars/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
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
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!_context.Cars.Any(e => e.CarID == car.CarID))
						return NotFound();
					else
						throw;
				}
				return RedirectToAction(nameof(Index));
			}

			return View(car);
		}

		// GET: Cars/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
				return NotFound();

			var car = await _context.Cars
				.FirstOrDefaultAsync(m => m.CarID == id);
			if (car == null)
				return NotFound();

			return View(car);
		}

		// POST: Cars/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
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
