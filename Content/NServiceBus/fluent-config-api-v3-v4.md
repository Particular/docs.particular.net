---
title: Fluent Configuration API in V3 and V4
summary: NServiceBus Fluent Configuration API in V3 and V4
tags:
- NServiceBus
- Fluent Configuration
---

**NOTE**: This article refers to NServiceBus V3 and V4

###Fluent Configuration API

The NServiceBus configuration entry point is the `Configure` class and its static `With()` method, each time we need to access an instance of the current configuration we can do that via the static `Instance` property of the `Configuration` class. 

####Configuration entry point

The `With()` method has several overloads each one resulting in the creation of a new Configuration instance.

* `With()`: Initializes a new configuration scanning all the assemblies found in the `bin` folder of the current application;
* `With(string probeDirectory)`: Initializes a new configuration scanning all the assemblies found in the given `probeDirectory` folder;
* `With(params Assembly[] assemblies)`: Initializes a new configuration scanning all the supplied assemblies; *NOTE*: the supplied asssemblies must contain also the NServiceBus binaries;
* `With(IEnumerable<Type> typesToScan)`: Initializes a new configuration scanning all the supplied types; *NOTE*: the supplied types must contain also all the NServiceBus types;

**NOTE**:

* Subsequent calls to the `With` method are idempotent and only one configuration is created;
* The `With` method, and in general the whole configuration API, is not thread safe; 

####Endpoint naming

By default the endpoint name is deducted by the namespace of the assembly that contains the configuration entry point, we can customize the endpoint name via the Fluent Configuration API using the `DefineEndpointName` method:            

* `DefineEndpointName( string endpointName )`: defines the endpoint name using the supplied string; 
* `DefineEndpointName( Func<String> endpointNameFunc )`: defines the endpoint name calling the supplied delegate at runtime;

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

####Encryption

To configure the Encryption feature it is mandatory to define the encryption algorithm, the one supported out of the box by NServiceBus is Rijndael and can be configured calling the `RijndaelEncryptionService()` method.

####Performance Counters

To enable Performance Counters for a specific endpoint call the `EnablePerformanceCounters()` method. For more information on the NServiceBus performance counters refers to the [Performance Counters](monitoring-nservicebus-endpoints#nservicebus-performance-counters) article.

####Persistance

#####RavenDB Persistance

                .RavenPersistence()
                .RavenPersistence( "connection string" )
                .RavenPersistence( () => "connection string" )
                .RavenPersistence( () => "connection string", "db name" )
                .RavenPersistenceWithStore( ( IDocumentStore )null )
                .RavenSagaPersister()
                .RavenSubscriptionStorage()

#####In Memory Persistance

                .InMemoryFaultManagement()
                .InMemorySagaPersister()
                .InMemorySubscriptionStorage()

in memory management useful for dev environments and for clients do are not interested in durability across restarts

                .MsmqSubscriptionStorage()


                .License( "text" )
                .LicensePath( "" )

handling licenses (more on [license](license-management))                

                .Log4Net()
                .NLog()

More on logging (can't find anything except an article on [log4net](logging-in-nservicebus))

                .MessageForwardingInCaseOfFault()

done also via [.config file](msmqtransportconfig)                

                .PurgeOnStartup( false )

                



                

                

                

                .SetEndpointSLA( TimeSpan.FromSeconds( 2 ) )
                .Synchronization()


                

                .UnicastBus()
                .CreateBus()

                


            .DisableGateway()
            .DisableTimeoutManager()

in v4 done via feature                

            .DoNotCreateQueues()

disables the automatic queue creation done at startup                



                .Start( () =>
                {
                    Configure.Instance.ForInstallationOn<Windows>();
                } )
                .Start()

                

                .SendOnly();
                
creates and "start" the send only bus, no need to start it.
                



*Mauro's comments*

How to document that? Shall we document every single method?

I've already discarded all the "obsolete" methods but I suppose that something should be done via the feature configuration and not via the fluent config.


    .DefineLocalAddressNameFunc( () => "" )

????