### Configuring the BlobServiceClient

There are several ways to configure the `BlobServiceClient`. 

#### Using a preconfigured BlobServiceClient

A fully configured `BlobServiceClient` can be set through the settings:

snippet: AzureDataBusConfigureServiceClient

#### Using a custom provider

A custom provider can be declared that provides a fully configured `BlobServiceClient`:

snippet: CustomBlobServiceClientProvider

The provider is then registered in the dependency injection container:

snippet: AzureDataBusInjectServiceClient

#### Providing a connection string and container name

snippet: AzureDataBusConnectionAndContainer

NOTE: The container name is optional and will be set to the default when omitted.
