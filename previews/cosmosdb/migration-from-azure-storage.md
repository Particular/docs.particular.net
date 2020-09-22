---
title: Migration from Azure Storage Persistence
component: cosmosdb
related:
- persistence/azure-storage
reviewed: 2020-09-20
---

The tool can be obtained from MyGet and installed using the following command:

```
dotnet tool install -g Particular.AzureStoragePersistenceSagaExporter --add-source=https://www.myget.org/F/particular/api/v3/index.json
```

Once installed, the `export-aspsagas` command line tool will be available for use.

`export-aspsagas [options]`

## Options
 
`-c` | `--connectionstring` : Set the connection string to the table storage

`-s` | `--sagadataname`: The saga data class name without the namespace (e.g. `OrderSagaData`) of the saga data to export. This will be used to derive the table storage name from.

## Migration

TBD