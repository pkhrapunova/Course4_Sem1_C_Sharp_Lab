using CarRental.Web.Models;
using CarRental.Web.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class CreateModel : PageModel
{
	private readonly CarRentalDbContext _context;
	public CreateModel(CarRentalDbContext context) => _context = context;

	[BindProperty]
	public Car Car { get; set; } = new Car();

	public void OnGet() { }

	public async Task<IActionResult> OnPostAsync()
	{
		if (!ModelState.IsValid) return Page();

		_context.Cars.Add(Car);
		await _context.SaveChangesAsync();
		return RedirectToPage("Index");
	}
}
