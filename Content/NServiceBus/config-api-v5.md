---
title: Configuration API in V5
summary: NServiceBus Configuration API in V5
tags:
- NServiceBus
- Fluent Configuration
---

NOTE: This article refers to NServiceBus V5

NOTE: Watch the webminar recording [Mastering NServiceBus Configuration](Mastering NServiceBus Configuration)

An introduction to the NServiceBus configuration is available in the [Introduction to Configuration API in V5](config-api-v5-intro) article. 

### Configuration API

NServiceBus V5 introduces a new configuration API to overcome limitations of the previous approach. The new configuration engine is a two step configuration engine where at startup time a new configration can be defined and finally used to create an `IBus` instance that will rely on a set of settings built given the original configuration, the `IBus` runtime settings are `read-only` and can only be changed recreating the bus.

The major change introduced in V5 is that NServiceBus V5 endpoints can now host multiple bus instances running different configurations. 

#### Configuration Entry Point

The NServiceBus configuration entry point is the `BusConfiguration` class. In a self-hosting scenario we can manually create an instance of the `BusConfiguration` class, in a scenario where we are using the `NServiceBus.Host` hosting service a new instance is created by the hosting engine and will be given to the endpoint configuration class at startup time.   

If we need to specify which assemblies should be scanned at startup time we can rely on the `AssembliesToScan()` method; in order to specify which types should be scanned we can rely on the `TypesToScan()` method.

*NOTE*: The supplied assemblies/types must also contain all the NServiceBus assemblies or types;

#### Endpoint Naming

By default, the endpoint name is deduced by the namespace of the assembly that contains the configuration entry point. You can customize the endpoint name via the Configuration API using the `EndpointName()` method:            

* `EndpointName( string endpointName )`: defines the endpoint name using the supplied string; 

To dive into the endpoint naming definition, read [How To Specify Your Input Queue Name?](how-to-specify-your-input-queue-name).

#### Dependency Injection

NServiceBus relies heavily on Dependency Injection to work properly. By default the built-in Inversion of Control container will be used.

You can also instruct NServiceBus to use your container to benefit from the dependency resolution event of your custom types. For details on how to change the default container implementation, refer to the [Containers](containers) article.

#### Transport

The configuration of the NServiceBus transport is made via the `UseTransport()` method of the configuration API.

* `UseTransport<TTransport>()`: the generic overload of the UseTransport method can be invoked using a transport class as generic parameter.
* `UseTransport( Type transportType )`: the non-generic overload of the `UseTransport()` method accepts a `Type` instance that is the type of transport class.

In both cases the call to `UseTransport()` will return a `TransportExtensions` instance that allows the configuration of the transport connection string, via the `ConnectionString()` method, orthe transport connection string name via the `ConnectionStringName()` method.

The list of the built-in supported transport is available in the [NServiceBus Connection String Samples](connection-strings-samples) article.

#### Unobtrusive Mode

In NServiceBus V5 message interfaces, such as `IMessage`, `ICommand` or `IEvent` are deprecated in order to prevent a strong dependency on the NServiceBus assemblies and cause versioning issues. NServiceBus now relies only on unobtrusive conventions configuration that can be accessed via the `Conventions()` method on the `BusConfiguration` instance. The `Conventions()` method will return an instance of the `ConventionsBuilder` class that exposes the following methods: 

