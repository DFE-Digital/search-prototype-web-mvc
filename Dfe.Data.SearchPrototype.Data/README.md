# data.search-prototype-data
Project created to manually push data from local file to Azure Search AI service
## File format
In order to upload data from your local file, you will need to use csv format. 

## Configuration
To run the app in order to push your data, update:
- `appsettings.json` 
 details needed to make a POST request to Azure Search service, as well as a path to your local file containing data.
- `secrets.json`
API key 

AzureSearchServiceDetails section within appsettings.json contains information which you can get from your SearchService container on Azure.

