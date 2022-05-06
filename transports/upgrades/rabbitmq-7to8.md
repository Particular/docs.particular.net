---
title: RabbitMQ Transport Upgrade Version 7 to 8
summary: Migration instructions on how to upgrade RabbitMQ Transport from Version 7 to 8.
reviewed: 2022-05-05
component: Rabbit
related:
- transports/rabbitmq
- nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

## Timeout manager

The [timeout manager has been removed from core](/nservicebus/upgrades/7to8/#timeout-manager-removed) which makes timeout manager backward compatibility mode obsolete. If backward compatibility mode was enabled these APIs must be removed.

## Configuring the RabbitMQ transport

To use the RabbitMQ transport for NServiceBus, create a new instance of the `RabbitMQTransport` and pass it to `EndpointConfiguration.UseTransport`.

Instead of:

```csharp
var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
transport.ConnectionString(connectionString);
```

Use:

```csharp
var transport = new RabbitMQTransport(Topology.Conventional, connectionString);
endpointConfiguration.UseTransport(transport);
```

The mandatory configuration settings, the topology and the connection string, are now required to construct the instance of the transport definition class.

include: v7-usetransport-shim-api

## Certificate path and passphrase

The certificate file path and passphrase can now be passed only via the connection string. When configuring a secure connection via the API, the only option is to pass an instance of the `X505Certificate2` class. This instance can be constructed using a path and passphrase.

Instead of this code:

snippet: 7to8certificatepath7

Use this:

snippet: 7to8certificatepath8

## Prefetch count

The two prefetch count settings have been replaced with a single setting that uses a callback. Instead of either of these APIs:

snippet: 7to8prefetchcount7

Use one of these:

snippet: 7to8prefetchcount8

## Disabling the durable exchanges and queues

Disabling the durable exchanges and queues has been moved to the constructor of the topology classes, `ConventionalRoutingTopology` and `DirectRoutingTopology`. In order to set the value of that parameter use the variant of the `RabbitMQTransport` constructor that accepts an instance of the topology.

## Configuration options

The RabbitMQ transport configuration options that have not changed have been moved to the `RabbitMQTransport` class. See the following table for further information:

| Version 6 configuration option | Version 7 configuration option |
| --- | --- |
| CustomMessageIdStrategy | MessageIdStrategy |
| DisableRemoteCertificateValidation | ValidateRemoteCertificate |
| SetClientCertificate | ClientCertificate |
| SetHeartbeatInterval | HeartbeatInterval |
| SetNetworkRecoveryInterval | NetworkRecoveryInterval |
| TimeToWaitBeforeTriggeringCircuitBreaker | TimeToWaitBeforeTriggeringCircuitBreaker |
| UseExternalAuthMechanism | UseExternalAuthMechanism |
