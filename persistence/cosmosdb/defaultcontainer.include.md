The container that is used by default for all incoming messages is specified via the `DefaultContainer(..)` configuration API:

snippet: CosmosDBDefaultContainer

#if-version [3.2, )
If the container information is being extracted during runtime from a message instance (header or message body), the default container specified will be overwritten by the last extractor in the pipeline. For example, if a [Header Extractor](/persistence/cosmosdb/transactions.md#specifying-the-container-to-use-for-the-transaction-using-message-header-values) (physical stage) and a [Message Extractor](/persistence/cosmosdb/transactions.md#specifying-the-container-to-use-for-the-transaction-using-the-message-contents) (logical stage) are both configured, then the container information within the Message Extractor would be used.

> [!NOTE]
> The exception to the above is if the extractor fails to pull container information from the message (logical), then the container information in the header (physical) will be used. If this also fails, the configured default container will be used as a fallback. If there is no default container configured, an exception will be thrown.
#end-if

When installers are enabled, this (default) container will be created if it doesn't exist. To opt-out of creating the default container, either disable the installers or use `DisableContainerCreation()`:

snippet: CosmosDBDisableContainerCreation

Any other containers that are resolved by extracing partition information from incoming messages need to be [manually created in Azure](https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/how-to-create-container).
