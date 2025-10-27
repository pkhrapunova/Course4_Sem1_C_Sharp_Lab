using CarRental.Web.Models;
using CarRental.Web.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CarsController : Controller
{
	private readonly CarRentalDbContext _context;

	public CarsController(CarRentalDbContext context)
	{
		_context = context;
	}

	public async Task<IActionResult> Index()
	{
		var cars = await _context.Cars.ToListAsync();
		return View(cars);
	}

	public IActionResult Create()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Create(Car car)
	{
		if (!ModelState.IsValid) return View(car);

		_context.Cars.Add(car);
		await _context.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Edit(int id)
	{
		var car = await _context.Cars.FindAsync(id);
		if (car == null) return NotFound();
		return View(car);
	}

	[HttpPost]
	public async Task<IActionResult> Edit(Car car)
	{
		if (!ModelState.IsValid) return View(car);

		_context.Update(car);
		await _context.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Delete(int id)
	{
		var car = await _context.Cars.FindAsync(id);
		if (car == null) return NotFound();
		return View(car);
	}

	[HttpPost]
	public async Task<IActionResult> DeleteConfirmed(int carID)
	{
		var car = await _context.Cars.FindAsync(carID);
		if (car != null)
		{
			_context.Cars.Remove(car);
			await _context.SaveChangesAsync();
		}
		return RedirectToAction(nameof(Index));
	}
}
