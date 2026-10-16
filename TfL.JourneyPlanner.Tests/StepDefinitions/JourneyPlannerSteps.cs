
using Microsoft.Playwright;
using Reqnroll;

namespace TfL.JourneyPlanner.Tests.StepDefinitions;

[Binding]
public class JourneyPlannerSteps
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IPage? _page;

    [BeforeScenario]
    public async Task StartBrowser()
    {
        _playwright = await Playwright.CreateAsync();

        _browser = await _playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions
            {
                Headless = false
            });

        _page = await _browser.NewPageAsync();
    }

    [AfterScenario]
    public async Task CloseBrowser()
    {
        if (_browser is not null)
        {
            await _browser.CloseAsync();
        }

        _playwright?.Dispose();
    }

    [Given("I open the TfL Journey Planner")]
    public async Task GivenIOpenTheTfLJourneyPlanner()
    {
        await Page.GotoAsync("https://tfl.gov.uk/plan-a-journey");

        var cookieOverlay = Page.Locator("#cb-cookieoverlay");

        if (await cookieOverlay.IsVisibleAsync())
        {
            await Page.Locator("#cb-btn-manage").ClickAsync();

            await Page.Locator("#cb-btn-save").ClickAsync();

            await Assertions.Expect(cookieOverlay).ToBeHiddenAsync();
        }
    }
    [When("I select {string} as the starting point")]

    public async Task WhenISelectStartingPoint(string station)
    {
        await Page.Locator("#InputFrom")
          .FillAsync(station);

        await Page.Locator(".tt-suggestion .stop-name")
          .GetByText(station, new() { Exact = true })
          .ClickAsync();
    }

    [When("I select {string} as the destination")]
    public async Task WhenISelectDestination(string station)
    {
        await Page.Locator("#InputTo").FillAsync(station);

        await Page.GetByRole(AriaRole.Option, new()
        {
            Name = station,
            Exact = true
        }).ClickAsync();
    }

    [When("I plan the journey")]
    public async Task WhenIPlanTheJourney()
    {
        // Move focus away to close the autocomplete dropdown
        await Page.Locator("#InputTo").PressAsync("Tab");

        // Click Plan my journey
        await Page.Locator("#plan-journey-button").ClickAsync();
    }

    [Then("I should see journey results")]
    public async Task ThenIShouldSeeJourneyResults()
    {
        await Assertions.Expect(
            Page.Locator(".jp-results-headline")
        ).ToHaveTextAsync("Journey results");
    }
    [When("I select the walking travel option")]
    public async Task WhenISelectTheWalkingTravelOption()
    {
        await Assertions.Expect(
            Page.Locator("a.journey-box.walking")
        ).ToBeVisibleAsync();
    }

    [Then("I should see the walking journey time")]
    public async Task ThenIShouldSeeTheWalkingJourneyTime()
    {
        var walkingTime = Page.Locator(
            "a.journey-box.walking .journey-info strong"
        );

        await Assertions.Expect(walkingTime).ToBeVisibleAsync();

        var timeText = await walkingTime.InnerTextAsync();

        Assert.True(
            int.TryParse(timeText.Trim(), out int minutes) && minutes > 0,
            $"Invalid walking journey time: {timeText}"
        );
    }
    [When("I select the cycling travel option")]
    public async Task WhenISelectTheCyclingTravelOption()
    {
        await Assertions.Expect(
            Page.Locator("a.journey-box.cycling")
        ).ToBeVisibleAsync();
    }

    [Then("I should see the cycling journey time")]
    public async Task ThenIShouldSeeTheCyclingJourneyTime()
    {
        var cyclingTime = Page.Locator(
            "a.journey-box.cycling .journey-info strong"
        );

        await Assertions.Expect(cyclingTime).ToBeVisibleAsync();

        var timeText = await cyclingTime.InnerTextAsync();

        Assert.True(
            int.TryParse(timeText.Trim(), out int minutes) && minutes > 0,
            $"Invalid cycling journey time: {timeText}"
        );
    }

    [When("I select routes with the least walking")]
    public async Task WhenISelectRoutesWithTheLeastWalking()
    {
        await Page.Locator(
            "label[for='JourneyPreference_2']"
        ).ClickAsync();

        await Assertions.Expect(
            Page.Locator("#JourneyPreference_2")
        ).ToBeCheckedAsync();
    }


    [When("I edit the journey preferences")]
    public async Task WhenIEditTheJourneyPreferences()
    {
        await Page.GetByText("Edit preferences", new()
        {
            Exact = true
        }).ClickAsync();

        await Assertions.Expect(
            Page.Locator("#JourneyPreference_2")
        ).ToBeVisibleAsync();
    }



    [When("I update the journey")]
    public async Task WhenIUpdateTheJourney()
    {
        await Page.Locator(
            "input.plan-journey-button[value='Update journey']:visible"
        ).ClickAsync();

        await Assertions.Expect(
            Page.Locator(".jp-results-headline")
        ).ToHaveTextAsync("Journey results");
    }

    [Then("I should see updated journey results")]
    public async Task ThenIShouldSeeUpdatedJourneyResults()
    {
        await Assertions.Expect(
            Page.Locator(".jp-results-headline")
        ).ToHaveTextAsync("Journey results");
    }


    [Then("I should see the journey time")]
    public async Task ThenIShouldSeeTheJourneyTime()
    {
        await Assertions.Expect(
            Page.GetByText("Showing routes with least walking")
        ).ToBeVisibleAsync();

        var journeyOption = Page.GetByRole(
            AriaRole.Heading,
            new()
            {
                NameRegex = new System.Text.RegularExpressions.Regex(
                @"^Option 1:.*\d+\s*mins"
            )
            }
        );

        await Assertions.Expect(journeyOption).ToBeVisibleAsync();

        var headingText = await journeyOption.InnerTextAsync();

        Assert.Matches(@"\d+\s*mins", headingText);
    }

    [When("I open the journey details")]
    public async Task WhenIOpenTheJourneyDetails()
    {
        var firstJourney = Page.GetByRole(
            AriaRole.Button,
            new()
            {
                NameRegex = new System.Text.RegularExpressions.Regex(
                @"^Option 1:"
            )
            }
        );

        await Assertions.Expect(firstJourney).ToBeVisibleAsync();

        if (await firstJourney.GetAttributeAsync("aria-expanded") != "true")
        {
            await firstJourney.ClickAsync();
        }

        await Assertions.Expect(
            Page.GetByRole(AriaRole.Heading, new()
            {
                Name = "Detailed steps for option 1"
            })
        ).ToBeVisibleAsync();
    }


    [Then("I should see complete access information for {string}")]
    public async Task ThenIShouldSeeCompleteAccessInformationFor(
        string station)
    {
        // Verify the destination station appears
        await Assertions.Expect(
            Page.GetByText(station, new() { Exact = false }).First
        ).ToBeVisibleAsync();


        var accessElements = Page.Locator(
            "summary, [class*='access'], [class*='station-information']"
        );

        var accessTexts = await accessElements.AllTextContentsAsync();

        await System.IO.File.WriteAllTextAsync(
            "accessibility-debug.txt",
            "ELEMENTS FOUND: " + await accessElements.CountAsync() +
            Environment.NewLine +
            "ACCESSIBILITY TEXT: " +
            string.Join(" | ", accessTexts)
        );


        // Find and open the station accessibility section
        var accessSection = Page.Locator(
            "#option-1-station-information-heading"
        );
        await Assertions.Expect(accessSection).ToBeVisibleAsync();
        await accessSection.ClickAsync();
        // Check the accessibility section has expanded

        await Assertions.Expect(
            accessSection.Locator("xpath=..")
        ).ToHaveAttributeAsync("open", "");

    }


    [When("I enter an invalid starting point")]
    public async Task WhenIEnterAnInvalidStartingPoint()
    {
        await Page.Locator("#InputFrom")
            .FillAsync("InvalidStationXYZ123");
    }

    [When("I enter an invalid destination")]
    public async Task WhenIEnterAnInvalidDestination()
    {
        await Page.Locator("#InputTo")
            .FillAsync("UnknownDestinationXYZ456");
    }



    [Then("I should see no journey results")]
    public async Task ThenIShouldSeeNoJourneyResults()
    {
        await Assertions.Expect(
            Page.Locator(".jp-results-headline")
        ).ToBeVisibleAsync();

        await Assertions.Expect(
            Page.Locator("a.journey-box.walking")
        ).ToHaveCountAsync(0);

        await Assertions.Expect(
            Page.Locator("a.journey-box.cycling")
        ).ToHaveCountAsync(0);

        await Assertions.Expect(
            Page.GetByRole(AriaRole.Heading, new()
            {
                NameRegex = new System.Text.RegularExpressions.Regex(
                    @"^Option \d+:"
                )
            })
        ).ToHaveCountAsync(0);
    }

    [When("I leave the starting point empty")]
    public async Task WhenILeaveTheStartingPointEmpty()
    {
        await Page.Locator("#InputFrom").FillAsync("");
    }

    [When("I leave the destination empty")]
    public async Task WhenILeaveTheDestinationEmpty()
    {
        await Page.Locator("#InputTo").FillAsync("");
    }

    [Then("I should see required field validation messages")]
    public async Task ThenIShouldSeeRequiredFieldValidationMessages()
    {
        await Assertions.Expect(
            Page.GetByText("The From field is required.")
        ).ToBeVisibleAsync();

        await Assertions.Expect(
            Page.GetByText("The To field is required.")
        ).ToBeVisibleAsync();

        await Assertions.Expect(
            Page.Locator(".jp-results-headline")
        ).ToHaveCountAsync(0);
    }


    private IPage Page =>
       _page ?? throw new InvalidOperationException(
           "The browser page has not been initialised.");
}