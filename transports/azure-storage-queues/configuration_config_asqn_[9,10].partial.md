
Settings can be overridden only using configuration API:

snippet: AzureStorageQueueConfigCodeOnly

Use the `QueueServiceClient`, `BlobServiceClient` or `TableServiceClient` constructor overload that suits the authentication needs when creating the clients passed to the transport.