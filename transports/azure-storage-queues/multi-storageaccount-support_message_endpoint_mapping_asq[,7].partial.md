### Message endpoint mappings

Message endpoint mappings provides a way to configure message destinations using an XML config section. Configure this by specifying a connection string behind the endpoint name separated by an `@` sign. Each endpoint can have its own storage account to overcome the throughput limitations.

Example: Endpoint 1 sends messages to Endpoint 2. Endpoint 1 defines a message mapping with a connection string associated with the Endpoint 2 Azure storage account. The same applies when Endpoint 
2 is sending messages to Endpoint 1.

Message mapping for Endpoint 1:

```xml
<MessageEndpointMappings>
  <add Messages="Contracts"
       Namespace="Contracts.Commands.ForEndpoint2"
       Endpoint="Endpoint2@DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];" />
</MessageEndpointMappings>
```

Message mapping for Endpoint 2:

```xml
<MessageEndpointMappings>
  <add Messages="Contracts"
       Namespace="Contracts.Commands.ForEndpoint1"
       Endpoint="Endpoint1@DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];" />
</MessageEndpointMappings>
```