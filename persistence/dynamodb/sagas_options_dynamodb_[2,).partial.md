Sagas being persisted use the built-in mapper as described above. With Version 2.1 of the persistence it is possible to override the saga persistence `JsonSerializationOptions` to support scenarios such as [source-generated serialization contexts](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation).

snippet: DynamoDBSagaOptions