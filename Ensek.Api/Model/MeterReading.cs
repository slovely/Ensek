namespace Ensek.Api.Model;

public class MeterReading
{
    public int MeterReadingId { get; set; }
    public Account Account { get; set; }
    public int AccountId { get; set; }
    public DateTimeOffset Date { get; set; }
    public int Value { get; set; }
}