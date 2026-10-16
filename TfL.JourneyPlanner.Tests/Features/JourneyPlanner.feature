Feature: TfL Journey Planner

  As a passenger
  I want to plan journeys using the TfL website
  So that I can find suitable travel options

  Background:
    Given I open the TfL Journey Planner

  @smoke @positive
  Scenario: Plan a valid journey between two Underground stations
    When I select "Leicester Square Underground Station" as the starting point
    And I select "Covent Garden Underground Station" as the destination
    And I plan the journey
    Then I should see journey results

  @positive @walking
  Scenario: Validate walking journey time
    When I select "Leicester Square Underground Station" as the starting point
    And I select "Covent Garden Underground Station" as the destination
    And I plan the journey
    And I select the walking travel option
    Then I should see the walking journey time

  @positive @cycling
  Scenario: Validate cycling journey time
    When I select "Leicester Square Underground Station" as the starting point
    And I select "Covent Garden Underground Station" as the destination
    And I plan the journey
    And I select the cycling travel option
    Then I should see the cycling journey time

  @positive @preferences
  Scenario: Update journey preferences to minimise walking
    When I select "Leicester Square Underground Station" as the starting point
    And I select "Covent Garden Underground Station" as the destination
    And I plan the journey
    And I edit the journey preferences
    And I select routes with the least walking
    And I update the journey
    Then I should see updated journey results
    And I should see the journey time

  @positive @accessibility
  
Scenario: View access information for Covent Garden Underground Station
    When I select "Leicester Square Underground Station" as the starting point
    And I select "Covent Garden Underground Station" as the destination
    And I plan the journey
    And I edit the journey preferences
    And I select routes with the least walking
    And I update the journey
    And I open the journey details
    Then I should see complete access information for "Covent Garden Underground Station"


  @negative @invalid
  Scenario: No results for invalid journey locations
    When I enter an invalid starting point
    And I enter an invalid destination
    And I plan the journey
    Then I should see no journey results

  @negative @empty
  Scenario: No results when journey locations are not entered
    When I leave the starting point empty
    And I leave the destination empty
    And I plan the journey
    Then I should see required field validation messages