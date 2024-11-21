# data.search-prototype-data

This utility is part of the Search Prototype project. 
It is responsible for pushing data from a local file to Azure Search AI service.

The data is sourced from Get Information About Schools (GIAS), specifically the public "all establishments" CSV file.

## Usage / Quick Start

Assuming your index is already setup (if not, see the "Index" section below):

1. Download a copy of the public "All EduBase data.csv"
   - A direct link to this file can be found via [the legacy GIAS/Edubase web UI](https://ea-edubase-backend-prod.azurewebsites.net/edubase/home.xhtml)
   - Alternatively, download and extract the "All establishment data/establishment fields CSV" file from [the GIAS web UI](https://www.get-information-schools.service.gov.uk/Downloads)
2. Update local configuration/secrets to point to your local copy of the CSV file and include connection details for your Azure Search service
   - See the "Configuration" section below
3. Run the app to read data from the CSV and push the data to your Azure Search service
   - This will happen in batches of 99 records at a time (as per Postcode API lookup limits) - see also `Dfe.Data.SearchPrototype.Data.ManageData.BatchSize`

## Azure AI/Cognitive Search Index

This has to be created prior to running the app to push the data.
![index](docs/images/index.jpg)

`id` field has been made:

- Retrievable
- Sortable
- Searchable

All remaining fields have been made:

- Retrievable
- Filterable
- Sortable
- Facetable
- Searchable

## Reference index field to data

`Dfe.Data.SearchPrototype.Data.DocumentBatchHelpers.ConvertBatchToJson` references:
`INDEX_FIELD = record.Column_header`
where: `INDEX_FIELD` has to match an index field in your Azure Search Service,
`record` is a row of your csv document and `Column_header` is a name of your column header.

## Configuration

To run the app in order to push your data, update:

- `appsettings.json`
  Details needed to make a POST request to Azure Search service, as well as a path to your local file containing data.

AzureSearchServiceDetails section contains information which you can get from your SearchService container on Azure.
![azureOverwiev](docs/images/azure_overview_screenshot.jpg)

- `secrets.json`
  ensure to copy your `apiKey` into the secrets file and NOT appsettings!

Simply run the app using your IDE or command line to push the data and observe your command-line to see if the push was
successful.
