
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace TfL.JourneyPlanner.Tests.SeleniumTests;

public class JourneyPlannerSeleniumTests
{
    [Fact]
    public void JourneyPlannerPage_ShouldLoadSuccessfully()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");

        using var driver = new ChromeDriver(options);

        driver.Navigate().GoToUrl(
            "https://tfl.gov.uk/plan-a-journey");

        var wait = new WebDriverWait(
            driver, TimeSpan.FromSeconds(30));

        var fromField = wait.Until(d =>
            d.FindElement(By.Id("InputFrom")));

        Assert.True(fromField.Displayed,
            "The Journey Planner From field should be visible.");

        var toField = driver.FindElement(
            By.Id("InputTo"));

        Assert.True(toField.Displayed,
            "The Journey Planner To field should be visible.");
    }
}
