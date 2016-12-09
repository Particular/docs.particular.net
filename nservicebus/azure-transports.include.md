NOTE: As part of the Azure support for NServiceBus, one can choose between two transports provided by the Azure platform: [Azure Storage Queues](/nservicebus/azure-storage-queues/) and [Azure Service Bus](/nservicebus/azure-service-bus/). Each of them has different features, capabilities, and usage characteristics. A detailed comparison and discussion of when to select which is beyond the scope of this document. To help decide which option best suits the application's needs, refer to the  [Azure Queues and Azure Service Bus Queues - Compared and Contrasted](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-azure-and-service-bus-queues-compared-contrasted) article.


{{WARNING:

When considering an Azure Transport it is important to consider the transaction guarantees that that service provides.

 * [Azure Storage Queues - Transaction Support](/nservicebus/azure-storage-queues/transaction-support.md)
 * [Azure Service Bus - Transaction Support](/nservicebus/azure-service-bus/transaction-support.md)
 * [Understanding Transactionality in Azure](/nservicebus/azure/understanding-transactionality-in-azure.md)

}}
