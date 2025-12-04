# NServiceBus v10 Breaking Changes - Affected Samples Analysis

This document identifies samples in this repository that would be affected when upgrading to NServiceBus v10, based on the breaking changes documented in the [NServiceBus 9 to 10 upgrade guide](nservicebus/upgrades/9to10/index.md).

## Executive Summary

**Total affected sample versions: 407**
- **176 samples** using NServiceBus v9 packages (need package upgrade)
- **15 samples** using DataBus feature (need migration to NServiceBus.ClaimCheck)
- **405 samples** targeting .NET 8/9 (need .NET 10 upgrade)

## Breaking Changes in NServiceBus v10

### 1. Minimum .NET Version
- **Change**: Requires .NET 10 (previously .NET 8)
- **Impact**: All samples targeting .NET 8 or .NET 9 need framework upgrade

### 2. DataBus → NServiceBus.ClaimCheck
- **Change**: DataBus feature moved to separate package with API changes
- **Impact**: Requires:
  - Add `NServiceBus.ClaimCheck` package reference
  - Update namespace from `NServiceBus.DataBus` to `NServiceBus.ClaimCheck`
  - Rename `DataBusProperty<T>` to `ClaimCheckProperty<T>`
  - Update configuration: `UseDataBus` → `UseClaimCheck`

### 3. ContextBag Null Values
- **Change**: ContextBag can no longer store null values
- **Impact**: Code storing null in ContextBag needs updates

### 4. PersistenceDefinition Extensibility
- **Change**: Must explicitly implement `IPersistenceDefinitionFactory<TDefinition>`
- **Impact**: Custom persistence implementations need updates

### 5. Async Disposable
- **Change**: `ICompletableSynchronizedStorageSession` and `IOutboxTransaction` implement `IAsyncDisposable`
- **Impact**: Disposal code may need updates for async patterns

## High Priority: DataBus Samples (15 samples)

These require the most code changes due to DataBus → ClaimCheck migration:

### DataBus Category Samples
1. `samples/databus/blob-storage-databus-cleanup-function/ABSDataBus_5/`
2. `samples/databus/blob-storage-databus-cleanup-function/ABSDataBus_6/`
3. `samples/databus/blob-storage-databus-cleanup-function/ABSDataBus_7/`
4. `samples/databus/blob-storage-databus/ABSDataBus_5/`
5. `samples/databus/blob-storage-databus/ABSDataBus_6/`
6. `samples/databus/blob-storage-databus/ABSDataBus_7/`
7. `samples/databus/custom-serializer/Core_8/`
8. `samples/databus/custom-serializer/DataBus_1/`
9. `samples/databus/databus-custom-serializer-converter/Core_8/`
10. `samples/databus/databus-custom-serializer-converter/Core_9/`
11. `samples/databus/file-share-databus/Core_8/`

### Other Samples Using DataBus
12. `samples/unobtrusive/Core_8/Client/`
13. `samples/unobtrusive/Core_8/Server/`
14. `samples/unobtrusive/Core_9/Client/`
15. `samples/unobtrusive/Core_9/Server/`

## Samples Using NServiceBus v9 (176 samples)

### AsyncAPI (2 samples)
- `samples/asyncapi/custom-message-types/Core_9/`
- `samples/asyncapi/simple/Core_9/`

### AWS (13 samples)
- `samples/aws/dynamodb-simple/DynamoDB_2/`
- `samples/aws/dynamodb-simple/DynamoDB_3/`
- `samples/aws/dynamodb-transactions/DynamoDB_2/`
- `samples/aws/dynamodb-transactions/DynamoDB_3/`
- `samples/aws/lambda-sqs-annotations/SQSLambda_2/`
- `samples/aws/lambda-sqs-annotations/SQSLambda_3/`
- `samples/aws/lambda-sqs/SQSLambda_2/`
- `samples/aws/lambda-sqs/SQSLambda_3/`
- `samples/aws/sagas-lambda-aurora/SQSLambda_2/`
- `samples/aws/sqs-native-integration/Sqs_7/`
- `samples/aws/sqs-native-integration/Sqs_8/`
- `samples/aws/sqs-simple/Sqs_7/`
- `samples/aws/sqs-simple/Sqs_8/`

