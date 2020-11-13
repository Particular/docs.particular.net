### Message endpoint mappings

Message endpoint mappings provide a way to configure message destinations using an XML config section. Configure the destination by specifying a connection string behind the endpoint name separated by an `@` sign. Each endpoint can have its own storage account to overcome the throughput limitations.

Example: Endpoint 1 sends messages to Endpoint 2. Endpoint 1 defines a message mapping with a connection string associated with the Endpoint 2 Azure storage account.

Message mapping for Endpoint 1:

snippet: storage_account_routing_send_message_mapping

The same applies when and endpoint is subscribing to and endpoint in another storage account. E.g Endpoint2 is subscribing to Endpoint 1.

Message mapping for Endpoint 2:

snippet: storage_account_routing_subscribe_message_mapping