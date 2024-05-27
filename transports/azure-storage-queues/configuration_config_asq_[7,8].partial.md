
Settings can be overridden only using configuration API:

snippet: AzureStorageQueueConfigCodeOnly

Use the [`QueueServiceClient`](https://learn.microsoft.com/en-us/dotnet/api/azure.storage.queues.queueserviceclient.-ctor?view=azure-dotnet), [`BlobServiceClient`](https://learn.microsoft.com/en-us/dotnet/api/azure.storage.blobs.blobserviceclient.-ctor?view=azure-dotnet) or [`TableServiceClient`](https://learn.microsoft.com/en-us/dotnet/api/azure.data.tables.tableserviceclient.-ctor?view=azure-dotnet) constructor overload that suits the authentication needs when creating the clients passed to the transport.