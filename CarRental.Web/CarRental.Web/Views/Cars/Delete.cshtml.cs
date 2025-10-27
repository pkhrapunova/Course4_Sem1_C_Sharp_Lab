using CarRental.Web.Models;
using CarRental.Web.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class DeleteModel : PageModel
{
	private readonly CarRentalDbContext _context;
	public DeleteModel(CarRentalDbContext context) => _context = context;

	[BindProperty]
	public Car Car { get; set; } = default!;

	public async Task<IActionResult> OnGetAsync(int id)
	{
		Car = await _context.Cars.FindAsync(id);
		if (Car == null) return RedirectToPage("Index");
		return Page();
	}

	public async Task<IActionResult> OnPostAsync(int id)
	{
		var car = await _context.Cars.FindAsync(id);
		if (car != null)
		{
			_context.Cars.Remove(car);
			await _context.SaveChangesAsync();
		}
		return RedirectToPage("Index");
	}
}
