---
title: Fluent Configuration API in V3 and V4
summary: NServiceBus Fluent Configuration API in V3 and V4
tags:
- NServiceBus
- Fluent Configuration
---

**NOTE**: This article refers to NServiceBus V3 and V4

###Fluent Configuration API

The full available API is the following:

Starting the configuration

            Configure.With()
            Configure.With(string probeDirectory)
            Configure.With(params Assembly[] assemblies)
            Configure.With(IEnumerable<Type> typesToScan)
            
defining the endpoint name ([details](how-to-specify-your-input-queue-name))

                .DefineEndpointName( "" )
                .DefineEndpointName( () => "" )

important: always call DefineEndpointName as the first child of With()

                .DefineLocalAddressNameFunc( () => "" )
                
????

                .DefaultBuilder()

other IoC [containers](containers)....

                .DefiningCommandsAs( t => true )
                .DefiningDataBusPropertiesAs( pi => true )
                .DefiningEncryptedPropertiesAs( pi => true )
                .DefiningEventsAs( t => true )
                .DefiningExpressMessagesAs( t => true )
                .DefiningMessagesAs( t => true )
                .DefiningTimeToBeReceivedAs( t => TimeSpan.FromSeconds( 2 ) )

More details on the [unobtrusive mode](unobtrusive-mode-messages)                

                .DisableGateway()
                .DisableTimeoutManager()

in v4 done via feature                

                .DoNotCreateQueues()

disables the automatic queue creation done at startup                

                .EnablePerformanceCounters()

more on [Performance Counters](monitoring-nservicebus-endpoints#nservicebus-performance-counters)

                .FileShareDataBus( "" )

Mode on the [DataBus / Attachments](attachments-databus-sample)                

                .InMemoryFaultManagement()
                .InMemorySagaPersister()
                .InMemorySubscriptionStorage()

in memory management useful for dev environments and for clients do are not interested in durability across restarts

                .License( "text" )
                .LicensePath( "" )

handling licenses (more on [license](license-management))                

                .Log4Net()
                .NLog()

More on logging (can't find anything except an article on [log4net](logging-in-nservicebus))

                .MessageForwardingInCaseOfFault()

done also via [.config file](msmqtransportconfig)

                .MsmqSubscriptionStorage()

                

                .PurgeOnStartup( false )

                

                .RavenPersistence()
                .RavenPersistence( "connection string" )
                .RavenPersistence( () => "connection string" )
                .RavenPersistence( () => "connection string", "db name" )
                .RavenPersistenceWithStore( ( IDocumentStore )null )
                .RavenSagaPersister()
                .RavenSubscriptionStorage()

                

                .RijndaelEncryptionService()

                

                .SetEndpointSLA( TimeSpan.FromSeconds( 2 ) )
                .Synchronization()
                .UseTransport( new Type(), "connection string (optional)" )
                .UseTransport<Msmq>( "connection string (optional)" )

                

                .UnicastBus()
                .CreateBus()

                

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