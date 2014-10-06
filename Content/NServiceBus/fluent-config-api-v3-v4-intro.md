---
title: Introduction to Fluent Configuration API in V3 and V4
summary: Introduction to NServiceBus Fluent Configuration API in V3 and V4
tags:
- NServiceBus
- Fluent Configuration
---

NOTE: This article refers to NServiceBus V3 and V4

Every NServiceBus endpoint that works properly relies on a configuration to determine settings and behaviors other than endpoint core functionalities.

### NServiceBus Host

NServiceBus provides its own [hosting service][1] that can be used to outsource the whole hosting boilerplate code without worrying about how to deal with Windows services.

When using the built-in hosting service, the endpoint configuration is specified using the `EndpointConfig` class, automatically created when adding NServiceBus packages via NuGet, and implementing one of the core interfaces that determine endpoint behavior:

* `As_AServer`: Indicates that the endpoint behaves as a server. It is configured as a transactional endpoint that does not purge messages on startup;
* `As_APublisher`: Indicates that the endpoint is a publisher that can publish events, and extends the server role. An endpoint configured as a publisher cannot be configured as client at the same time;
* `As_AClient`: Indicates that the endpoint is a client.  A client endpoint is configured as a non-transactional endpoint that purges messages on startup.

```c#
public class EndpointConfiguration : IConfigureThisEndpoint, As_AServer
{
	
}
```

**IConfigureThisEndpoint**

The class in the example above represents the main entry point of an NServiceBus endpoint. Under the hood this is determined by the fact that the class implements the `IConfigureThisEndpoint` interface that identifies the class responsible for holding the initial configuration for an endpoint.

At runtime, during the startup phase, NServiceBus scans all the types, looking for a class that implements the `IConfigureThisEndpoint` interface.

More about the [NServiceBus.Host](the-nservicebus-host).

### Configuration Customization

When NServiceBus endpoints are hosted using the built-in NServiceBus host, you can customize the default configuration by adding a class to the project that implements the `IWantCustomInitialization` interface. This class is invoked at runtime by the hosting process and is provided with the default configuration initialized by the host:

```c#
public class CustomConfiguration : IWantCustomInitialization
{
	public void Init()
	{
		var currentConfig = Configure.Instance;
	}
}
```

NOTE: Do not start the bus; the host will do it. *`(comment - expand on this topic a bit)`*

More about [configuration customization](customizing-nservicebus-configuration).

### Features 

NOTE: The following section applies to NServiceBus V4 (or later)

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

By default, if no configuration is performed, the XML serializer is enabled.

NOTE: Only one serializer can be enabled in an endpoint at a time.

NOTE: NServiceBus serializers operate only on primitive types.

To enable a feature, use this simple and straightforward API:

```c#
Configure.Features.Enable<TFeature>();
```

To disable a specific feature, call the `Disable<TFeature>` method:

```c#
Configure.Features.Disable<TFeature>();
```

where the feature type is one of the feature classes defined in the `NServiceBus.Features` namespace.

### Self-Hosting Configuration

In some scenarios you need to self-host the bus without relying on the NServiceBus Host process. A well known sample scenario is a web application. When opting for self-hosting, you are responsible for configuring, creating, and starting the bus.
The configuration entry point is the `Configure` class that exposes a Fluent configuration API that allows fine grained control over all configuration settings.

#### Self-Hosting in V4

Below is a sample console application with minimal code to create, initialize, and finally start NServiceBus.

```c#
public class Program
{
    public static void Main()
    {
        var bus = Configure.With()
	        .DefaultBuilder()
	        .UsingTransport<Msmq>()
	        .UnicastBus()
	        .CreateBus()
	        .Start();
	}
}
```

More on [Self-hosting in v4](hosting-nservicebus-in-your-own-process-v4.x).

#### Self-Hosting in V3

```c#
public class Program
{
    public static void Main()
    {
        var bus = Configure.With()
	        .DefaultBuilder()
	        .MsmqTransport()
	        .UnicastBus()
	        .CreateBus()
	        .Start();
	}
}
```

More on [Self-hosting in V3](hosting-nservicebus-in-your-own-process).

The `With()` method is the main configuration entry point that initializes and starts the configuration engine, followed by a call to `DefaultBuilder()`, which instructs the configuration engine to use the built-in inversion of the control container to manage dependencies. More on [NServiceBus and IoC containers](containers).

V4 supports various transports (MSMQ, Azure, RabbitMQ, SQL, etc.) and the `UsingTransport<TTransport>()` generic method defines the underlying transport that the bus instance will use. The example above uses MSMQ. In V3 the only supported transport was MSMQ, thus the only viable option was to use the `MsmqTransport()` method.

Finally, you define that the bus will be a unicast bus, which is the only option currently available, and then create and start the bus.

More about [Fluent Configuration API](fluent-config-api-v3-v4).

[1]: http://www.nuget.org/packages/NServiceBus.Host/ "NServiceBus Host NuGet package"
