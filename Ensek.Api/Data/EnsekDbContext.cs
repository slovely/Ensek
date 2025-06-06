using Ensek.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Ensek.Api.Data;

public class EnsekDbContext : DbContext
{
    public EnsekDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<MeterReading> MeterReadings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var accountBuilder = modelBuilder.Entity<Account>();
        accountBuilder.HasKey(x => x.AccountId);
        accountBuilder.Property(x => x.AccountId).UseIdentityColumn();
        accountBuilder.Property(x => x.FirstName).HasMaxLength(100);
        accountBuilder.Property(x => x.LastName).HasMaxLength(100);

        var readingBuilder = modelBuilder.Entity<MeterReading>();
        readingBuilder.HasKey(x => x.MeterReadingId);
        readingBuilder.Property(x => x.MeterReadingId).UseIdentityColumn();
        readingBuilder.Property(x => x.Value).IsRequired();
        readingBuilder.Property(x => x.Date).IsRequired();
    }
}