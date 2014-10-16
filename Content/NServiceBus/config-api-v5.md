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

NServiceBus V5 introduces a new configuration API to overcome limitations of the previous approach. The new configuration engine is a two step configuration engine where at startup time a new configuration can be defined and finally used to create an `IBus` instance that will rely on a set of settings built given the original configuration, the `IBus` runtime settings are `read-only` and can only be changed recreating the bus.

The major change introduced in V5 is that NServiceBus V5 endpoints can now host multiple bus instances running different configurations. 

#### Configuration Entry Point

The NServiceBus configuration entry point is the `BusConfiguration` class. In a self-hosting scenario we can manually create an instance of the `BusConfiguration` class, in a scenario where we are using the `NServiceBus.Host` hosting service a new instance is created by the hosting engine and will be given to the endpoint configuration class at startup time.   

If we need to specify which assemblies should be scanned at startup time we can rely on the `AssembliesToScan()` method; in order to specify which types should be scanned we can rely on the `TypesToScan()` method.

It is also possible to define a custom probing directory to override the default one, that is the one where the process is running from. In order to change the probing directory call the `ScanAssembliesInDirectory` method.

*NOTE*: The supplied assemblies/types must also contain all the NServiceBus assemblies or types;

#### Endpoint Naming and Versioning

##### Naming

By default, the endpoint name is deduced by the namespace of the assembly that contains the configuration entry point. You can customize the endpoint name via the Configuration API using the `EndpointName()` method:            

* `EndpointName( string endpointName )`: defines the endpoint name using the supplied string; 

To dive into the endpoint naming definition, read [How To Specify Your Input Queue Name?](how-to-specify-your-input-queue-name).

##### Versioning

	//TODO
	//cfg.EndpointVersion();

#### Dependency Injection

NServiceBus relies heavily on Dependency Injection to work properly. By default the built-in Inversion of Control container will be used.

	//TODO
            //cfg.RegisterComponents();

You can also instruct NServiceBus to use your container to benefit from the dependency resolution event of your custom types. For details on how to change the default container implementation, refer to the [Containers](containers) article.

#### Transport

The configuration of the NServiceBus transport is made via the `UseTransport()` method of the configuration API.

* `UseTransport<TTransport>()`: the generic overload of the UseTransport method can be invoked using a transport class as generic parameter.
* `UseTransport( Type transportType )`: the non-generic overload of the `UseTransport()` method accepts a `Type` instance that is the type of transport class.

In both cases the call to `UseTransport()` will return a `TransportExtensions` instance that allows the configuration of the transport connection string, via the `ConnectionString()` method, orthe transport connection string name via the `ConnectionStringName()` method.

The list of the built-in supported transport is available in the [NServiceBus Connection String Samples](connection-strings-samples) article.

#### Serialization

Serialization can be controlled via the `UseSerialization` method, using one of the following supported serializers:

* `BinarySerializer`: binary serializer;
* `BsonSerializer`: BSON serializer;
* `JsonSerializer`: JSON serializer;
* `XmlSerializer`: XML serializer;

Some serializers have specific `SerializationExtensions` that allows to customize the serializer behavior:

* `JsonSerializer`:
	* `Encoding`: defines the necoding of the serialized stream;
* `XmlSerializer`:
	* `DontWrapRawXml`: Tells the serializer to not wrap properties which have either XDocument or XElement with a "PropertyName" element;
	* `Namespace`: Configures the serializer to use a custom namespace. `http://tempuri.net` is the default. If the provided namespace ends with trailing forward slashes, those will be removed on the fly;
	* `SanitizeInput`: Tells the serializer to sanitize the input data from illegal characters;
	            
#### Transactions

In order to configure the transactions settings of the endpoint it is possible to call the `Transactions()` method:

