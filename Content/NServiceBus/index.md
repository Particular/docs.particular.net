---
title: NServiceBus
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
- [Samples](/platform/samples.md)


## Getting Started

- [Overview](Overview.md)
- [NServiceBus Step by Step Guide](NServiceBus-Step-by-Step-Guide.md)
- [NServiceBus Step by Step Guide Fault Tolerance Code First](NServiceBus-Step-by-Step-Guide-fault-tolerance-code-first.md)
- [NServiceBus Step by Step Publish Subscribe Communication Code First](nservicebus-step-by-step-publish-subscribe-communication-code-first.md)
- [Getting Started Fault Tolerance](NServiceBus-Step-by-Step-Guide-fault-tolerance-code-first.md)
- [Architectural Principles](architectural-principles.md)
- [Transactions Message Processing](transactions-message-processing.md)
- [Building NServiceBus from Source Files](building-nservicebus-from-source-files.md)
- [NServiceBus and WCF](nservicebus-and-wcf.md)
- [NServiceBus and WebSphere Sonic](nservicebus-and-websphere-sonic.md)
- [NServiceBus and BizTalk](nservicebus-and-biztalk.md)
- [Reliable messaging without MSDTC](no-dtc.md)

## Persistence in NServiceBus

- [Persistence in NServiceBus](persistence-in-nservicebus.md)
- [Using RavenDB in NServiceBus Installing](using-ravendb-in-nservicebus-installing.md)
- [Using RavenDB in NServiceBus Connecting](using-ravendb-in-nservicebus-connecting.md)
- [Relational Persistence Using NHibernate](relational-persistence-using-nhibernate.md)
- [Unit of Work in NServiceBus](unit-of-work-in-nservicebus.md)
- [Unit of Work Implementation for RavenDB](unit-of-work-implementation-for-ravendb.md)
- [RavenDB Version Compatibility](RavenDB/VersionCompatibility.md)
- [Configuration order for persistence](persistence-order.md)

## Scaling Out

- [Performance](performance.md)
- [The Gateway and Multi Site Distribution](the-gateway-and-multi-site-distribution.md)
- [Load Balancing with the Distributor](load-balancing-with-the-distributor.md)
- [Introduction to the Gateway](introduction-to-the-gateway.md)
- [Deploying NServiceBus in a Windows Fail-over Cluster](deploying-nservicebus-in-a-windows-failover-cluster.md)

## Day to Day

- [Containers](containers.md)
- [NServiceBus Support for Child Containers](nservicebus-support-for-child-containers.md)
- [Logging in NServiceBus](logging-in-nservicebus.md)
- [Messages as Interfaces](messages-as-interfaces.md)
- [Introducing IEvent and ICommand](introducing-ievent-and-icommand.md)
- [Staying Updated with Nuget](staying-updated-with-nuget.md)
- [Unobtrusive Mode Messages](unobtrusive-mode-messages.md)
- [Unit Testing](unit-testing.md)
- [One Way Send Only Endpoints](one-way-send-only-endpoints.md)
- [Scheduling with NServiceBus](scheduling-with-nservicebus.md)
- [Second Level Retries](second-level-retries.md)
- [NServiceBus Installers](nservicebus-installers.md)
- [Managing NServiceBus Using PowerShell](managing-nservicebus-using-powershell.md)
- [NServiceBus Connection String Samples](connection-strings-samples.md)

## Hosting

- [The NServiceBus Host](the-nservicebus-host.md)
- [Hosting NServiceBus in Your Own Process](hosting-nservicebus-in-your-own-process.md)
- [Profiles for NServiceBus Host](profiles-for-nservicebus-host.md)
- [More on Profiles](more-on-profiles.md)
- [NServiceBus 32 Bit X86 Host Process](nservicebus-32-bit-x86-host-process.md)
- [Hosting NServiceBus in Windows Azure](hosting-nservicebus-in-windows-azure.md)

## Management and Monitoring

- [Monitoring NServiceBus Endpoints](monitoring-nservicebus-endpoints.md)
- [MSMQ Information](msmq-information.md)
- [Auditing with NServiceBus](auditing-with-nservicebus.md)
- [Push-based error notifications](subscribing-to-push-based-error-notifications.md)
- [Disconnect workers from a running Distributor](DisconnectWorkersFromARunningDistributor.md)

## Publish Subscribe

- [How Pub Sub Works](how-pub-sub-works.md)
- [Publish Subscribe Configuration](publish-subscribe-configuration.md)

## Long Running Processes

