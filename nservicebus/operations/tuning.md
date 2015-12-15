---
title: Tuning endpoint message processing
summary: 'How to optimize message processing by limiting concurrency and/or throughput.'
tags: []
redirects:
- nservicebus/how-to-reduce-throughput-of-an-endpoint
- nservicebus/operations/reducing-throughput
- nservicebus/operations/throughput
related:
- nservicebus/licensing/licensing-limitations
---
NServiceBus will by default allow the transport to optimize for maximum performance when it comes to message processing. While this is usually the preferred mode of operation there are situations where tuning needs to be applied.


## Tuning concurrency

You can define a maximum concurrency setting that will make sure that no more messages than the specified value is ever being processed at the same time. Set this value to `1` to process messages sequentially. If not specified the transport will choose an optimal value.

Examples where concurrency tuning is relevant are

 * Non thread safe code that needs to run sequentially
 * Databases that might deadlock when getting to many concurrent requests

NOTE: NServiceBus Version 5 and below will by default limit concurrency to `1` if not configured by the user


## Tuning throughput

You can define a maximum value for the number of messages per second that the endpoint will process at any given time. This will help you to avoid your endpoint overloading sensitive resources that it's using like web-services, databases, other endpoints etc. A concrete example here could be an integration endpoint calling a web api, like api.github.com, that have restrictions on the number or requests per unit of time allowed.

NServiceBus will not enforce any throughput restrictions by default.


## Configuration

The default limits of an endpoint can be changed in both code and via app.config.


### Via Code 

By [overriding app.config settings](/nservicebus/hosting/custom-configuration-providers.md).

<!-- import TuningFromCode--->


### Via app.config

By using raw xml.

<!-- import TuningFromAppConfig--->


## Run time settings

Version 5 and lower allowed both concurrency and throughput throttling to be changed and read at run time using the code below.


## Optimizing at run time

<!-- import ChangeTuning--->


## Reading current values at run time

<!-- import ReadTuning--->
