---
title: Configuration API endpoint naming in V5
summary: Configuration API endpoint naming in V5
tags:
- NServiceBus
- BusConfiguration
- V5
---

By default, the endpoint name is deduced by the namespace of the assembly that contains the configuration entry point. You can customize the endpoint name via the Configuration API using the `EndpointName()` method:            

* `EndpointName( string endpointName )`: defines the endpoint name using the supplied string; 

To dive into the endpoint naming definition, read [How To Specify Your Input Queue Name?](/nservicebus/how-to-specify-your-input-queue-name).