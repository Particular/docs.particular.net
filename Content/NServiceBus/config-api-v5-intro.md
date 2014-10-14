---
title: Introduction to Configuration API in V5
summary: Introduction to NServiceBus Configuration API in V5
tags:
- NServiceBus
- Fluent Configuration
---

NOTE: This article refers to NServiceBus V5

NOTE: Watch the webminar recording [Mastering NServiceBus Configuration](Mastering NServiceBus Configuration)

Every NServiceBus endpoint that works properly relies on a configuration to determine settings and behaviors other than endpoint core functionalities.

### NServiceBus Host

NServiceBus provides its own [hosting service][1] that can be used to outsource the whole hosting boilerplate code without worrying about how to deal with Windows services.

When using the built-in hosting service, the endpoint configuration is specified using the `EndpointConfig` class, automatically created when adding NServiceBus packages via NuGet, and implementing one of the core interfaces that determine endpoint behavior:

* `As_AServer`: Indicates that the endpoint behaves as a server. It is configured as a transactional endpoint that does not purge messages on startup, that can send commands and publish events;
* `As_APublisher`: Is now deprecated and all its functionalities are included in the `As_AServer` behavior;
* `As_AClient`: Indicates that the endpoint is a client.  A client endpoint is configured as a non-transactional endpoint that purges messages on startup.

```c#
public class EndpointConfiguration : IConfigureThisEndpoint, As_AServer
{
	public void Customize( BusConfiguration config )	{
	}
}
```

**IConfigureThisEndpoint**

The class in the example above represents the main entry point of an NServiceBus endpoint. Under the hood this is determined by the fact that the class implements the `IConfigureThisEndpoint` interface that identifies the class responsible for holding the initial configuration for an endpoint.

In V5 the `IConfigureThisEndpoint` interface requires us to implement a `Customize` method that will receive an instance of the current configuration. From a customization point of view this is the preferred and easiest way to customize the current bus configuration.

At runtime, during the startup phase, NServiceBus scans all the types, looking for a class that implements the `IConfigureThisEndpoint` interface.

More about the [NServiceBus.Host](the-nservicebus-host).

### Configuration Customization

When NServiceBus endpoints are hosted using the built-in NServiceBus host, you can customize the default configuration by adding a class to the project that implements the `INeedInitialization` interface. This class is invoked at runtime by the hosting process and is provided with the current configuration initialized by the host:

```c#
public class CustomConfiguration : INeedInitialization
{
	public void Customize( BusConfiguration config )
	{

	}
}
```

NOTE: Do not start the bus; the host will do it.

More about [configuration customization](customizing-nservicebus-configuration).

### Pipeline

NServiceBus V5 introduces a new message handling pipeline that allows to easily interact with all the various aspects and steps of the handling process of an incoming or of an outgoing message.

More about the [message handling pipeline](nservicebus-pipeline-intro).

### Features 

NServiceBus V4 has introduced the concept of features. A *feature* is a high level concept that encapsulates a set of settings related to a certain feature. Features can be enabled or disabled. Enabled features can be configured.

List of built-in features:

* `Audit`: The audit feature is responsible for message auditing. When enabled and configured, every received message is forwarded to the audit queue. By default, the feature is enabled but not configured;
* `AutoSubscribe`: This feature manages endpoint subscriptions in a pub/sub environment. It is enabled by default and  automatically subscribes to defined events;
* `Gateway`: more about [Gateway](introduction-to-the-gateway);
* `MessageDrivenSubscriptions`: brief description;
* `Sagas`: more about [Sagas](sagas-in-nservicebus);
* `SecondLevelRetries`: more about [Second Level Retries](second-level-retries);
* `StorageDrivenPublisher`: brief description;
* `TimeoutManager`: brief description;

You can also configure the serialization format through the feature API:

* `BinarySerialization`: binary serializer;
* `BsonSerialization`: BSON serializer;
* `JsonSerialization`: JSON serializer;
* `XmlSerialization`: XML serializer;

By default, if no configuration is performed in V5, the Json serializer is enabled.

NOTE: NServiceBus serializers operate only on primitive types.

To enable a feature, use this simple and straightforward API:

```c#
config.EnableFeature<TFeature>();
```

To disable a specific feature, call the `DisableFeature<TFeature>` method:

```c#
config.DisableFeature<TFeature>();
```

where the feature type is one of the feature classes defined in the `NServiceBus.Features` namespace and the `config` instance is the current bus configuration available at startup time.

### Self-Hosting Configuration

In some scenarios you need to self-host the bus without relying on the NServiceBus Host process. A well known sample scenario is a web application. When opting for self-hosting, you are responsible for configuring, creating, and starting the bus.
The configuration entry point is the `BusConfigure` class that exposes an API that allows fine grained control over all configuration settings.

Below is a sample console application with minimal code to create, initialize, and finally start NServiceBus.

```c#
public class Program
{
    public static void Main()
    {
        var config = new BusConfiguration();
        config.UsePersistence<InMemoryPersistence>();
        config.Conventions()
            .DefiningCommandsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Commands" ) )
            .DefiningEventsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Events" ) );
        
        using( var bus = Bus.CreateBus( config ).Start() )
        {
            //NServiceBus is up & running
        }    
    }
}
```
The differences with the V3 and V4 configuration API is much more evident in the self-hosting scenario where we can create from scratch a new `BusConfiguration` instance, configure only what we require and rely on default values for all the other settings.

V5 supports various transports (MSMQ, Azure, RabbitMQ, SQL, etc.) and the `UseTransport<TTransport>()` generic method defines the underlying transport that the bus instance will use. If not specified, as in the example above, the default transport is `MSMQ`.

More about [Configuration API](config-api-v5).

[1]: http://www.nuget.org/packages/NServiceBus.Host/ "NServiceBus Host NuGet package"
