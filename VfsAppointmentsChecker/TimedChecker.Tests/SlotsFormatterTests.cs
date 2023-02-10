using TimedChecker.Job.Services;

namespace TimedChecker.Tests;
public class SlotsFormatterTests
{
    private readonly SlotsFormatter _sut;
    private static readonly IDictionary<string, string> _slots = new Dictionary<string, string>()
    {
        { "LONDON", "Earliest appointment found on 1/1/1970 at 14:00" },
        { "MANCHESTER", "Earliest appointment found on 1/1/1970 at 10:15" }
    };
    private readonly string _expectedString = $":tada: **Found new appointments!!**:\n" +
                                                    $"{_slots.ElementAt(0).Key}: {_slots.ElementAt(0).Value}\n" +
                                                    $"{_slots.ElementAt(1).Key}: {_slots.ElementAt(1).Value}";

    public SlotsFormatterTests()
    {
        _sut = new SlotsFormatter();
    }

    [Fact]
    public void Format_GivenValidSlots_ReturnsString()
    {
        var result = _sut.Format(_slots);

        result.Should().NotBeEmpty().And.BeEquivalentTo(_expectedString);
    }
}