### Azure (8 samples)
- `samples/azure/azure-table/saga-transactions/ASTP_6/`
- `samples/azure/azure-table/simple/ASTP_6/`
- `samples/azure/azure-table/table/ASTP_6/`
- `samples/azure/azure-table/transactions/ASTP_6/`
- `samples/azure/custom-audit-transport/Core_9/`
- `samples/azure/native-integration-asq/ASQN_13/`
- `samples/azure/storage-queues/ASQN_13/`
- `samples/azure/webjob-host/Core_9/`

### Azure Functions (6 samples)
- `samples/azure-functions/service-bus-kafka/ASBS_4/`
- `samples/azure-functions/service-bus-kafka/ASBS_5/`
- `samples/azure-functions/service-bus-worker/ASBFunctionsWorker_5/`
- `samples/azure-functions/service-bus-worker/ASBFunctionsWorker_6/`
- `samples/azure-functions/service-bus/ASBFunctions_5/`
- `samples/azure-functions/service-bus/ASBFunctions_6/`

### Azure Service Bus (12 samples)
- `samples/azure-service-bus-netstandard/performance-tuning/ASBS_4/`
- `samples/azure-service-bus-netstandard/performance-tuning/ASBS_5/`
- `samples/azure-service-bus-netstandard/sanitization/ASBS_4/`
- `samples/azure-service-bus-netstandard/sanitization/ASBS_5/`
- `samples/azure-service-bus-netstandard/send-reply/ASBS_4/`
- `samples/azure-service-bus-netstandard/send-reply/ASBS_5/`
- `samples/azure-service-bus-netstandard/topology-api/ASBS_4/`
- `samples/azure-service-bus-netstandard/topology-api/ASBS_5/`
- `samples/azure-service-bus-netstandard/transaction-session/ASBS_4/`
- `samples/azure-service-bus-netstandard/transaction-session/ASBS_5/`
- `samples/azure-service-bus-netstandard/transactions/ASBS_4/`
- `samples/azure-service-bus-netstandard/transactions/ASBS_5/`

### Bridge (11 samples)
- `samples/bridge/azure-service-bus-msmq-bridge/Bridge_3/`
- `samples/bridge/azure-service-bus-msmq-bridge/Bridge_4/`
- `samples/bridge/backplane/Bridge_4/`
- `samples/bridge/backplane/TransportBridge_1/`
- `samples/bridge/rabbitmq-delayed-delivery/TransportBridge_1/`
- `samples/bridge/service-control/Bridge_3/`
- `samples/bridge/service-control/Bridge_4/`
- `samples/bridge/service-control/TransportBridge_1/`
- `samples/bridge/simple/Bridge_3/`
- `samples/bridge/sql-multi-instance/Bridge_3/`
- `samples/bridge/sql-multi-instance/Bridge_4/`

### Callbacks (1 sample)
- `samples/callbacks/Callbacks_5/`

### Consumer Driven Contracts (1 sample)
- `samples/consumer-driven-contracts/Core_9/`

### Cooperative Cancellation (1 sample)
- `samples/cooperative-cancellation/Core_9/`

### CosmosDB (3 samples)
- `samples/cosmosdb/container/CosmosDB_3/`
- `samples/cosmosdb/simple/CosmosDB_3/`
- `samples/cosmosdb/transactions/CosmosDB_3/`

### Custom Recoverability (1 sample)
- `samples/custom-recoverability/Core_9/`

### DataBus (2 samples with v9)
- `samples/databus/blob-storage-databus-cleanup-function/ABSDataBus_6/`
- `samples/databus/databus-custom-serializer-converter/Core_9/`

### Delayed Delivery (1 sample)
- `samples/delayed-delivery/Core_9/`

### Dependency Injection (3 samples)
- `samples/dependency-injection/aspnetcore/Core_9/`
- `samples/dependency-injection/externally-managed-mode/Core_9/`
- `samples/dependency-injection/externally-managed-mode/Core_9/`

### Encryption (3 samples)
- `samples/encryption/basic-encryption/PropertyEncryption_5/`
- `samples/encryption/encryption-conventions/PropertyEncryption_5/`
- `samples/encryption/message-body-encryption/Core_9/`

### Entity Framework Core (1 sample)
- `samples/entity-framework-core/SqlPersistence_8/`

### Error Handling (1 sample)
- `samples/errorhandling/Core_9/`

### Fault Tolerance (1 sample)
- `samples/faulttolerance/Core_9/`

### Feature (1 sample)
- `samples/feature/Core_9/`

### Full Duplex (1 sample)
- `samples/fullduplex/Core_9/`

### Gateway (1 sample)
- `samples/gateway/Gateway_5/`

