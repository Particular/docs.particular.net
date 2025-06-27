## Design considerations

It's recommended to not mix the processing of dispatch messages with business messages in order to get:

- Predictable dispatch message dispatch: Processing of dispatch messages will be more reliable since there is no risk of getting delayed behind slow business messages
- More accurate metrics: Metrics like critical time and queue length will accurately represent the performance of the dispatch message processing and not be skewed by business messages
- Simplified management: Knowing that the endpoint only processes dispatch messages makes it possible to always retry all failed messages related to the endpoint via tools like ServicePulse

When configuring endpoints for usage measurement in ServicePulse, mark dedicated transactional session processor endpoints with the appropriate [endpoint type indicator](/servicepulse/usage.md#setting-an-endpoint-type-endpoint-type-indicators).