- [Sagas in NServiceBus](sagas-in-nservicebus.md)
- [NServiceBus Sagas and Concurrency](nservicebus-sagas-and-concurrency.md)

## Customization

- [Customizing NServiceBus Configuration](customizing-nservicebus-configuration.md)
- [Pipeline Management Using Message Mutators](pipeline-management-using-message-mutators.md)
- [Override host identifier](override-hostid.md)

## Versioning

- [Migrating to NServiceBus 3.0 Timeouts](migrating-to-nservicebus-3.0-timeouts.md)

## FAQ

- [MsmqTransportConfig](msmqtransportconfig.md)
- [How Do I Define a Message](how-do-i-define-a-message.md)
- [Using the in Memory Bus](using-the-in-memory-bus.md)
- [How Do I Specify Store Forward for a Message](how-do-i-specify-store-forward-for-a-message.md)
- [How Do I Discard Old Messages](how-do-i-discard-old-messages.md)
- [How Do I Instantiate a Message](how-do-i-instantiate-a-message.md)
- [How Do I Send a Message](how-do-i-send-a-message.md)
- [How Do I Specify to Which Destination a Message Will Be Sent](how-do-i-specify-to-which-destination-a-message-will-be-sent.md)
- [How Can I See the Queues and Messages on a Machine](how-can-i-see-the-queues-and-messages-on-a-machine.md)
- [How Do I Handle a Message](how-do-i-handle-a-message.md)
- [How Do I Specify the Order in Which Handlers Are Invoked](how-do-i-specify-the-order-in-which-handlers-are-invoked.md)
- [How Do I Get a Reference to IBus in My Message Handler](how-do-i-get-a-reference-to-ibus-in-my-message-handler.md)
- [How Do I Get Technical Information about a Message](how-do-i-get-technical-information-about-a-message.md)
- [How Do I Reply to a Message](how-do-i-get-technical-information-about-a-message.md)
- [How Do I Handle Responses on the Client Side](how-do-i-handle-responses-on-the-client-side.md)
- [How Do I Handle Exceptions](how-do-i-handle-exceptions.md)
- [How Do I Expose an NServiceBus Endpoint as a Web WCF Service](how-do-i-expose-an-nservicebus-endpoint-as-a-web-wcf-service.md)
- [Type Was Not Registered in the Serializer](type-was-not-registered-in-the-serializer.md)
- [MessageQueueException Insufficient Resources to Perform Operation](messagequeueexception-insufficient-resources-to-perform-operation.md)
- [How to Specify Your Input Queue Name](how-to-specify-your-input-queue-name.md)
- [In a Distributor Scenario What Happens to the Message If a Worker Goes Down](in-a-distributor-scenario-what-happens-to-the-message-if-a-worker-goes-down.md)
- [No Endpoint Configuration Found in Scanned Assemblies Exception](no-endpoint-configuration-found-in-scanned-assemblies-exception.md)
- [DtcPing Warning the Cid Values for Both Test Machines Are the Same](dtcping-warning-the-cid-values-for-both-test-machines-are-the-same.md)
- [Why You Can't Use NLB with MSMQ](why-you-can-t-use-nlb-with-msmq.md)
- [Configuring AWS for NServiceBus](configuring-aws-for-nservicebus.md)
- [Licensing and throughput limitations](licensing-limitations.md)
- [Licensing and Distribution](licensing-and-distribution.md)
- [How to Debug RavenDb Through Fiddler Using NServiceBus](how-to-debug-ravendb-through-fiddler-using-nservicebus.md)
- [How Do I Centralize All Unobtrusive Declarations](how-do-i-centralize-all-unobtrusive-declarations.md)
- [DefiningMessagesas and DefiningEventsas When Starting Endpoint](definingmessagesas-and-definingeventsas-when-starting-endpoint.md)
- [How to Reduce Throughput of an Endpoint](how-to-reduce-throughput-of-an-endpoint.md)
- [InvalidOperationException in Unobtrusive Mode](invalidoperationexception-in-unobtrusive-mode.md)
- [License Management](license-management.md)
- [Preparing Your Machine to Run NServiceBus](preparing-your-machine-to-run-nservicebus.md)
- [Running NServiceBus on Windows](running-nservicebus-on-windows.md)
- [How to specify time to wait before raising critical exception for timeout outages](how-do-I-specify-time-to-wait-before-raising-critical-exception-for-timeout-outages.md)
- [How to register a custom serializer](how-to-register-a-custom-serializer.md)
