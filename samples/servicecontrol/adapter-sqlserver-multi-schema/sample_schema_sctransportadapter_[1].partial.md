The Sales and Shipping endpoints use separate schemas within the same database. The configuration of the schemas differs slightly because the Sales endpoint uses NServiceBus 6 while Shipping uses NServiceBus 5. The following code shows the Version 6 configuration:

snippet: SalesSchema

The following code shows similar configuration expressed using version 5 APIs:

snippet: ShippingSchema

Both snippets show how to map the ServiceControl queues to the adapter schema, how to configure the schema for the current endpoint and how to specify other endpoints' schemas.