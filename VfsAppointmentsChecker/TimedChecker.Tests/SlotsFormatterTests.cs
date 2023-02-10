using TimedChecker.Job.Services;

namespace TimedChecker.Tests;
public class SlotsFormatterTests
{
    private readonly SlotsFormatter _sut;
    private readonly IDictionary<string, IEnumerable<string>> _validSlots = new Dictionary<string, IEnumerable<string>>()
    {
        { "LONDON", new List<string> { "14:00", "15:30"} },
        { "MANCHESTER", new List<string> { "10:15"} }
    };

    private readonly string _expectedString = $"Found 3 new appointments:\nLONDON: 14:00, 15:30\nMANCHESTER: 10:15";

    public SlotsFormatterTests()
    {
        _sut = new SlotsFormatter();
    }

    [Fact]
    public void Format_GivenValidSlots_ReturnsString()
    {
        var result = _sut.Format(_validSlots);

        result.Should().NotBeEmpty().And.BeEquivalentTo(_expectedString);
    }
}
