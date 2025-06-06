using Ensek.Api;
using Ensek.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.AddServiceDefaults();

builder.Services.AddDbContext<EnsekDbContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("ensek-db"))
        .UseSnakeCaseNamingConvention();
});

var app = builder.Build();

app.SetupDatabase();

app.MapGet("/", () => "Hello World!");

app.Run();