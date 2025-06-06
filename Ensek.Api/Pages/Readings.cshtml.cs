using Ensek.Api.Data;
using Ensek.Api.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Ensek.Api.Pages;

public class Readings : PageModel
{
    private readonly EnsekDbContext _context;
    public List<MeterReading> MeterReadings;

    public Readings(EnsekDbContext context)
    {
        _context = context;
    }

    public async Task OnGetAsync()
    {
        MeterReadings = await _context.MeterReadings
            .Include(x => x.Account)
            .OrderBy(x => x.AccountId)
            .ThenBy(x => x.Date)
            .ToListAsync();
 
    }
}