* `DefiningMessagesAs( Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies, the given predicate will be invoked to evaluate if the type should be considered a message or not. 
* `DefiningCommandsAs( Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies, the given predicate will be invoked to evaluate if the type should be considered a command or not.
* `DefiningEventsAs(Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies, the given predicate will be invoked to evaluate if the type should be considered an event or not.
* `DefiningExpressMessagesAs( Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies, the given predicate will be invoked to evaluate if the type should be considered an [express message](how-do-i-specify-store-forward-for-a-message).
* `DefiningTimeToBeReceivedAs( Func<Type, TimeSpan> timeToBeReceivedHandler )`: for each type found in the scanned assemblies, the given predicate will be invoked to determine the [time to be received](how-do-i-discard-old-messages) of each message, if any. 

NServiceBus can also define special behaviors for some message properties:

* `DefiningEncryptedPropertiesAs( Func<PropertyInfo, Boolean> predicate )`: for each property of each type, invoke the given predicate to determine if the property value should be encrypted before the message is delivered.
* `DefiningDataBusPropertiesAs( Func<PropertyInfo, Boolean> predicate )`: for each property of each type, invoke the given predicate to determine if the property value should be transported using the data bus instead of the defined transport.
                
To dive into the unobtrusive mode, data bus, and encryption features:

* [Unobtrusive Mode Messages](unobtrusive-mode-messages).
* [Encryption Sample](encryption-sample).
* [DataBus / Attachments](attachments-databus-sample).
* [Encryption with Multi-Key Decryption](encryption-with-multi-key-decryption.md)

#### DataBus

To configure the DataBus feature, call the `FileShareDataBus( string pathToSharedFolder )` passing as argument a path that must be accessible by all the endpoints that need to share the same messages.

##### Azure and the DataBus

Endpoints running on Windows Azure cannot access UNC paths or file shares. In this case the [NServiceBus Azure Transport](http://www.nuget.org/packages/nservicebus.azure) provides its own DataBus implementation that you can configure using the `AzureDataBus()` method.

#### Encryption

To configure the encryption feature you must define the encryption algorithm. NServiceBus supports Rijndael out of the box and you can configure it by calling the `RijndaelEncryptionService()` method. If we need to plugin our own encryption service we can invoke the `RegisterEncryptionService()` that accepts a delegate that will be invoked at runtime when an instance of the current `IEncryptionService` is required.

#### Logging

NServiceBus V5 has its own internal logging implementation.

#### Fault Management

?????

#### Performance Counters

???? Enabled by default, no way to control it?

To enable Performance Counters for a specific endpoint, call the `EnablePerformanceCounters()` method. For more information on  NServiceBus performance counters, read the [Performance Counters](monitoring-nservicebus-endpoints#nservicebus-performance-counters) article.

#### Service Level Agreement

NServiceBus has the concept of [SLA](/servicepulse/monitoring-nservicebus-endpoints#service-level-agreement-sla-). The endpoint SLA can be defined using the `EnableSLAPerformanceCounter( TimeSpan sla )` method.














#### Persistence

Some NServiceBus features rely on persistence storage to work properly. Beginning with V3 the default persistence storage is RavenDB.

##### RavenDB Persistence

* `RavenPersistence()`: configures the endpoint to use RavenDB and expects to find a connection string in the endpoint configuration file, named `NServiceBus/Persistence`.
* `RavenPersistence( 
*  connectionString )`: configures the endpoint to use RavenDB using the supplied RavenDB connection string.
* `RavenPersistence( Func<string> connectionStringProvider )`: configures the endpoint to use RavenDB and invokes the supplied delegate to get a valid RavenDB connection string at runtime.
* `RavenPersistence( Func<string> connectionStringProvider, string dbName )`: configures the endpoint to use RavenDB, invokes the supplied delegate to get a valid RavenDB connection string at runtime, and expects the name of the database as the second parameter.
* `RavenPersistenceWithStore( IDocumentStore store )`: configures the endpoint to use RavenDB using the supplied IDocumentStore.
* `RavenSagaPersister()`: configures sagas to use RavenDB as storage.
* `RavenSubscriptionStorage()`: configures the subscription manager to use RavenDB as storage.

For a detailed explanation on how to connect to RavenDB, read the [Connecting to RavenDB from NServiceBus](using-ravendb-in-nservicebus-connecting) article.
                
##### NHibernate

NHibernate persistence is supported via a separate package:

* [Relational Persistence Using NHibernate in NServiceBus V3](relational-persistence-using-nhibernate);
* [Relational Persistence Using NHibernate in NServiceBus V4](relational-persistence-using-nhibernate---nservicebus-4.x);

##### In Memory Persistence

Some scenarios my benefit from an in-memory persistence configuration, such as the development environment or a lightweight client not interested in durability across restarts:

* `InMemoryFaultManagement()`: configures the fault manager to run in memory.
* `InMemorySagaPersister()`: configures the saga persistence to run in memory.
* `InMemorySubscriptionStorage()`: configures the subscription manager to persist subscriptions in memory.

Details of all the persistence options are in the [Persistence in NServiceBus](persistence-in-nservicebus) article.

#### MSMQ

When using MSMQ as a transport you can use one queue as the subscription storage by invoking the `MsmqSubscriptionStorage()` method.

#### License

The methods of assigning the license to an endpoint are all detailed in the [How to install your license file](license-management) article. You can also specify a license at configuration time:

* `LicensePath( string partToLicenseFile )`: configures the endpoint to use the license file found at the supplied path;
* `License( string licenseText )`: configures the endpoint to use the supplied license, where the license text is the content of a license file.

####Queue Management

At configuration time it is possible to define queue behavior:

* `PurgeOnStartup( Boolean purge )`: determines if endpoint queues should be purged at startup.
* `DoNotCreateQueues()`: configures the endpoint to not try to create queues at startup if they are not already created.

#### Creating and Starting the Bus

Once the endpoint is configured the last step is to define the type of the bus and create it.

* `UnicastBus()`: defines that the bus is a unicast bus, currently the only supported bus type.

#### Creation

* `CreateBus()`: creates a startable bus ready to be started as required.
* `SendOnly()`: creates and starts a send-only bus, suitable for a send-only endpoint that does not receive commands and does not handle events.

#### Startup

If the created bus is not a send-only bus it must be started:

* `Start()`: starts the bus.
* `Start( Action startupAction )`: Starts the bus, invoking the supplied delegate at startup time.





### Resources

[Customizing NServiceBus Configuration](customizing-nservicebus-configuration)