---
title: Upgrade Version 6 to 7
summary: Instructions on how to upgrade Azure Cloud Service Host from Version 6 to 7.
tags:
 - upgrade
 - migration
related:
- nservicebus/upgrades/5to6

---

## IWantToRunWhenEndpointStartsAndStops 

A new interface called `IWantToRunWhenEndpointStartsAndStops` has been added. This interface replaces the `IWantToRunWhenBusStartsAndStops` in the NServiceBus core. 

snippet:5to6-EndpointStartAndStop

For more details, please refer to the [NServiceBus Azure Cloud Service Host documentation](/nservicebus/azure/hosting-in-azure-cloud-services.md) on the new interface. 