### Header Manipulation (1 sample)
- `samples/header-manipulation/Core_9/`

### Hosting (3 samples)
- `samples/hosting/aspire/Core_9/`
- `samples/hosting/docker/Core_9/`
- `samples/hosting/generic-host/Core_9/`

### Immutable Messages (2 samples)
- `samples/immutable-messages/Core_9/`

### Logging (6 samples)
- `samples/logging/datadog/Core_9/`
- `samples/logging/default/Core_9/`
- `samples/logging/extensions-logging/Extensions.Logging_3/`
- `samples/logging/metrics/Metrics_5/`
- `samples/logging/new-relic/Metrics_5/`
- `samples/logging/notifications/Core_9/`

### Message Assembly Sharing (1 sample)
- `samples/message-assembly-sharing/Core_9/`

### Message Mutators (1 sample)
- `samples/messagemutators/Core_9/`

### MongoDB (3 samples)
- `samples/mongodb/simple/MongoDB_4/`
- `samples/mongodb/simple/MongoDB_5/`
- `samples/mongodb/simple/MongoDB_6/`

### Multi-Tenant (6 samples)
- `samples/multi-tenant/di/Core_9/`
- `samples/multi-tenant/nhibernate/NHibernate_10/`
- `samples/multi-tenant/propagation/Core_9/`
- `samples/multi-tenant/ravendb/Raven_10/`
- `samples/multi-tenant/ravendb/Raven_9/`
- `samples/multi-tenant/sqlp/SqlPersistence_8/`

### Near Real-time Clients (2 samples)
- `samples/near-realtime-clients/Core_9/`

### NHibernate (2 samples)
- `samples/nhibernate/custom-mappings/NHibernate_10/`
- `samples/nhibernate/simple/NHibernate_10/`

### Open Telemetry (6 samples)
- `samples/open-telemetry/application-insights/Core_9/`
- `samples/open-telemetry/customizing/Core_9/`
- `samples/open-telemetry/jaeger/Core_9/`
- `samples/open-telemetry/logging/Core_9/`
- `samples/open-telemetry/metrics-shim/Core_9/`
- `samples/open-telemetry/prometheus-grafana/Core_9/`

### Outbox (4 samples)
- `samples/outbox/cosmosdb/Core_9/`
- `samples/outbox/mongodb/Core_9/`
- `samples/outbox/rabbit/Core_9/`
- `samples/outbox/sql/Core_9/`

### Performance Counters (1 sample)
- `samples/performance-counters/PerfCounters_6/`

### Pipeline (9 samples)
- `samples/pipeline/audit-filtering/Core_9/`
- `samples/pipeline/dispatch-notifications/Core_9/`
- `samples/pipeline/feature-toggle/Core_9/`
- `samples/pipeline/fix-messages-using-behavior/Core_9/`
- `samples/pipeline/header-propagation/Core_9/`
- `samples/pipeline/masstransit-messages/Core_9/`
- `samples/pipeline/message-signing/Core_9/`
- `samples/pipeline/session-filtering/Core_9/`
- `samples/pipeline/unit-of-work/Core_9/`

### Platform Connector (3 samples)
- `samples/platform-connector/code-first/PlatformConnector_3/`
- `samples/platform-connector/json/PlatformConnector_3/`
- `samples/platform-connector/ms-config/PlatformConnector_3/`

### PostgreSQL Transport (2 samples)
- `samples/postgresqltransport/simple/PostgresqlTransport_9/`
- `samples/postgresqltransport/simple/PostgresqlTransport_8/`

### Pub/Sub (1 sample)
- `samples/pubsub/native/Core_9/`

### RabbitMQ (4 samples)
- `samples/rabbitmq/native-integration/Rabbit_10/`
- `samples/rabbitmq/native-integration/Rabbit_9/`
- `samples/rabbitmq/simple/Rabbit_10/`
- `samples/rabbitmq/simple/Rabbit_9/`

### RavenDB (2 samples)
- `samples/ravendb/simple/Raven_10/`
- `samples/ravendb/simple/Raven_9/`

### Recoverability Policy Testing (1 sample)
- `samples/recoverabilitypolicytesting/Core_9/`

### Routing (2 samples)
- `samples/routing/command-routing/Core_9/`
- `samples/routing/message-forwarding/Core_9/`

### Saga (4 samples)
- `samples/saga/batching/Core_9/`
- `samples/saga/nh-custom-sagafinder/NHibernate_10/`
- `samples/saga/simple/Core_9/`
- `samples/saga/sql-sagafinder/SqlPersistence_8/`

