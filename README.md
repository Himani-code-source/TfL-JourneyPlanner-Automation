
# TfL Journey Planner – UI Test Automation Framework

## Overview

This project is a C#/.NET UI automation framework developed
for the Transport for London (TfL) to test Journey Planner.

The framework tests the TfL Journey Planner:
https://tfl.gov.uk/plan-a-journey

It uses:
- C# and .NET 10
- Microsoft Playwright for browser automation
- Reqnroll for Gherkin/BDD scenarios
- xUnit for test execution
- Selenium WebDriver with ChromeDriver for an additional UI smoke test
![alt text](<Screenshot 2026-10-17 003342.png>)

## Project Structure

TfL.JourneyPlanner.Tests/
- Features/JourneyPlanner.feature
- StepDefinitions/JourneyPlannerSteps.cs
- SeleniumTests/JourneyPlannerSeleniumTests.cs
- TfL.JourneyPlanner.Tests.csproj

TfL.JourneyPlanner.slnx
README.md

## Test Coverage

The framework includes eight automated tests:

1. Plan a valid journey between two Underground stations.
2. Validate walking journey time.
3. Validate cycling journey time.
4. Update journey preferences to minimise walking.
5. View access information for Covent Garden Underground Station.
6. Verify behaviour for invalid journey locations.
7. Verify required-field validation for empty locations.
8. Verify Journey Planner page loads using Selenium ChromeDriver.

## Prerequisites

- .NET 10 SDK
- Visual Studio Code or another compatible .NET IDE
- Google Chrome
- Playwright Chromium browser binaries
- Internet connection to access the live TfL website

## Setup

Restore project dependencies:

```powershell
dotnet restore TfL.JourneyPlanner.slnx
```

Build the solution:

```powershell
dotnet build TfL.JourneyPlanner.slnx
```

Install Playwright Chromium if it has not already been installed:

```powershell
pwsh TfL.JourneyPlanner.Tests/bin/Debug/net10.0/playwright.ps1 install chromium
```

## Running Tests

Run all tests:

```powershell
dotnet test TfL.JourneyPlanner.slnx
```

Run the Selenium test:

```powershell
dotnet test TfL.JourneyPlanner.slnx --filter "FullyQualifiedName~JourneyPlannerSeleniumTests"
```

List available tests:

```powershell
dotnet test TfL.JourneyPlanner.slnx --list-tests
```

## Framework Design Decisions

### Behaviour-Driven Development

Reqnroll and Gherkin were selected to express business
scenarios using Given, When and Then steps.

This makes test scenarios easier to understand for both
technical and non-technical stakeholders.

### Playwright

Playwright was selected for its automatic waiting,
browser interaction capabilities and locator-based assertions.

### Selenium ChromeDriver

A separate Selenium smoke test demonstrates the use of
ChromeDriver and validates the Journey Planner's main
input fields.

### Test Isolation

Playwright scenarios create their own browser session,
reducing dependencies between tests.

### Assertions

Tests verify relevant UI behaviour, including journey
results, journey times, validation messages, preferences
and accessibility information.


## AI Usage Disclosure

AI assistance (ChatGPT) was used during development to
support framework setup, code generation, debugging,
locator identification, test implementation and documentation.

The generated code was executed and iteratively validated
against the TfL Journey Planner website.

AI-assisted work is disclosed in accordance with the
technical assessment instructions.
