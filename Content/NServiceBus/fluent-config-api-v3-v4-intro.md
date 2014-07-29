---
title: Introduction to Fluent Configuration API in V3 and V4
summary: Introduction to NServiceBus Fluent Configuration API in V3 and V4
tags:
- NServiceBus
- Fluent Configuration
---

**NOTE**: This article refers to NServiceBus V3 and V4

Every NServiceBus endpoint to work properly relies on a configuration that determines settings and behaviors other than endpoint core functionalities.

###NServiceBus Host

NServiceBus provides its own hosting service (link to the NuGet package) that can be used to outsource the whole hosting "drama" (find a synonym) without worrying about how to deal with Windows Services.

When using the built in hosting services the endpoint configuration is specified using the EndpointConfig class, automatically created when adding NServiceBus packages via NuGet, and implementing one of the core interfaces that determine the default endpoint behavior:

* `*As_AServer*`: Indicates that the endpoint behaves as a server. It will be set up as a transactional endpoint that won't purge messages on startup;
* `*As_APublisher*`: Indicates that the endpoint is a publisher and extends the server role. A publisher can also publish events, an endpoint configured as a publisher cannot be configured as client at the same time;
* `*As_AClient*`: Indicates that the endpoint is a client.  A client endpoint will be set up as a non-transactional endpoint that will purge messages on startup.

```
public class EndpointConfiguration : IConfigureThisEndpoint, As_AServer
{
	}
```

**IConfigureThisEndpoint**

The above class represents the main entry point of the NServiceBus endpoint, this is under the hood determined by the fact that the class implements the `IConfigureThisEndpoint` interface that identifies the class responsible to hold the initial endpoint configuration.

At runtime, during the startup phase, NServiceBus scans all the types looking for a class that implements the `IConfigureThisEndpoint` interface.

More about the [NServiceBus.Host](the-nservicebus-host)

###Configuration customization

When NServiceBus endpoints are hosted using the built in NServiceBus host it is possible to customize the default configuration adding to the project a class that implements the IWantCustomInitialization interface, this class will be invoked at runtime by the hosting process and will be provided with a default configuration initialized by the host and ready to be configured as required.

```
public class CustomConfiguration : IWantCustomInitialization
{
	public void Init()	{
		var currentConfig = Configure.Instance;
	}}
```

**NOTE**: Do not start the bus it will be done by the host *`(comment - expand on this topic a bit)`*

More about [configuration customization](customizing-nservicebus-configuration)

###Features (V4 only)

NServiceBus in V4 has introduced the concept of features, features are a high level concept that encapsulate a set of settings related to a certain feature, thus features can be entirely enabled or disabled and when enabled configured.

List of built-in features

* `Audit`: The audit feature is responsible for message auditing, every received message will be forwarded to the audit queue when this feature is enabled and configured, by default the feature is enabled but not configured;
* `AutoSubscribe`: This feature menages endpoint subscriptions in a pub/sub environment, is enabled by default and will subscribe automatically to defined events;
* `Gateway`: brief description;
* `MessageDrivenSubscriptions`: brief description;
* `Sagas`: brief description;
* `SecondLevelRetries`: brief description;
* `StorageDrivenPublisher`: brief description;
* `TimeoutManager`: brief description;

Through the feature API it is also possible to configure the serialization format:

* `BinarySerialization`: brief description;
* `BsonSerialization`: brief description;
* `JsonSerialization`: brief description;
* `XmlSerialization`: brief description;

By default if no configuration is performed the XML serializer is enabled.

**NOTE**: Only one serializer can be enabled in an endpoint at a time.

To enable a feature there is a simple and straightforward API:

    Configure.Features.Enable<*TFeature*>();

To disable a specific feature call the `Disable<*TFeature*>` method:

    Configure.Features.Disable<*TFeature*>();

Where the feature type is one of the feature classes defined in the `NServiceBus.Features` namespace.

###Self-hosting configuration

There are scenarios in which we need to self host the bus without being able to rely on the NServiceBus Host process, a well known sample scenario is a web application, in these cases we are responsible to configure, create and start the bus.
The configuration entry point point is the Configure class that exposes a fluent configuration API that let us take fine control of all the configuration settings.

####Self hosting in V4

```
public class Program
{
    public static void Main()    {
        var bus = Configure.With()
	        .DefaultBuilder()
	        .UsingTransport<Msmq>()
	        .UnicastBus()
	        .CreateBus()
	        .Start();
	}}
```

More on [Self-hosting in v4](hosting-nservicebus-in-your-own-process-v4.x)

####Self hosting in V3

```
public class Program
{
    public static void Main()    {
        var bus = Configure.With()
	        .DefaultBuilder()
	        .MsmqTransport()
	        .UnicastBus()
	        .CreateBus()
	        .Start();
	}}
```

More on [Self-hosting in v3](hosting-nservicebus-in-your-own-process-v3.x)

The above samples, from a console application, is the minimum required to create, initialize and finally start the bus in a self hosting scenario.

The `With()` method is the main configuration entry point that initialize and starts the configuration engine, right after that the call to `DefaultBuilder()` instructs the configuration engine to use the built-in inversion of control container to manage dependencies.

V4 supports multiple transports and the `UsingTransport<TTransport>()` generic method defines what will be the underlying transport that the bus instance will use, in the above sample we are using MSMQ.

In V3 the only supported transport was MSMQ thus the only viable option was to use the `MsmqTransport()` method.

Finally we define that the bus will be a unicast bus, the only option currently available, and we create and start the bus.

More about [Fluent Configuration API](fluent-config-api-v3-v4)