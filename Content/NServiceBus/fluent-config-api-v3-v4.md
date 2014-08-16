---
title: Fluent Configuration API in V3 and V4
summary: NServiceBus Fluent Configuration API in V3 and V4
tags:
- NServiceBus
- Fluent Configuration
---

**NOTE**: This article refers to NServiceBus V3 and V4

An introduction to the NServiceBus configuration is available in the [Introduction to Fluent Configuration API in V3 and V4](fluent-config-api-v3-v4-intro) article. 

###Fluent Configuration API

The NServiceBus configuration entry point is the `Configure` class and its static `With()` method, each time we need to access an instance of the current configuration we can do that via the static `Instance` property of the `Configuration` class. 

####Entry Point Configuration

The `With()` method has several overloads each one resulting in the creation of a new Configuration instance.

* `With()`: Initializes a new configuration scanning all the assemblies found in the `bin` folder of the current application;
* `With(string probeDirectory)`: Initializes a new configuration scanning all the assemblies found in the given `probeDirectory` folder;
* `With(params Assembly[] assemblies)`: Initializes a new configuration scanning all the supplied assemblies; *NOTE*: the supplied asssemblies must contain also the NServiceBus binaries;
* `With(IEnumerable<Type> typesToScan)`: Initializes a new configuration scanning all the supplied types; *NOTE*: the supplied types must contain also all the NServiceBus types;

**NOTE**:

* Subsequent calls to the `With` method are idempotent and only one configuration is created;
* The `With` method, and in general the whole configuration API, is not thread safe; when configuring the entry point, make sure it's done in a thread safe manner, based on the host used:
	* For `IIS` configure NServiceBus in the `Application_Start()` method;
	* For `OWIN` configure NServiceBus in the `Startup()` method;
	* For self-hosted `WCF` services configure NServiceBus before opening the `ServiceHost`;

####Endpoint Naming

By default the endpoint name is deducted by the namespace of the assembly that contains the configuration entry point, we can customize the endpoint name via the Fluent Configuration API using the `DefineEndpointName` method:            

* `DefineEndpointName( string endpointName )`: defines the endpoint name using the supplied string; 

**NOTE**: If we need to customize the endpoint name via code, using the `DefineEndpointName` method, it is important to call it as the first one right after the `With()` configuration entry point.

To dive into the endpoint naming definition: [How To Specify Your Input Queue Name?](how-to-specify-your-input-queue-name)

####Dependency Injection

NServiceBus heavily relies on Dependency Injection to work properly, in order to initialize the built-in Inversion of Control container the `.DefaultBuilder()` method must be called.

It is also possible to instruct NServiceBus to use our own container to benefit of the dependency resolution event of our own custom types; refer to the [Containers](containers) article for the details on how to change the default container implementation.

####Transport

The way the NServiceBus transport is configured depends on the version of the binaries we are using.

In V3 the transport configuration is done via the `MsmqTransport()` method.

In V4, given the requirement to support multiple transports, the `UseTransport()` method can be called:

* `UseTransport<TTransport>( "connection string (optional)" )`: the generic overload of the UseTransport method can be invoked using as generic parameter a transport class and optionally passing in a transport connection string.
* `UseTransport( Type transportType, "connection string (optional)" )`: the non generic overload of the `UserTransport()` method accepts a `Type` instance that is the type of transport class and optionally the transport connection string.

The list of the built-in supported transport is available in the [NServiceBus Connection String Samples](connection-strings-samples) article.

####Unobtrusive Mode

When using NServiceBus we define our message contracts using plain C# classes or interfaces. For NServiceBus to find those classes when scanning our assemblies we need to mark them with the special `IMessage` interface, or the `ICommand` or `IEvent` interfaces that inherit from the `IMessage` one. This requirement creates a strong dependency on the NServiceBus assemblies and can cause versioning issues.

To completely overcome the problem NServiceBus can run in unobtrusive mode, meaning that we do not need to mark our messages with any interface and at configuration time we can define NServiceBus what is a message, a command or an event. 

