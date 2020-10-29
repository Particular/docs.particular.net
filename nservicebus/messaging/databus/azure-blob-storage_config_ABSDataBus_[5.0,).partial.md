### Configuring the BlobServiceClient

There are several ways to configure the `BlobServiceClient`. 

#### Using a custom provider

A custom provider can be declared that provides a fully configured `BlobServiceClient`:

snippet: CustomBlobServiceClientProvider

The provider is then registered in the container:

snippet: AzureDataBusInjectServiceClient

#### Using the databus settings

A fully configured `BlobServiceClient` can be set through the settings:

snippet: AzureDataBusConfigureServiceClient

#### Providing a connection string and container name

snippet: AzureDataBusConnectionAndContainer

NOTE: The container name is not required and will be set to a default value unless a value is supplied.