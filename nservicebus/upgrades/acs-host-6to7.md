---
title: Upgrade Azure Cloud Service Host Version 6 to 7
summary: Instructions on how to upgrade Azure Cloud Service Host from Version 6 to 7.
tags:
 - upgrade
 - migration
related:
- nservicebus/upgrades/5to6
---


## IWantToRunWhenEndpointStartsAndStops 

An interface called [`IWantToRunWhenEndpointStartsAndStops`](/nservicebus/azure/hosting-in-azure-cloud-services.md) has been added. This interface replaces the [`IWantToRunWhenBusStartsAndStops`](/nservicebus/lifecycle/endpointstartandstop.md) in the NServiceBus core.

snippet:5to6-EndpointStartAndStop