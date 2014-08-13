---
title: NServiceBus Documentation
summary: NServiceBus Documentation Table of Contents
---

## Table of Contents

- [Getting Started](#getting-started)
- [Persistence in NServiceBus](#persistence-in-nservicebus)
- [Scaling Out](#scaling-out)
- [Day to Day](#day-to-day)
- [Hosting](#hosting)
- [Management and Monitoring](#management-and-monitoring)
- [Publish Subscribe](#publish-subscribe)
- [Long Running Processes](#long-running-processes)
- [Customization](#customization)
- [Versioning](#versioning)
- [FAQ](#faq)
- [Samples](/platform/samples/)


## Getting Started
- [Overview](Overview)
- [NServiceBus Step by Step Guide](NServiceBus-Step-by-Step-Guide)
- [NServiceBus Step by Step Guide Fault Tolerance Code First](NServiceBus-Step-by-Step-Guide-fault-tolerance-code-first)
- [NServiceBus Step by Step Publish Subscribe Communication Code First](nservicebus-step-by-step-publish-subscribe-communication-code-first)
- [Getting Started Fault Tolerance](NServiceBus-Step-by-Step-Guide-fault-tolerance-code-first)
- [Architectural Principles](architectural-principles)
- [Transactions Message Processing](transactions-message-processing)
- [Building NServiceBus from Source Files](building-nservicebus-from-source-files)
- [NServiceBus and WCF](nservicebus-and-wcf)
- [NServiceBus and WebSphere Sonic](nservicebus-and-websphere-sonic)
- [NServiceBus and BizTalk](nservicebus-and-biztalk)

## Persistence in NServiceBus
- [Persistence in NServiceBus](persistence-in-nservicebus)
- [Using RavenDB in NServiceBus Installing](using-ravendb-in-nservicebus-installing)
- [Using RavenDB in NServiceBus Connecting](using-ravendb-in-nservicebus-connecting)
- [Relational Persistence Using NHibernate](relational-persistence-using-nhibernate)
- [Unit of Work in NServiceBus](unit-of-work-in-nservicebus)
- [Unit of Work Implementation for RavenDB](unit-of-work-implementation-for-ravendb)
- [Relational Persistence Using NHibernate NServiceBus 4.X](relational-persistence-using-nhibernate---nservicebus-4.x)

## Scaling Out
- [Performance](performance)
- [The Gateway and Multi Site Distribution](the-gateway-and-multi-site-distribution)
- [Load Balancing with the Distributor](load-balancing-with-the-distributor)
- [Introduction to the Gateway](introduction-to-the-gateway)
- [Deploying NServiceBus in a Windows Fail-over Cluster](deploying-nservicebus-in-a-windows-failover-cluster)

## Day to Day
- [Containers](containers)
- [NServiceBus Support for Child Containers](nservicebus-support-for-child-containers)
- [Logging in NServiceBus](logging-in-nservicebus)
- [Messages as Interfaces](messages-as-interfaces)
- [Introducing IEvent and ICommand](introducing-ievent-and-icommand)
- [Staying Updated with Nuget](staying-updated-with-nuget)
- [Unobtrusive Mode Messages](unobtrusive-mode-messages)
- [Unit Testing](unit-testing)
- [One Way Send Only Endpoints](one-way-send-only-endpoints)
- [Scheduling with NServiceBus](scheduling-with-nservicebus)
- [Second Level Retries](second-level-retries)
- [NServiceBus Installers](nservicebus-installers)
- [Managing NServiceBus Using PowerShell](managing-nservicebus-using-powershell)
- [NServiceBus Connection String Samples](connection-strings-samples)

## Hosting
- [The NServiceBus Host](the-nservicebus-host)
- [Hosting NServiceBus in Your Own Process](hosting-nservicebus-in-your-own-process)
- [Profiles for NServiceBus Host](profiles-for-nservicebus-host)
- [More on Profiles](more-on-profiles)
- [NServiceBus 32 Bit X86 Host Process](nservicebus-32-bit-x86-host-process)
- [Hosting NServiceBus in Windows Azure](hosting-nservicebus-in-windows-azure)

## Management and Monitoring
- [Monitoring NServiceBus Endpoints](monitoring-nservicebus-endpoints)
- [MSMQ Information](msmq-information)
- [Auditing with NServiceBus](auditing-with-nservicebus)

## Publish Subscribe
- [How Pub Sub Works](how-pub-sub-works)
- [Publish Subscribe Configuration](publish-subscribe-configuration)

## Long Running Processes
- [Sagas in NServiceBus](sagas-in-nservicebus)
- [NServiceBus Sagas and Concurrency](nservicebus-sagas-and-concurrency)

## Customization
- [Customizing NServiceBus Configuration](customizing-nservicebus-configuration)
- [Pipeline Management Using Message Mutators](pipeline-management-using-message-mutators)

## Versioning
- [Migrating to NServiceBus 3.0 Timeouts](migrating-to-nservicebus-3.0-timeouts)

## FAQ
- [MsmqTransportConfig](msmqtransportconfig)
- [How Do I Define a Message](how-do-i-define-a-message)
- [Using the in Memory Bus](using-the-in-memory-bus)
- [How Do I Specify Store Forward for a Message](how-do-i-specify-store-forward-for-a-message)
- [How Do I Discard Old Messages](how-do-i-discard-old-messages)
- [How Do I Instantiate a Message](how-do-i-instantiate-a-message)
- [How Do I Send a Message](how-do-i-send-a-message)
- [How Do I Specify to Which Destination a Message Will Be Sent](how-do-i-specify-to-which-destination-a-message-will-be-sent)
- [How Can I See the Queues and Messages on a Machine](how-can-i-see-the-queues-and-messages-on-a-machine)
- [How Do I Handle a Message](how-do-i-handle-a-message)
- [How Do I Specify the Order in Which Handlers Are Invoked](how-do-i-specify-the-order-in-which-handlers-are-invoked)
- [How Do I Get a Reference to IBus in My Message Handler](how-do-i-get-a-reference-to-ibus-in-my-message-handler)
- [How Do I Get Technical Information about a Message](how-do-i-get-technical-information-about-a-message)
- [How Do I Reply to a Message](how-do-i-get-technical-information-about-a-message)
- [How Do I Handle Responses on the Client Side](how-do-i-handle-responses-on-the-client-side)
- [How Do I Handle Exceptions](how-do-i-handle-exceptions)
- [How Do I Expose an NServiceBus Endpoint as a Web WCF Service](how-do-i-expose-an-nservicebus-endpoint-as-a-web-wcf-service)
- [Type Was Not Registered in the Serializer](type-was-not-registered-in-the-serializer)
- [MessageQueueException Insufficient Resources to Perform Operation](messagequeueexception-insufficient-resources-to-perform-operation)
- [How to Specify Your Input Queue Name](how-to-specify-your-input-queue-name)
- [In a Distributor Scenario What Happens to the Message If a Worker Goes Down](in-a-distributor-scenario-what-happens-to-the-message-if-a-worker-goes-down)
- [No Endpoint Configuration Found in Scanned Assemblies Exception](no-endpoint-configuration-found-in-scanned-assemblies-exception)
- [DtcPing Warning the Cid Values for Both Test Machines Are the Same](dtcping-warning-the-cid-values-for-both-test-machines-are-the-same)
- [Why You Can't Use NLB with MSMQ](why-you-can-t-use-nlb-with-msmq)
- [Configuring AWS for NServiceBus](configuring-aws-for-nservicebus)
- [Licensing and throughput limitations](licensing-limitations)
- [Licensing and Distribution](licensing-and-distribution)
- [How to Debug RavenDb Through Fiddler Using NServiceBus](how-to-debug-ravendb-through-fiddler-using-nservicebus)
- [How Do I Centralize All Unobtrusive Declarations](how-do-i-centralize-all-unobtrusive-declarations)
- [DefiningMessagesas and DefiningEventsas When Starting Endpoint](definingmessagesas-and-definingeventsas-when-starting-endpoint)
- [How to Reduce Throughput of an Endpoint](how-to-reduce-throughput-of-an-endpoint)
- [InvalidOperationException in Unobtrusive Mode](invalidoperationexception-in-unobtrusive-mode)
- [License Management](license-management)
- [Preparing Your Machine to Run NServiceBus](preparing-your-machine-to-run-nservicebus)
- [Running NServiceBus on Windows](running-nservicebus-on-windows)
- [Licensing ServiceMatrix V2.0](licensing-servicematrix-v2.0) 
