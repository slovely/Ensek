using System.Transactions;
using Ensek.Api.Data;
using Ensek.Api.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ensek.Api.Endpoints.MeterReadings;

public class MeterReadingsController : ControllerBase
{
    private readonly IMeterReadingParser _parser;
    private readonly EnsekDbContext _context;

    public MeterReadingsController(IMeterReadingParser parser, EnsekDbContext context)
    {
        _parser = parser;
        _context = context;
    }

    [HttpPost("/meter-reading-uploads")]
    public async Task<IActionResult> Post(MeterReadingRequest request)
    {
        if (request?.File == null)
            return BadRequest();
        
        // Depending on expected file input sizes, we may want to store the file and process asynchronously,
        // rather than having to process in-line and returning success/failure to the caller.

        var result = new MeterReadingResponse();
        var meterReadings = _parser.Parse(request.File.OpenReadStream());
        using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        foreach (var meterReading in meterReadings)
        {
            if (!meterReading.IsValid)
            {
                result.FailedCount++;
                continue;
            }
            
            var account = _context.Accounts
                .FirstOrDefault(x => x.AccountId == meterReading.AccountId);
            if (account == null)
            {
                result.FailedCount++;
                continue;
            }
            
            // Prevent same entry (and prevents earlier dates) (based on assumption
            // that 'same entry' means a reading on the same day)
            var existingReadingsAfterDate = _context
                .MeterReadings
                .Where(x => x.AccountId == meterReading.AccountId
                            && x.Date.Date >= meterReading.MeterReadingDateTime.Date);

            if (existingReadingsAfterDate.Any())
            {
                result.FailedCount++;
                continue;
            }

            _context.MeterReadings.Add(new MeterReading
            {
                Account = account,
                AccountId = account.AccountId,
                Date = meterReading.MeterReadingDateTime.ToUniversalTime(),
                Value = meterReading.MeterReadValue,
            });
            result.SuccessfulCount++;
            await _context.SaveChangesAsync();
        }
        tx.Complete();

        return Ok(result);
    }

    [HttpGet("/all-readings")]
    public async Task<IActionResult> Get()
    {
        var meterReadings = await _context.MeterReadings
            .Include(x => x.Account)
            .OrderBy(x => x.AccountId)
            .ThenBy(x => x.Date)
            .ToListAsync();

        return Ok(meterReadings.Select(x => new
            {
                x.AccountId,
                x.Account.FullName,
                x.Date,
                x.Value,
            }
        ));
    }
}

public class MeterReadingResponse
{
    public int SuccessfulCount { get; set; }
    public int FailedCount { get; set; }
}

public class MeterReadingRequest
{
    public IFormFile? File { get; set; }
}