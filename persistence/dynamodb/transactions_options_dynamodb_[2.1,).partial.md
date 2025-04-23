Starting with Version 2.1 of the persistence it is possible to override or extend the default `JsonSerializationOptions` used by the mapper. This allows the mapper to be more aware of the object model attributes used by the `DynamoDBContext` if desired.

snippet: DynamoDBMapperContextUsageWithJsonOptions