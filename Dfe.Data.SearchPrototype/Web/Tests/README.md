# Overview

This page outlines the test types that form part of the Dfe.Data.SearchPrototype.web.Tests project.

## WebApplicationFactory

A number of tests utilise the `WebApplicationFactory` which hosts the .NET Core 
request pipeline defined for the application in `Program.cs`. 

It uses ``TestServer`` from 
the Microsoft.AspNetCore.Mvc.Testing package.

## API Tests

Functional tests against the Dfe.Data.SearchPrototype.WebApi project.

A test server is hosted in-memory running/bootstrapping the WebApi app through `WebApplicationFactory`.

Data is derived from a deployed Azure AI Search component.

## Partial Integration Tests

TODO

## Presentation Layer Tests

TODO

## Unit Tests

TODO

## Web Integration Accessibility Tests

Non-functional accessibility tests against the Dfe.Data.SearchPrototype.Web project.

A test server is hosted in-memory running/bootstrapping the Web app through `WebApplicationFactory`.

Browser commands are sent to a WebDriver instance.

Data is derived from a deployed Azure AI Search component.

## Web Integration HTTP Tests

Functional tests of page objects and respective page content against the Dfe.Data.SearchPrototype.Web project.

A test server is hosted in-memory running/bootstrapping the Web app through `WebApplicationFactory`.

HTTP requests are sent to an in-memory instance of the Web app.

Data is derived from a deployed Azure AI Search component.

## Web Integration UI Tests

Functional tests of behaviours such as Javascript components against the Dfe.Data.SearchPrototype.Web project.

A Test server is hosted in-memory running/bootstrapping the Web app through `WebApplicationFactory`.

Browser commands are sent to a WebDriver instance.

Data is derived from a deployed Azure AI Search component.