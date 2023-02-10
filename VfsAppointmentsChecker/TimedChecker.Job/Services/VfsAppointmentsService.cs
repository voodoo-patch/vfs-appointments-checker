using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TimedChecker.Job.Configuration;

namespace TimedChecker.Job.Services;

public class VfsAppointmentsService : IAppointmentsService
{
    private readonly VfsSettings _settings;
    private IPlaywright _playwright;

    public VfsAppointmentsService(IOptions<VfsSettings> settings)
    {
        _settings = settings.Value;
    }

    private async Task<IBrowser> GetBrowser()
    {
        _playwright = await Playwright.CreateAsync();
        var browser = await _playwright.Chromium.LaunchAsync(new()
        {
            Headless = false
        });
        return browser;
    }

    public async Task<(bool, IDictionary<string, string>)> GetSlots()
    {
        var slots = new Dictionary<string, string>();
        var browser = await GetBrowser();
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        await Authenticate(page);

        await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Start New Booking" }).ClickAsync();

        await SelectLondonTouristVisa(page);
        slots.Add(VfsSettings.London, await GetSlotsFromPage(page));

        await SelectManchesterTouristVisa(page);
        slots.Add(VfsSettings.Manchester, await GetSlotsFromPage(page));

        await browser.DisposeAsync();
        _playwright.Dispose();

        bool found = IsAppointmentAvailable(slots);
        return (found, slots);
    }

    private bool IsAppointmentAvailable(Dictionary<string, string> slots) =>
        slots.Values.Any(_ => !_.Contains("No appointment"));

    private async Task<string> GetSlotsFromPage(IPage page)
    {
        var text = await page.Locator(".alert.alert-info").TextContentAsync();
        return text;
    }

    private static async Task SelectManchesterTouristVisa(IPage page)
    {
        await page.GetByText("Italy Visa Application Centre, Manchester").ClickAsync();

        await page.Locator("#mat-select-value-3").ClickAsync();

        await page.GetByText("Italy UK VisaCategory").ClickAsync();

        await page.Locator("#mat-select-value-5").ClickAsync();

        await page.GetByText("Tourist", new PageGetByTextOptions { Exact = true }).ClickAsync();
    }

    private static async Task SelectLondonTouristVisa(IPage page)
    {
        await page.Locator("#mat-select-value-1").ClickAsync();

        await page.GetByText("Italy Visa Application Centre, London").ClickAsync();
        
        await page.Locator("div")
            .Filter(new LocatorFilterOptions { HasText = "Select your appointment category" })
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
        await page.GotoAsync(_settings.Urls.Authentication);

        await page.GetByPlaceholder("jane.doe@email.com").FillAsync(_settings.Account.Email);

        await page.GetByPlaceholder("**********").FillAsync(_settings.Account.Password);

        await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Sign In" }).ClickAsync();
    }
}