* `DefiningMessagesAs( Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies the given predicate will be invoked to evaluate if the type should be considered a message or not. 
* `DefiningCommandsAs( Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies the given predicate will be invoked to evaluate if the type should be considered a command or not.
* `DefiningEventsAs(Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies the given predicate will be invoked to evaluate if the type should be considered an event or not.
* `DefiningExpressMessagesAs( Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies the given predicate will be invoked to evaluate if the type should be considered an [express message](how-do-i-specify-store-forward-for-a-message).
* `DefiningTimeToBeReceivedAs( Func<Type, TimeSpan> timeToBeReceivedHandler )`: for each type found in the scanned assemblies the given predicate will be invoked to determine the [time to be received](how-do-i-discard-old-messages) of each message, if any. 

NServiceBus can also define special behaviors for some message properties:

* `DefiningEncryptedPropertiesAs( Func<PropertyInfo, Boolean> predicate )`: for each property of each type the given predicate is invoked to determine if the property value should be encrypted before the message is delivered.
* `DefiningDataBusPropertiesAs( Func<PropertyInfo, Boolean> predicate )`: for each property of each type the given predicate is invoked to determine if the property value should be transported using the Data Bus instead of the defined transport.
                
To dive into the unobtrusive mode, data bus and encryption features:

* [Unobtrusive Mode Messages](unobtrusive-mode-messages).
* [Encryption Sample](encryption-sample).
* [DataBus / Attachments](attachments-databus-sample).

####DataBus

To configure the DataBus feature it is enough to call the `FileShareDataBus( string pathToSharedFolder )` passing as argument a path that must be accessible by all the endpoints that needs to share the same messages.

#####Azure and the DataBus

Endpoints running on Windows Azure cannot access UNC paths or file shares, in this case the [NServiceBus Azure Transport](http://www.nuget.org/packages/nservicebus.azure) provides its own DataBus implementation that can be configured using the `AzureDataBus()` method.

####Encryption

To configure the Encryption feature it is mandatory to define the encryption algorithm, the one supported out of the box by NServiceBus is Rijndael and can be configured calling the `RijndaelEncryptionService()` method.

####Logging

Logging in NServiceBus is achieved using Log4Net as logging library to configure the endpoint it is enough to call the `Log4Net()` method. More information on logging can be found in the [Logging in NServiceBus 4 and below](logging-in-nservicebus4_and_below) article.

####Fault Management

NServiceBus [manages fault](msmqtransportconfig) for us, to activate the fault manager it is enough to call the `MessageForwardingInCaseOfFault()` method.

####Performance Counters

To enable Performance Counters for a specific endpoint call the `EnablePerformanceCounters()` method. For more information on the NServiceBus performance counters refers to the [Performance Counters](monitoring-nservicebus-endpoints#nservicebus-performance-counters) article.

####Service Level Agreement

NServiceBus has the concept of [SLA](/servicepulse/monitoring-nservicebus-endpoints#service-level-agreement-sla-). The endpoint SLA can be defined using the `SetEndpointSLA( TimeSpan sla )` method.

####Persistance

Some NServiceBus features relies on a persistence storage to work properly, beginning with V3 the default persistence storage is RavenDB.

#####RavenDB Persistence

* `RavenPersistence()`: configures the endpoint to use RavenDB and expects to find a connection string, in the endpoint configuration file, named `NServiceBus/Persistence`.
* `RavenPersistence( 
*  connectionString )`: configures the endpoint to use RavenDB using the supplied RavenDB connection string.
* `RavenPersistence( Func<string> connectionStringProvider )`: configures the endpoint to use RavenDB and invokes the supplied delegate to get, at runtime, a valid RavenDB connection string.
* `RavenPersistence( Func<string> connectionStringProvider, string dbName )`: configures the endpoint to use RavenDB, invokes the supplied delegate to get, at runtime, a valid RavenDB connection string and expect as the second parameter the name of the database to use.
* `RavenPersistenceWithStore( IDocumentStore store )`: configures the endpoint to use RavenDB using the supplied IDocumentStore.
* `RavenSagaPersister()`: configures Sagas to use RavenDB as storage.
* `RavenSubscriptionStorage()`: configures the subscriptions manager to use RavenDB as storage.

A detailed explanation on how to connect to RavenDB can be found in the [Connecting to RavenDB from NServiceBus](using-ravendb-in-nservicebus-connecting) article.
                
#####NHibernate

Starting from NServiceBus V3 NHIbernate persistence is supported via a separate package:

* [Relational Persistence Using NHibernate in NServiceBus V3](relational-persistence-using-nhibernate);
* [Relational Persistence Using NHibernate in NServiceBus V4](relational-persistence-using-nhibernate---nservicebus-4.x);

#####In Memory Persistence

There are scenarios, such as the development environment or lightweight client not interested in durability across restarts, in which an in memory persistence configuration can be used:

* `InMemoryFaultManagement()`: configures the fault manager to run in memory.
* `InMemorySagaPersister()`: configures the saga persistence to run in memory.
* `InMemorySubscriptionStorage()`: configures the subscription manager to persist subscriptions in memory.

More details on all the persistence options can be found in the [Persistence in NServiceBus](persistence-in-nservicebus) article.

####MSMQ

We have also the option, when using MSMQ as a transport, to use one queue as the subscription storage, this is done invoking the `MsmqSubscriptionStorage()` method.

####License

There are several ways to assign the license to an endpoint, all detailed in the [How to install your license file](license-management) article. It is also supported to specify license at configuration time:

* `LicensePath( string partToLicenseFile )`: configures the endpoint to use the license file found at the supplied path;
* `License( string licenseText )`: configures the endpoint to use the supplied license, where the license text is the content of a license file.

####Queues Management

At configuration time it is possible to define some queue behavior:

* `PurgeOnStartup( Boolean purge )`: determines if endpoint queues should be purged at startup or not.
* `DoNotCreateQueues()`: configures the endpoint to not try to create queues at startup if they are not already created.

####Creating and Starting the Bus

Once the endpoint is configured the last step is to define the type of the bus we need and create it.

* `UnicastBus()`: defines that the bus will be an unicast bus, currently the only supported bus type.

####Creation

* `CreateBus()`: creates a startable bus ready to be started as required.
* `SendOnly()`: creates and start a send-only bus, suitable for a send-only endpoint that does not receive commands and does not handle events.

####Startup

If the created bus is not a send-only bus it must be started:

* `Start()`: starts the bus.
* `Start( Action startupAction )`: Starts the bus, invoking at startup time the supplied delegate.
