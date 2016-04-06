---
title: Upgrade NServiceBus Host Version 6 to 7
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

snippet:5to6-EndpointStartAndStop


include:5to6removePShelpers

WARNING: If the `EndpointConfig.cs` file already exists in the project, be careful to not override it when upgrading `NServiceBus.Host` package. If VisualStudio detects a conflict, it will ask whether the file should be overwritten. To keep the old configuration, choose `No`.   