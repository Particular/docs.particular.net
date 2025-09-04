### Disable fallback reads

By default, outbox records are retrieved using the new structured ID format. If no record is found, the persistence falls back to reading entries stored with the previous non-structured ID. This fallback ensures backward compatibility during the transition period.

Fallback reads can be disabled once all legacy records have either expired or been dispatched, ensuring that only the structured format is queried. For details and recommendations, see the [upgrade guide](/persistence/upgrades/mongodb-5to6.md).

snippet: MongoDBDisableReadFallback