* `DefaultTimeout`: Sets the default timeout period for the transaction;
* `IsolationLevel`: Sets the isolation level of the transaction;
* `Disable` / `Enable`: Configures the current Transport to use or not use any transactions;
* `DisableDistributedTransactions` / `EnableDistributedTransactions`: Configures the crrent Transport to enlist or not in Distributed Transactions;
* `DoNotWrapHandlersExecutionInATransactionScope` / `WrapHandlersExecutionInATransactionScope`: Configures the endpoint so that `IHandleMessages<T>` are not wrapped in a `System.Transactions.TransactionScope`;

#### Outbox

	//TODO
	//cfg.EnableOutbox();
	            
#### Message Handling Pipeline

NServiceBus V5 introduces a new message handling pipeline, at configuration time it is possibile to interact with the pipeline configuration exposed by the `Pipeline` property.

An introduction to the Pipeline is available in the [Message Handling Pipeline](nservicebus-pipeline-intro) article.

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

#### Message Handlers Order

If a single endpoint contains multiple handlers that are registered to handle the same message type, or a base classe of the incoming message type, whether required it is possible to specify the order in which handlers needs to be invoked each time a specific message is received:

* `LoadMessageHandlers`: allow to specify the order in which handlers of a given message should be invoked;

#### Subscriptions

Using the `AutoSubscribe` methos it is possible to control some options of the message subscriptions engine:

* `DoNotRequireExplicitRouting`: Allows another endpoint to subscribe to messages addressed to the current endpoint;
* `DoNotAutoSubscribeSagas`: Turns off auto subscriptions for sagas. Sagas where not auto subscribed by default before V4;
* `AutoSubscribePlainMessages`: Turns on auto-subscriptions for messages not marked as commands. This was the default before V4;

#### Logging

	//TODO
NServiceBus V5 has its own internal logging implementation.

#### Fault Management

	//TODO
?????

            //cfg.DefineCriticalErrorAction();
	     //cfg.DiscardFailedMessagesInsteadOfSendingToErrorQueue();            //cfg.SecondLevelRetries();

#### Performance Counters

	//TODO
???? Enabled by default, no way to control it?

