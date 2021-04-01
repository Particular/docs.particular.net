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

NServiceBus V8 comes with a new transport configuration API. Instead of the generic-based `UseTransport<TTransport>` method, create an instance of the transport's configuration class and pass it to the `UseTransport` method. For example, instead of:

```csharp
var transport = endpointConfiguration.UseTransport<MyTransport>();
transport.Transactions(TransportTransactionMode.ReceiveOnly);
var routing = t.Routing();
routing.RouteToEndpoint(typeof(MyMessage), "DestinationEndpoint");
```

use:

```csharp
var transport = new MyTransport{
    TransportTransactionMode = TransportTransactionMode.ReceiveOnly
};
var routing = endpointConfiguration.UseTransport(transport);
routing.RouteToEndpoint(typeof(MyMessage), "DestinationEndpoint");
```

## Transport specific configuration

See the transport-specific upgrade guides for further details on the configuration options:

* [Azure Service Bus transport upgrade guide](/transports/upgrades/asbs-1to2.md)
* [Azure Storage Queues transport upgrade guide](/transports/upgrades/asq-10to11.md)
* [RabbitMQ transport upgrade guide](/transports/upgrades/rabbitmq-6to7.md)
* [MSMQ transport upgrade guide](/transports/upgrades/msmq-1to2.md)
* [SQL Server transport upgrade guide](/transports/upgrades/sqlserver-6to7.md)
* [Amazon SQS transport upgrade guide](/transports/upgrades/amazonsqs-5to6.md)

## Backwards-compatible API

The existing API transport configuration API is supported for this major version via a shim API. This shim API emulates the replaced configuration API surface but uses different types. Therefore, custom code that refers to the configuration API types might need to be updated to use the shim API types instead.

## Transaction configuration

Instead of the `Transactions` method, use the `TransportTransactionMode` property on the transport configuration instance to configure the desired transaction mode.

```
var transportConfiguration = new MyTransport{
    TransportTransactionMode = TransportTransactionMode.ReceiveOnly
};
endpointConfiguration.UseTransport(transportConfiguration);
```

## Connection strings

Configuring a transport's connection using `.ConnectionStringName(name)`, which was removed for .NET Core in NServiceBus version 7, has been removed from all platforms in NServiceBus version 8. To retrieve the connection string by the named value in the configuration, first retrieve the connection string and then pass it to the transport configuration API.

A connection string named `NServiceBus/Transport` will **no longer be detected automatically** on any platform. The connection string value must be configured explicitly using `.ConnectionString(value)`.

## Routing configuration

Routing can be configured on the `RoutingSettings` object returned from the `UseTransport` method.

```
var routing = endpointConfiguration.UseTransport(transportConfiguration);
routing.RouteToEndpoint(typeof(MyMessage), "DestinationEndpoint");
```

## Subscription authorization

The `SubscriptionAuthorizer` method is now available on the `RoutingSettings`:

```
var routing = endpointConfiguration.UseTransport(transportConfiguration);
routing.SubscriptionAuthorizer(context => <...>);
```

Note: Subscription Authorization is only available for transports using [message-driven publish-subscribe](/nservicebus/messaging/publish-subscribe/#mechanics-message-driven-persistence-based)

## Renamed APIs

The following low-level transport APIs have been renamed:

* `IDispatchMessages` has been renamed to `IMessageDispatcher`
* `IReceiveMessages` has been renamed to `IMessageReceiver`
* `IManageSubscriptions` has been renamed to `ISubscriptionManager`

## Obsolete configuration options

The following configuration operations are obsolete in NServiceBus version 8. See the transport-specific upgrade guides for further information about the availablity of these configuration options.

* `EndpointConfiguration.DoNotCreateQueues`
* `TransportExtensions.ConnectionString`
