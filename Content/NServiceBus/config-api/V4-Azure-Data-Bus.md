---
title: Configuration API Azure Data Bus in V4
summary: Configuration API Azure Data Bus in V4
tags:
- NServiceBus
- BusConfiguration
- V4
---

Endpoints running on Windows Azure cannot access UNC paths or file shares. In this case the [NServiceBus Azure Transport](http://www.nuget.org/packages/nservicebus.azure) provides its own DataBus implementation that you can configure using the `AzureDataBus()` method.

	//TODO: Define DataBus properties how to?