## Multi-tenant support

Starting with version 4.2, it's possible to select the database used to store NServiceBus-related data, such as saga data and outbox records, based on information stored in the incoming messages headers:

snippet: multi-tenant-support
