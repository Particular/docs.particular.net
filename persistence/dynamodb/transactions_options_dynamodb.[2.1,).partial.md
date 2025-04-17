Starting with Version 2.1 of the persistence it is possible to override or extend the default JsonSerializationOptions used by the mapper. This allows making the mapper more aware of the object model attributes used by the `DynamoDBContext` if desired.

snippet: DynamoDBMapperContextUsageWithJsonOptions