---
title: Transport configuration changes
summary: Transport configuration changes from NServiceBus 7 to 8.
reviewed: 2021-02-12
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

## Transport Specific Sonfiguration

See the transport specific upgrade guides for further details on the configuration options:

* [Azure Service Bus transport upgrade guide](/transports/upgrades/asbs-1to2.md)
* [Azure Storage Queues transport upgrade guide](TODO)
* [RabbitMQ transport upgrade guide](TODO)
* [MSMQ transport upgrade guide](/transports/upgrades/msmq-1to2.md)
* [SQL Server transport upgrade guide](TODO)
* [Amazon SQS transport upgrade guide](TODO)

## Transaction Configuration

Instead of the `Transactions` method, use the `TransportTransactionMode` property on the transport configuration instance to configure the desired transaction mode.

```
var transportConfiguration = new MyTransport{
    TransportTransactionMode = TransportTransactionMode.ReceiveOnly
};
endpointConfiguration.UseTransport(transportConfiguration);
```

## Routing Configuration

Routing can be configured on the `RoutingSettings` returned from the `UseTransport` method.

```
var routing = endpointConfiguration.UseTransport(transportConfiguration);
routing.RouteToEndpoint(typeof(MyMessage), "DestinationEndpoint");
```

## Obsoleted Configuration Options

The following configuration operations have been obsoleted. See the transport specific upgrade guides for further information about the availablity of these configuration options.

* `EndpointConfiguration.DoNotCreateQueues`
* `TransportExtensions.ConnectionString`

## Transport APIs

The following low-level transport APIs have been renamed:

* `IDispatchMessages` has been renamed to `IMessageDispatcher`
* `IReceiveMessages` has been renamed to `IMessageReceiver`
* `IManageSubscriptions` has been renamed to `ISubscriptionManager`

## Subscription Authorization

The `SubscriptionAuthorizer` method is now available on the `RoutingSettings`:

```
var routing = endpointConfiguration.UseTransport(transportConfiguration);
routing.SubscriptionAuthorizer(context => <...>);
```
