## Accessing options from Behaviors

Settings configured on message operation options (e.g., `SendOptions`, `PublishOptions`, etc.) are internally stored in a dedicated `ContextBag`. These settings are accessibe within the pipeline via the `context.GetOperationProperties()` extension:

snippet: getoperationproperties