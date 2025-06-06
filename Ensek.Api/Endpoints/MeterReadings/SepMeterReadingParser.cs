using System.Globalization;
using nietras.SeparatedValues;

namespace Ensek.Api.Endpoints.MeterReadings;

public interface IMeterReadingParser
{
    IEnumerable<MeterReadingInput> Parse(Stream stream);
}

public class SepMeterReadingParser : IMeterReadingParser
{
    public IEnumerable<MeterReadingInput> Parse(Stream stream)
    {
        using var reader = Sep.Reader().From(stream);
        foreach (var row in reader)
        {
            var accountId = row["AccountId"].TryParse<int>();
            var meterReading = row["MeterReadValue"].TryParse<int>();
            var meterReadingDateStr = row["MeterReadingDateTime"].Parse<string>();
            var dateIsValid = DateTimeOffset.TryParseExact(meterReadingDateStr, "dd/MM/yyyy HH:mm", 
                null, DateTimeStyles.AssumeLocal, out var meterReadingDateParsed);

            var lineNumber = row.LineNumberFrom;
            if (!accountId.HasValue || accountId <= 0 || !meterReading.HasValue || !dateIsValid || meterReading < 0 || meterReading > 99999)
                yield return new MeterReadingInput(lineNumber, false, accountId ?? 0, dateIsValid ? meterReadingDateParsed : DateTimeOffset.MinValue, meterReading ?? 0);
            else 
                yield return new MeterReadingInput(lineNumber, true, accountId ?? 0, meterReadingDateParsed, meterReading ?? 0);
        }
    }
}

public record MeterReadingInput(int LineNumber, bool IsValid, int AccountId, DateTimeOffset MeterReadingDateTime, int MeterReadValue);