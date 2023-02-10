using Microsoft.Extensions.Options;
using TimedChecker.Job.Configuration;
using TimedChecker.Job.Services;

namespace TimedChecker.Tests;
public class VfsAppointmentsServiceTests
{
    private readonly IAppointmentsService _sut;
    private readonly string _validEmail = "email";
    private readonly string _validPassword = "password";
    private readonly string _center1 = "center-1";
    private readonly IEnumerable<string> _center1Slots = new List<string> { "10:00", "11:00" };
    private readonly string _center2 = "center-2";
    private readonly IEnumerable<string> _center2Slots = new List<string> { "12:00", "13:00" };

    public VfsAppointmentsServiceTests()
    {
        var vfsSettingsMock = GetVfsSettingsMock();
        _sut = new VfsAppointmentsService(vfsSettingsMock.Object);
    }

    private Mock<IOptions<VfsSettings>> GetVfsSettingsMock()
    {
        Mock<IOptions<VfsSettings>> configurationMock = new Mock<IOptions<VfsSettings>>();
        configurationMock.Setup(_ => _.Value)
            .Returns(() => new VfsSettings()
            {
                Account = new VfsSettings.AccountSettings()
                {
                    Email = _validEmail,
                    Password = _validPassword
                }
            });
        return configurationMock;
    }


    [Fact]
    public void GetSlots_GivenAvailableSlots_ExpectsSlots()
    {
        var result = _sut.GetSlots().Result;
        result.Should().NotBeNull()
            .And
            .HaveCount(2)
            .And
            .ContainKeys(_center1, _center2)
            .And
            .Contain(new KeyValuePair<string, IEnumerable<string>>(_center1, _center1Slots),
                new KeyValuePair<string, IEnumerable<string>>(_center2, _center2Slots));
    }

    [Fact]
    public void GetSlots_GivenNoSlots_ExpectsEmpty()
    {
        var result = _sut.GetSlots().Result;
        result.Should().BeEmpty();
    }
}
