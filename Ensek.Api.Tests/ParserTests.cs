using Ensek.Api.Endpoints.MeterReadings;

namespace Ensek.Api.Tests;

public class ParserTests
{
    [Test]
    public void EnsureSingleValidRowCanBeParsed()
    {
        var parser = new SepMeterReadingParser();

        var input = @"AccountId,MeterReadingDateTime,MeterReadValue
1,22/02/2025 12:00,12345
";
        var results = parser.Parse(BuildStreamFromString(input)).ToArray();

        Assert.That(results.Length, Is.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(results.First().AccountId, Is.EqualTo(1));
            Assert.That(results.First().MeterReadingDateTime, Is.EqualTo(new DateTimeOffset(2025, 2, 22, 12, 0, 0, TimeSpan.Zero)));
            Assert.That(results.First().MeterReadValue, Is.EqualTo(12345));
            Assert.That(results.First().IsValid, Is.True);
        });
    }

    [Test]
    public void TestInvalidAccountId()
    {
        var parser = new SepMeterReadingParser();

        var input = @"AccountId,MeterReadingDateTime,MeterReadValue
ABC,22/02/2025 12:00,12345
-9,22/02/2025 12:00,12345
";
        var results = parser.Parse(BuildStreamFromString(input)).ToArray();

        Assert.That(results.First().IsValid, Is.False);
        Assert.That(results.ElementAt(1).IsValid, Is.False);
    }

    [Test]
    public void TestInvalidReading()
    {
        var parser = new SepMeterReadingParser();

        var input = @"AccountId,MeterReadingDateTime,MeterReadValue
1,22/02/2025 12:00,100000
2,22/02/2025 12:00,-1
";
        var results = parser.Parse(BuildStreamFromString(input)).ToArray();

        Assert.That(results.First().IsValid, Is.False);
        Assert.That(results.ElementAt(1).IsValid, Is.False);
    }

    private Stream BuildStreamFromString(string input)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(input);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}