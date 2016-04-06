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

A new interface called `IWantToRunWhenEndpointStartsAndStops` has been added. This interface replaces the `IWantToRunWhenBusStartsAndStops` in the NServiceBus core. 

snippet:5to6-EndpointStartAndStop

For more details, please refer to the [NServiceBus.Host documentation](/nservicebus/hosting/nservicebus-host/) on the new interface.  