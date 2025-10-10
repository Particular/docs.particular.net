The container that is used by default for all incoming messages is specified via the `DefaultContainer(..)` configuration API:

snippet: CosmosDBDefaultContainer

#if-version [3, )

---

**Added in version 3.2.1:** By default, message container extractors cannot override the configured default container. To allow extractors to override the default container, enable the `EnableContainerFromMessageExtractor` flag:

```csharp
config.UsePersistence<CosmosPersistence>()
    .EnableContainerFromMessageExtractor();
```

When this flag is enabled and multiple extractors are configured, the last extractor in the pipeline determines the final container. For example, if both a [Header Extractor](/persistence/cosmosdb/transactions.md#specifying-the-container-to-use-for-the-transaction-using-message-header-values) (physical stage) and a [Message Extractor](/persistence/cosmosdb/transactions.md#specifying-the-container-to-use-for-the-transaction-using-the-message-contents) (logical stage) are configured, the Message Extractor takes precedence.

> [!NOTE]
> If an extractor fails to retrieve container information, the system falls back to the next available source in this order: Message Extractor → Header Extractor → configured default container. If no default container is configured and all extractors fail, an exception is thrown.

---

#end-if

When installers are enabled, this (default) container will be created if it doesn't exist. To opt-out of creating the default container, either disable the installers or use `DisableContainerCreation()`:

snippet: CosmosDBDisableContainerCreation

Any other containers that are resolved by extracting partition information from incoming messages need to be [manually created in Azure](https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/how-to-create-container).
