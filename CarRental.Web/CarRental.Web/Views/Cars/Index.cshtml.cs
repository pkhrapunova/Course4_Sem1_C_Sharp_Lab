using CarRental.Web.Models;
using CarRental.Web.Models.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class IndexModel : PageModel
{
	private readonly CarRentalDbContext _context;
	public IndexModel(CarRentalDbContext context) => _context = context;

	public IList<Car> Cars { get; set; } = default!;

	public async Task OnGetAsync()
	{
		Cars = await _context.Cars.ToListAsync();
	}
}
