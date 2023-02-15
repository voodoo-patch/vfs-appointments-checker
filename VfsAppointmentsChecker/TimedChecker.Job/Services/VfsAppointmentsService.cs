using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TimedChecker.Job.Configuration;

namespace TimedChecker.Job.Services;

public class VfsAppointmentsService : IAppointmentsService
{
    private readonly JobSettings _jobSettings;
    private readonly VfsSettings _vfsSettings;
    private IPlaywright? _playwright;

    public VfsAppointmentsService(IOptions<VfsSettings> vfsSettings, IOptions<JobSettings> jobSettings)
    {
        _jobSettings = jobSettings.Value;
        _vfsSettings = vfsSettings.Value;
    }

    private async Task<IBrowser> GetBrowser()
    {
        _playwright = await Playwright.CreateAsync();
        var browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = _jobSettings.Headless
        });
        return browser;
    }

    public async Task<(bool, IDictionary<string, string>)> GetSlots()
    {
        var slots = new Dictionary<string, string>();
        var browser = await GetBrowser();
        var desktop = _playwright?.Devices["Desktop Chrome"];
        var context = await browser.NewContextAsync(desktop);
        var page = await context.NewPageAsync();

        await Authenticate(page);

        await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions {Name = "Start New Booking"}).ClickAsync();

        await SelectLondonTouristVisa(page);
        slots.Add(VfsSettings.London, await GetSlotsFromPage(page));

        await SelectManchesterTouristVisa(page);
        slots.Add(VfsSettings.Manchester, await GetSlotsFromPage(page));

        await context.DisposeAsync();
        await browser.DisposeAsync();
        _playwright?.Dispose();

        var found = IsAppointmentAvailable(slots);
        return (found, slots);
    }

    private bool IsAppointmentAvailable(Dictionary<string, string> slots)
    {
        return slots.Values.Any(_ => !_.Contains("No appointment"));
    }

    private async Task<string> GetSlotsFromPage(IPage page)
    {
        var text = await page.Locator(".alert.alert-info").TextContentAsync();
        return text ?? string.Empty;
    }

    private static async Task SelectManchesterTouristVisa(IPage page)
    {
        await page.GetByText("Italy Visa Application Centre, Manchester").ClickAsync();

        await page.Locator("#mat-select-value-3").ClickAsync();

        await page.GetByText("Italy UK VisaCategory").ClickAsync();

        await page.Locator("#mat-select-value-5").ClickAsync();

        await page.GetByText("Tourist", new PageGetByTextOptions {Exact = true}).ClickAsync();
    }

    private static async Task SelectLondonTouristVisa(IPage page)
    {
        await page.Locator("#mat-select-value-1").ClickAsync();

        await page.GetByText("Italy Visa Application Centre, London").ClickAsync();

        await page.Locator("div")
            .Filter(new LocatorFilterOptions {HasText = "Select your appointment category"})
            .Nth(2)
            .ClickAsync();

        await page.GetByText("Italy UK VisaCategory").ClickAsync();

        await page.Locator("div")
            .Filter(new LocatorFilterOptions
            {
                HasText = "Italy Visa Application Centre, London"
            })
            .Nth(2)
            .ClickAsync();
    }

    private async Task Authenticate(IPage page)
    {
        await page.GotoAsync(_vfsSettings.Urls.Authentication);

        await page.Locator("input[formcontrolname=\"username\"]").FillAsync(_vfsSettings.Account.Email);
        //await page.GetByPlaceholder("jane.doe@email.com").FillAsync(_vfsSettings.Account.Email);

        await page.Locator("input[formcontrolname=\"password\"]").FillAsync(_vfsSettings.Account.Password);
        //await page.GetByPlaceholder("**********").FillAsync(_vfsSettings.Account.Password);

        await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions {Name = "Sign In"}).ClickAsync();
    }
}