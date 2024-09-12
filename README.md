This solution uses the SearchPrototype application package to surface the results of a keyword search through both a Web MVC project and an API.
# data.search-prototype-web-mvc
Web MVC frontend host for search-prototype application.
## data.search-prototype-data
Project responsible for pushing data from a local file to Azure Search AI service. Refer to README file within the project for more information
## data.search-prototype-webApi
Project created to surface the results of a keyword search through an API.
## Using The Nuget Packages From Your Development Machine
Some Nuget packages referenced by this repository are served under the DfE-Digital organisation.
To be able to use these Nuget Packages (and others) you must configure your development machine to have a new NuGet Package Source.
To do this, you must first create a PAT token that has at least __read access for packages__.

> **NEVER commit your PAT token to GitHub or any other VCS !**

Next add a package source to your NuGet configuration using the CLI.
Use the following command, replacing `[USERNAME] with your GitHub username, and `[PAT-TOKEN] with the PAT token you just generated.

`dotnet nuget add source --username "[USERNAME]" --password "[PAT-TOKEN]" --store-password-in-clear-text --name DfE "https://nuget.pkg.github.com/DFE-Digital/index.json"`

> Alternatively you may add a package source directly in Visual Studio.Once you have generated a PAT token you can add a new NuGet Package Source to visual studio. You may be prompted to sign in, if you are then enter your GitHub username and instead of the password enter the PAT token you generated.

---
 
## Referencing the Nuget Registry From a GitHub Action That Directly Builds DotNet Projects
This applies when building dotnet solutions that reference the nuget registry directly within a GitHub action.

You can use the Nuget Registry from a GitHub action pipeline without need for a PAT token. GitHub creates a special token for use during the lifetime of the GitHub action. For your apps to be able to restore from the DfE nuget repository, add the followint two lines to your yml file __before__ restoring packages

```sh
- name: Add nuget package source
  run: dotnet nuget add source --username USERNAME --password ${{ secrets.GITHUB_TOKEN }} --store-password-in-clear-text --name github "https://nuget.pkg.github.com/DFE-Digital/index.json"
```