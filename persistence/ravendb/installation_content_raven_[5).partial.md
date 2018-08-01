To install RavenDB [download the server](https://ravendb.net/download) and install as described in the [RavenDB Documentation](https://ravendb.net/docs/) or use a hosted RavenDB provider like [RavenHQ](https://www.ravenhq.com/).

RavenDB should only be used for NServiceBus persistence when the endpoint's business data is already stored in RavenDB. NServiceBus will need to share the same `DocumentStore` object used for business data, configured using the [RavenDB connection options](connection.md).


## Upgrading RavenDB

To upgrade an existing RavenDB installation refer to the [RavenDB upgrade process](https://ravendb.net/docs/search/latest/csharp?searchTerm=server-administration%20upgrade).

It is highly recommended to backup all databases before upgrading.