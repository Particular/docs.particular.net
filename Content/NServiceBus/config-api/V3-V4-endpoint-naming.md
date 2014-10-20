---
title: Configuration API endpoint naming in V3 and V4
summary: Configuration API endpoint naming in V3 and V4
tags:
- NServiceBus
- Fluent Configuration
- V3
- V4
---

By default, the endpoint name is deduced by the namespace of the assembly that contains the configuration entry point. You can customize the endpoint name via the Fluent Configuration API using the `DefineEndpointName` method:            

* `DefineEndpointName( string endpointName )`: defines the endpoint name using the supplied string; 

NOTE: If you need to customize the endpoint name via code using the `DefineEndpointName` method, it is important to call it as the first one right after the `With()` configuration entry point.

To dive into the endpoint naming definition, read [How To Specify Your Input Queue Name?](how-to-specify-your-input-queue-name).