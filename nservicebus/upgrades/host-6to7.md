---
title: NServiceBus Host Upgrade Version 6 to 7
summary: Instructions on how to upgrade NServiceBus Host Version 6 to 7.
reviewed: 2016-04-06
tags:
 - upgrade
 - migration
related:
- nservicebus/hosting/nservicebus-host
- nservicebus/upgrades/5to6
---


## IConfigureThisEndpoint changes

`IConfigureThisEndpoint.Customize` is passed an instance of `EndpointConfiguration` instead of `BusConfiguration`.

snippet: 6to7customize_nsb_host


## IWantToRunWhenEndpointStartsAndStops

An interface called [`IWantToRunWhenEndpointStartsAndStops`](/nservicebus/hosting/nservicebus-host/) has been added. This interface replaces the [`IWantToRunWhenBusStartsAndStops`](/nservicebus/lifecycle/endpointstartandstop.md) in the NServiceBus core.

DANGER: The `Start` and `Stop` methods will block start up and shut down of the endpoint. For more details, read: [Host Start Up Behavior](/nservicebus/hosting/nservicebus-host/#startup-behavior).


### Interface in Version 5 of NServiceBus

snippet:5to6-EndpointStartAndStopCore


### Interface in Version 7 of NServiceBus.Host

snippet:5to6-EndpointStartAndStopHost


The `IMessageSession` parameter provides all the necessary methods to send messages as part of the endpoint start up.

include:5to6removePShelpers

WARNING: If an `EndpointConfig.cs` file already exists in the project, be careful to not overwrite it when upgrading the `NServiceBus.Host` package. If Visual Studio detects a conflict, it will ask whether the file should be overwritten. To keep the old configuration, choose `No`.


## WCF Integration

The WCF integration using `WcfService` has been moved from the host to the separate NuGet package [NServiceBus.Wcf](https://www.nuget.org/packages/NServiceBus.Wcf/). That package must be used in order to use the WCF integration functionality when targeting NServiceBus versions 6 and above. The NServiceBus.Wcf NuGet package **has no dependency** on the NServiceBus.Host NuGet package and can also be used in self-hosting scenarios.

The WCF integration has been augmented with additional functionality such as the ability to reply with messages to the client, cancel requests and reroute to other endpoints. More information can be found in [Wcf](/nservicebus/wcf).