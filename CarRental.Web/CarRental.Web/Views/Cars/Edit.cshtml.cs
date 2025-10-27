using CarRental.Web.Models;
using CarRental.Web.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class EditModel : PageModel
{
	private readonly CarRentalDbContext _context;
	public EditModel(CarRentalDbContext context) => _context = context;

	[BindProperty]
	public Car Car { get; set; } = default!;

	public async Task<IActionResult> OnGetAsync(int id)
	{
		Car = await _context.Cars.FindAsync(id);
		if (Car == null) return RedirectToPage("Index");
		return Page();
	}

	public async Task<IActionResult> OnPostAsync()
	{
		if (!ModelState.IsValid) return Page();
		_context.Attach(Car).State = EntityState.Modified;
		await _context.SaveChangesAsync();
		return RedirectToPage("Index");
	}
}
