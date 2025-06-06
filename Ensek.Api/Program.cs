using Ensek.Api;
using Ensek.Api.Data;
using Ensek.Api.Endpoints.MeterReadings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IMeterReadingParser, SepMeterReadingParser>();

builder.Services.AddDbContext<EnsekDbContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("ensek-db"))
        .UseSnakeCaseNamingConvention();
});

var app = builder.Build();

app.SetupDatabase();

app.MapControllers();
app.MapRazorPages();

app.Run();