### Scheduling (4 samples)
- `samples/scheduling/hangfire/Core_9/`
- `samples/scheduling/periodictimer/Core_9/`
- `samples/scheduling/quartz/Core_9/`
- `samples/scheduling/timer/Core_9/`

### Serializers (7 samples)
- `samples/serializers/avro/Core_9/`
- `samples/serializers/multiple-deserializers/Core_9/`
- `samples/serializers/newtonsoft-bson/Newtonsoft_4/`
- `samples/serializers/newtonsoft/Newtonsoft_4/`
- `samples/serializers/system-json/Core_9/`
- `samples/serializers/transitioning-formats/Core_9/`
- `samples/serializers/xml/Core_9/`

### ServiceControl (4 samples)
- `samples/servicecontrol/events-subscription/ServiceControlContracts_5/`
- `samples/servicecontrol/fix-messages/Core_9/`
- `samples/servicecontrol/monitoring3rdparty/CustomChecks_5/`
- `samples/servicecontrol/retry-messages/Core_9/`

### Showcase (2 samples)
- `samples/showcase/cinema/Core_9/`
- `samples/showcase/on-premises/Core_9/`

### SQL Persistence (4 samples)
- `samples/sql-persistence/injecting-services/SqlPersistence_8/`
- `samples/sql-persistence/saga-rename/SqlPersistence_8/`
- `samples/sql-persistence/simple/SqlPersistence_8/`
- `samples/sql-persistence/transitioning-correlation-ids/SqlPersistence_8/`

### SQL Transport (2 samples)
- `samples/sqltransport/native-integration/SqlTransport_8/`
- `samples/sqltransport/simple/SqlTransport_8/`

### SQL Transport with NHibernate (1 sample)
- `samples/sqltransport-nhpersistence/Core_9/`

### SQL Transport with SQL Persistence (1 sample)
- `samples/sqltransport-sqlpersistence/Core_9/`

### Startup Shutdown Sequence (1 sample)
- `samples/startup-shutdown-sequence/Core_9/`

### Throttling (1 sample)
- `samples/throttling/Core_9/`

### Transactional Session (2 samples)
- `samples/transactional-session/aspnetcore-webapi/SqlPersistenceTS_8/`
- `samples/transactional-session/cosmosdb/CosmosTS_3/`

### Unit Testing (1 sample)
- `samples/unit-testing/Testing_9/`

### Unobtrusive (2 samples)
- `samples/unobtrusive/Core_9/`

### Username Header (1 sample)
- `samples/username-header/Core_9/`

### Versioning (2 samples)
- `samples/versioning/Core_9/`

### Web (4 samples)
- `samples/web/asp-mvc-application/Core_9/`
- `samples/web/asp-web-application/Core_9/`
- `samples/web/blazor-server-application/Core_9/`
- `samples/web/send-from-aspnetcore-webapi/Core_9/`

## Recommended Upgrade Strategy

### Phase 1: DataBus Samples (Highest Priority)
These require code changes beyond package updates:
1. Add `NServiceBus.ClaimCheck` package reference
2. Update all namespace references
3. Rename `DataBusProperty<T>` to `ClaimCheckProperty<T>`
4. Update configuration: `UseDataBus()` → `UseClaimCheck()`
5. Consider message contract migration strategy

### Phase 2: Standard v9 Samples
For samples using NServiceBus v9:
1. Update NServiceBus package references to v10
2. Update related package versions (transports, persistence, etc.)
3. Update `TargetFramework` to `net10.0`
4. Test for behavioral changes
5. Update documentation

### Phase 3: Framework-Only Updates
For samples on .NET 8/9 but not using v9:
1. Update `TargetFramework` to `net10.0`
2. Verify compatibility

## Additional Notes

- **Line-level compatibility**: DataBus and ClaimCheck are line-level compatible for in-flight messages
- **Runtime compatibility**: Not runtime compatible - endpoints need matching property types
- **Migration path**: Use conventions or versioned contracts during transition
- **Custom persistence**: May need `IPersistenceDefinitionFactory` implementation updates
- **Async disposal**: Check for proper async disposal patterns in persistence implementations

## Reference

- [NServiceBus 9 to 10 Upgrade Guide](nservicebus/upgrades/9to10/index.md)
- [GitHub Milestone 320](https://github.com/Particular/NServiceBus/milestone/320?closed=1)