To enable Performance Counters for a specific endpoint, call the `EnablePerformanceCounters()` method. For more information on  NServiceBus performance counters, read the [Performance Counters](monitoring-nservicebus-endpoints#nservicebus-performance-counters) article.

#### Service Level Agreement

NServiceBus has the concept of [SLA](/servicepulse/monitoring-nservicebus-endpoints#service-level-agreement-sla-). The endpoint SLA can be defined using the `EnableSLAPerformanceCounter( TimeSpan sla )` method.

#### Persistence

Some NServiceBus features rely on persistence storage to work properly. Until V4 the default persistence storage was RavenDB, now RavenDB is not part of the core anymore and has been externalized as a separate [package](http://www.nuget.org/packages/NServiceBus.RavenDB/).

To define the persistence engine to use the `UsePersistence()` method must be called on the `BusConfiguration` instance. The `UsePersistence()` method returns a `PersitenceExtensions<TPersistence>` instance that allows the caller to define, via the `For()` method, for which features the given persistence storage should be used:

* `Storage.Timeouts`: Storage for timeouts;
* `Storage.Subscriptions`: Storage for subscriptions;
* `Storage.Sagas`: Storage for sagas;
* `Storage.GatewayDeduplication`: Storage for gateway deduplication;
* `Storage.Outbox`: Storage for the outbox;

If the `For()` method is not called NServiceBus assumes that the given persistence should be used for all the features that requires persistence.

##### RavenDB Persistence

To configure the persitence engine to use RavenDB as the persitence storage call the `UsePersistence<RavenDBPersitence()`, or the `UsePersistence( typeof( RavenDBPersitence ) )`, method. The `NServiceBus.RavenDB` persistence package adds some behaviors to the default `PersitenceExtensions<TPersistence>` instance:

* `SetDefaultDocumentStore`: sets the default RavenDB document store to use as default for storage;
* `UseDocumentStoreForGatewayDeduplication`: sets the default RavenDB document store for gateway deduplication;
* `UseDocumentStoreForSagas`: sets the default RavenDB document store for saga storage;
* `UseDocumentStoreForSubscriptions`: sets the default RavenDB document store for subscriptions storage;
* `UseDocumentStoreForTimeouts`: sets the default RavenDB document store for timeouts storage;
* `DoNotSetupDatabasePermissions`: instructs NServiceBus to not try to setup database permissions on the current storage at startup;
* `AllowStaleSagaReads`: allows the saga storage to retrieve sagas even if the saga query returns stale data;
* `UseSharedSession`: setups a shared RavenDB session that can be used to retrieve data from the RavenDB storage;

For a detailed explanation on how to connect to RavenDB, read the [Connecting to RavenDB from NServiceBus](using-ravendb-in-nservicebus-connecting) article.
                
##### NHibernate

To configure the persitence engine to use NHibernate as the persitence ORM call the `UsePersistence<NHibernatePersistence()`, or the `UsePersistence( typeof( NHibernatePersistence ) )`, method. The `NServiceBus.NHibernate` persistence package adds some behaviors to the default `PersitenceExtensions<TPersistence>` instance:

* `DisableGatewayDeduplicationSchemaUpdate`: Disables the Gateway Deduplication schema updates;
* `DisableSubscriptionStorageSchemaUpdate`: Disables the Subscription Storage schema updates;
* `DisableTimeoutStorageSchemaUpdate`: Disables the Timeout Storage schema updates;
* `SagaTableNamingConvention`: Allow to define the conventions used for Sagas table namings;
* `UseGatewayDeduplicationConfiguration`: Defines the configuration to use for Gateway Deduplication;
* `UseSubscriptionStorageConfiguration`: Defines the configuration to use for the Subscription Storage;
* `UseTimeoutStorageConfiguration`: Defines the configuration to use for the Timeout Storage;
* `EnableCachingForSubscriptionStorage`: Enables the usage of caching for Subscriptions;

##### In Memory Persistence

Some scenarios my benefit from an in-memory persistence configuration, such as the development environment or a lightweight client not interested in durability across restarts. In order to configure in-memory persistence use the `InMemoryPersistence` persistence class.

Details of all the persistence options are in the [Persistence in NServiceBus](persistence-in-nservicebus) article.

#### License

The methods of assigning the license to an endpoint are all detailed in the [How to install your license file](license-management) article. You can also specify a license at configuration time:

* `LicensePath( string partToLicenseFile )`: configures the endpoint to use the license file found at the supplied path;
* `License( string licenseText )`: configures the endpoint to use the supplied license, where the license text is the content of a license file.

#### Queue Management

At configuration time it is possible to define queue behavior:

* `PurgeOnStartup( Boolean purge )`: determines if endpoint queues should be purged at startup. Purging queue at startup means that messages in the queue will be deleted each time the endpoint starts;
* `DoNotCreateQueues()`: configures the endpoint to not try to create queues at startup if they are not already created.

#### Creating and Starting the Bus

When using the `NServiceBus.Host` the bus creation and startup is done by the hosting engine, when self-hosting the bus we are responsible to create and start the bus.

#### Creation

Given a `BusConfiguration` instance to create the bus it is enough to call the static `Create` method of the `Bus` class, or to create a `SendOnly` bus call the `CreateSendOnly` method.

#### Startup

If the created bus is not a send-only bus it must be started via the `Start()` method of the `IStartableBus` instance.

	//TODO: startupAction ????

#### Installers

NServicesBus allows the definition of [installers](nservicebus-installers) via the `INeedToInstallSomething` interface, in NServiceBus V5 installers can be enabled calling the `EnableInstallers` method. 

### Resources

[Customizing NServiceBus Configuration](customizing-nservicebus-configuration)

            //cfg.DisableDurableMessages();            //cfg.EnableDurableMessages();                        //cfg.EnableCriticalTimePerformanceCounter();
                                    //cfg.OverrideLocalAddress();            //cfg.OverridePublicReturnAddress();            //cfg.ScaleOut();          