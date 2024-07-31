Feature: AccessibilityTests

Scenario: Home page accessibility
	When the user views the home page
	Then the home page is accessible

@ignore
Scenario: Search results page accessibility
	When the user views the search results page
	Then the search results page is accessible