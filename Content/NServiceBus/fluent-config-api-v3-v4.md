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

            Configure.With()
                .DefineEndpointName( "" )
                .DefineEndpointName( () => "" )
                .DefineLocalAddressNameFunc( () => "" )
                .DefaultBuilder()
                .DefiningCommandsAs( t => true )
                .DefiningDataBusPropertiesAs( pi => true )
                .DefiningEncryptedPropertiesAs( pi => true )
                .DefiningEventsAs( t => true )
                .DefiningExpressMessagesAs( t => true )
                .DefiningMessagesAs( t => true )
                .DefiningTimeToBeReceivedAs( t => TimeSpan.FromSeconds( 2 ) )
                .DisableGateway()
                .DisableTimeoutManager()
                .DoNotCreateQueues()
                .EnablePerformanceCounters()
                .FileShareDataBus( "" )
                .InMemoryFaultManagement()
                .InMemorySagaPersister()
                .InMemorySubscriptionStorage()
                .License( "text" )
                .LicensePath( "" )
                .Log4Net()
                .MessageForwardingInCaseOfFault()
                .MsmqSubscriptionStorage()
                .NLog()
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
                
How to document that? Shall we document every single method?

I've already discarded all the "obsolete" methods but I suppose that something should be done via he feature configuration and not via the fluent config.