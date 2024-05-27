
Settings should be set when instantiating the transport:

snippet: AzureStorageQueueConfigCodeOnly

Use the `QueueServiceClient`, `BlobServiceClient` or `TableServiceClient` constructor overload that suits the authentication needs when creating the clients passed to the transport.