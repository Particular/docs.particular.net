The container that is used by default for all incoming messages is specified via the `DefaultContainer(..)` configuration API:

snippet: CosmosDBDefaultContainer

#if-version [3, )
**Added in version 3.2.1:** An opt-in flag `EnableContainerFromMessageExtractor` has been introduced to enable the below behavior added in `v3.2.0` in a non-breaking way, particularily if a default container and a message container extractor is used. If this flag is not used, the default behavior will be that the message container extractor will not overwrite the default container. This flag will become redundant in `v4.0.0` resulting in the correct default behavior introduced in `v3.2.0`, and be removed in `v5.0.0`.

```csharp
config.UsePersistence<CosmosPersistence>()
    .EnableContainerFromMessageExtractor();
```

**Added in version 3.2.0:** If the container information is being extracted during runtime from a message instance (header or message body), the default container specified will be overwritten by the last extractor in the pipeline. For example, if a [Header Extractor](/persistence/cosmosdb/transactions.md#specifying-the-container-to-use-for-the-transaction-using-message-header-values) (physical stage) and a [Message Extractor](/persistence/cosmosdb/transactions.md#specifying-the-container-to-use-for-the-transaction-using-the-message-contents) (logical stage) are both configured, then the container information within the Message Extractor would be used.

> [!NOTE]
> **Added in version 3.2.0:** The exception to the above is if the extractor fails to pull container information from the message (logical), then the container information in the header (physical) will be used. If this also fails, the configured default container will be used as a fallback. If there is no default container configured, an exception will be thrown.
#end-if

When installers are enabled, this (default) container will be created if it doesn't exist. To opt-out of creating the default container, either disable the installers or use `DisableContainerCreation()`:

snippet: CosmosDBDisableContainerCreation

Any other containers that are resolved by extracting partition information from incoming messages need to be [manually created in Azure](https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/how-to-create-container).
