---
title: Fluent Configuration API in V3 and V4
summary: NServiceBus Fluent Configuration API in V3 and V4
tags:
- NServiceBus
- Fluent Configuration
---

NOTE: This article refers to NServiceBus V3 and V4

NOTE: Watch the webminar recording [Mastering NServiceBus Configuration](https://particular-1.wistia.com/medias/q8tdr6mnzz) (It includes V5 configuration overview)

An introduction to the NServiceBus configuration is available in the [Introduction to Fluent Configuration API in V3 and V4](fluent-config-api-v3-v4-intro.md) article. 

### Fluent Configuration API

The NServiceBus configuration entry point is the `Configure` class and its static `With()` method. Each time you need to access an instance of the current configuration, use the static `Instance` property of the `Configuration` class. 

#### Entry Point Configuration

The `With()` method has several overloads, each resulting in the creation of a new configuration instance.

* `With()`: Initializes a new configuration, scanning all the assemblies found in the `bin` folder of the current application;
* `With(string probeDirectory)`: Initializes a new configuration, scanning all the assemblies found in the given `probeDirectory` folder;
* `With(params Assembly[] assemblies)`: Initializes a new configuration, scanning all the supplied assemblies; *NOTE*: The supplied assemblies must also contain the NServiceBus binaries;
* `With(IEnumerable<Type> typesToScan)`: Initializes a new configuration, scanning all the supplied types; *NOTE*: The supplied types must also contain all the NServiceBus types;

#### Subsequent calls

* Subsequent calls to the `With` method are idempotent and only one configuration is created;
* The `With` method (and in general the whole configuration API) is not thread safe; when you configure the entry point, make sure it is thread safe, based on the host used:
	* For `IIS`, configure NServiceBus in the `Application_Start()` method;
	* For `OWIN`, configure NServiceBus in the `Startup()` method;
	* For self-hosted `WCF` services, configure NServiceBus before opening the `ServiceHost`;

#### Endpoint Naming

By default, the endpoint name is deduced by the namespace of the assembly that contains the configuration entry point. You can customize the endpoint name via the Fluent Configuration API using the `DefineEndpointName` method:            

* `DefineEndpointName(string endpointName)`: defines the endpoint name using the supplied string; 

NOTE: If you need to customize the endpoint name via code using the `DefineEndpointName` method, it is important to call it first, right after the `With()` configuration entry point.

To dive into the endpoint naming definition, read [How To Specify Your Input Queue Name?](how-to-specify-your-input-queue-name.md).

#### Dependency Injection

NServiceBus relies heavily on Dependency Injection to work properly. To initialize the built-in Inversion of Control container, call the `.DefaultBuilder()` method.

You can also instruct NServiceBus to use your container to benefit from the dependency resolution event of your custom types. For details on how to change the default container implementation, refer to the [Containers](containers.md) article.

#### Transport

The configuration of the NServiceBus transport depends on the version of the binaries you are using.

In V3, configure the transport configuration using the `MsmqTransport()` method.

In V4, given the requirement to support multiple transports, call the `UseTransport()` method:

* `UseTransport<TTransport>( "connection string (optional)" )`: the generic overload of the UseTransport method can be invoked using a transport class as generic parameter and optionally passing in a transport connection string.
* `UseTransport( Type transportType, "connection string (optional)" )`: the non-generic overload of the `UseTransport()` method accepts a `Type` instance that is the type of transport class and optionally the transport connection string.

The list of the built-in supported transport is available in the [NServiceBus Connection String Samples](connection-strings-samples.md) article.

#### Unobtrusive Mode

Because plain C# classes or interfaces define message contracts, for NServiceBus to find those classes when scanning assemblies, you need to mark them with the special `IMessage` interface, or the `ICommand` or `IEvent` interfaces that inherit from the `IMessage` one. This requirement creates a strong dependency on the NServiceBus assemblies and can cause versioning issues. To completely overcome the problem, NServiceBus can run in unobtrusive mode, meaning that you do not need to mark your messages with any interface and at configuration time you can define messages, commands, and events for NServiceBus: 

* `DefiningMessagesAs( Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies, the given predicate will be invoked to evaluate if the type should be considered a message or not. 
* `DefiningCommandsAs( Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies, the given predicate will be invoked to evaluate if the type should be considered a command or not.
* `DefiningEventsAs(Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies, the given predicate will be invoked to evaluate if the type should be considered an event or not.
* `DefiningExpressMessagesAs( Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies, the given predicate will be invoked to evaluate if the type should be considered an [express message](how-do-i-specify-store-forward-for-a-message.md).
* `DefiningTimeToBeReceivedAs( Func<Type, TimeSpan> timeToBeReceivedHandler )`: for each type found in the scanned assemblies, the given predicate will be invoked to determine the [time to be received](how-do-i-discard-old-messages.md) of each message, if any. 

NServiceBus can also define special behaviors for some message properties:

* `DefiningEncryptedPropertiesAs( Func<PropertyInfo, Boolean> predicate )`: for each property of each type, invoke the given predicate to determine if the property value should be encrypted before the message is delivered.
* `DefiningDataBusPropertiesAs( Func<PropertyInfo, Boolean> predicate )`: for each property of each type, invoke the given predicate to determine if the property value should be transported using the data bus instead of the defined transport.
                
To dive into the unobtrusive mode, data bus, and encryption features:

* [Unobtrusive Mode Messages](unobtrusive-mode-messages.md).
* [Encryption Sample](encryption-sample.md).
* [DataBus / Attachments](attachments-databus-sample.md).
* [Encryption](encryption.md)

#### DataBus

To configure the DataBus feature, call the `FileShareDataBus( string pathToSharedFolder )` passing as argument a path that must be accessible by all the endpoints that need to share the same messages.

##### Azure and the DataBus

Endpoints running on Windows Azure cannot access UNC paths or file shares. In this case the [NServiceBus Azure Transport](http://www.nuget.org/packages/nservicebus.azure) provides its own DataBus implementation that you can configure using the `AzureDataBus()` method.

#### Encryption

To configure the encryption feature you must define the encryption algorithm. NServiceBus supports Rijndael out of the box and you can configure it by calling the `RijndaelEncryptionService()` method.

#### Logging

You can log NServiceBus using Log4Net as the logging library. To configure the endpoint simply call the `Log4Net()` method. More information on logging is in the [Logging in NServiceBus 4 and below](logging-in-nservicebus4-and-below.md) article.

#### Fault Management

NServiceBus [manages fault](msmqtransportconfig.md). To activate the fault manager, call the `MessageForwardingInCaseOfFault()` method.

#### Performance Counters

To enable Performance Counters for a specific endpoint, call the `EnablePerformanceCounters()` method. For more information on  NServiceBus performance counters, read the [Performance Counters](monitoring-nservicebus-endpoints.md#nservicebus-performance-counters) article.

#### Service Level Agreement

NServiceBus has the concept of [SLA](/servicepulse/monitoring-nservicebus-endpoints.md#service-level-agreement-sla-). The endpoint SLA can be defined using the `SetEndpointSLA( TimeSpan sla )` method.

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

For a detailed explanation on how to connect to RavenDB, read the [Connecting to RavenDB from NServiceBus](using-ravendb-in-nservicebus-connecting.md) article.
                
##### NHibernate

Starting from NServiceBus V3, NHIbernate persistence is supported via a separate package:

* [Relational Persistence Using NHibernate in NServiceBus](relational-persistence-using-nhibernate.md);

##### In Memory Persistence

Some scenarios require an in-memory persistence configuration, such as the development environment or a lightweight client not interested in durability across restarts:

* `InMemoryFaultManagement()`: configures the fault manager to run in memory.
* `InMemorySagaPersister()`: configures the saga persistence to run in memory.
* `InMemorySubscriptionStorage()`: configures the subscription manager to persist subscriptions in memory.

Details of all the persistence options are in the [Persistence in NServiceBus](persistence-in-nservicebus.md) article.

#### MSMQ

When using MSMQ as a transport you can use one queue as the subscription storage by invoking the `MsmqSubscriptionStorage()` method.

#### License

The methods of assigning the license to an endpoint are all detailed in the [How to install your license file](license-management.md) article. You can also specify a license at configuration time:

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
