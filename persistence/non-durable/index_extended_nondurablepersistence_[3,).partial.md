## Advanced configuration

For scenarios that require additional control, use `UseNonDurablePersistence(NonDurablePersistenceOptions)`:

snippet: ConfiguringNonDurableOptions

### Shared storage

When multiple endpoints need to share the same in-memory state, provide a `NonDurableStorage` instance through the options:

snippet: ConfiguringNonDurableSharedStorage

Storage resolution follows this precedence:

1. A `NonDurableStorage` resolved from dependency injection
2. The `Storage` property set on `NonDurablePersistenceOptions`
3. A default shared storage instance

When using the [generic host or multi-endpoint hosting](/nservicebus/hosting/), register `NonDurableStorage` in the service collection. The persister automatically resolves it from dependency injection before falling back to the options or default storage:

snippet: NonDurableMultiEndpointStorage

### Time provider

Supply a custom `System.TimeProvider` to control how timestamps and outbox entry expiry are calculated. This is useful for testing scenarios that need deterministic time behavior.

### Saga serialization

Saga data is the only persistence state that is JSON-serialized. By default, `System.Text.Json` is used with reflection. For AOT-compatible deployments or trimmed applications, provide a source-generated serializer context:

snippet: ConfiguringNonDurableSagaSerialization

## Custom saga finders

Non-durable persistence supports [custom saga finders](/nservicebus/sagas/saga-finding.md) via `ISagaFinder<TSagaData, TMessage>`.

### Querying saga data through the synchronized storage session

When correlation logic is too complex to express through the standard saga mapping API, use `INonDurableStorageSession.GetSagaData` to query the in-memory saga store directly from within a custom finder:

snippet: NonDurableSagaProjection

The query is evaluated against a moment-in-time snapshot of the underlying storage. Entries added or removed concurrently may or may not be included. The returned saga data is a copy of the stored entry, and optimistic concurrency checks still apply if the saga is later updated or completed.

For unit testing, use `TestableNonDurableSynchronizedStorageSession` to create a fake session backed by an in-memory store.

### Using a custom index with ISagaPersister

When maintaining a custom lookup index outside of the persister, resolve the saga ID from the index and delegate to `ISagaPersister.Get` to load the saga data. This still captures the saga entry for optimistic concurrency checks:

snippet: NonDurableSagaFinderWithPersister

## OpenTelemetry instrumentation

Non-durable persistence emits spans via the `NServiceBus.Persistence.NonDurable` activity source when an OpenTelemetry listener is configured.

### Saga spans

| Span name | Description |
|:---|:---|
| `NServiceBus.NonDurable.Persistence.Saga.GetById` | Loading a saga by its identifier |
| `NServiceBus.NonDurable.Persistence.Saga.GetByProperty` | Loading a saga by a correlated property |
| `NServiceBus.NonDurable.Persistence.Saga.Save` | Saving a new saga instance |
| `NServiceBus.NonDurable.Persistence.Saga.Update` | Updating an existing saga instance |
| `NServiceBus.NonDurable.Persistence.Saga.Complete` | Completing a saga instance |

### Outbox spans

| Span name | Description |
|:---|:---|
| `NServiceBus.NonDurable.Persistence.Outbox.BeginTransaction` | Beginning an outbox transaction |
| `NServiceBus.NonDurable.Persistence.Outbox.Get` | Retrieving an outbox record |
| `NServiceBus.NonDurable.Persistence.Outbox.Store` | Storing transport operations in the outbox |
| `NServiceBus.NonDurable.Persistence.Outbox.SetAsDispatched` | Marking an outbox record as dispatched |

### Subscription spans

| Span name | Description |
|:---|:---|
| `NServiceBus.NonDurable.Persistence.Subscription.Subscribe` | Subscribing to a message type |
| `NServiceBus.NonDurable.Persistence.Subscription.Unsubscribe` | Unsubscribing from a message type |
| `NServiceBus.NonDurable.Persistence.Subscription.GetSubscribers` | Resolving subscribers for a message type |

See the [OpenTelemetry documentation](/nservicebus/operations/opentelemetry.md) for instructions on how to enable tracing in an endpoint.