In cases when the `IMongoClient` is configured and used via dependency injection, a custom provider can be implemented:

snippet: MongoDBClientProvider

and then registered on the container

snippet: MongoDBCustomClientProviderRegistration

When hosting with the generic host, the registrations can also be done directly on the service collection.