---
title: Tuning endpoint message processing
summary: 'How to optimize message processing by limiting concurrency and/or throughput.'
redirects:
- nservicebus/how-to-reduce-throughput-of-an-endpoint
- nservicebus/operations/reducing-throughput
- nservicebus/operations/throughput
related:
- nservicebus/licensing/licensing-limitations
---

NServiceBus will by default allow the transport to optimize for maximum performance when it comes to message processing. While this is usually the preferred mode of operation there are situations where tuning needs to be applied.


## Tuning concurrency

NOTE: The default concurrency limit for NServiceBus Versions 5 and below is `1`, for Versions 6 and above `100`.

Define a maximum concurrency setting that will make sure that no more messages than the specified value is ever being processed at the same time. Set this value to `1` to process messages sequentially. If not specified the transport will choose an optimal value.

Examples where concurrency tuning is relevant are

 * Non thread safe code that needs to run sequentially
 * Databases that might deadlock when getting to many concurrent requests


## Tuning throughput

WARNING: Throughput throttling options have been deprecated in NServiceBus Version 6. To enable throttling on Version 6 and higher, a custom behavior should be used. The [throttling sample](/samples/throttling/) demonstrates how such a behavior can be implemented.

Define a maximum value for the number of messages per second that the endpoint will process at any given time. This will help avoid the endpoint overloading sensitive resources that it's using like web-services, databases, other endpoints etc. A concrete example here could be an integration endpoint calling a web API, like api.github.com, that have restrictions on the number or requests per unit of time allowed.

NServiceBus will not enforce any throughput restrictions by default.


## Configuration

The default limits of an endpoint can be changed in both code and via app.config.


### Via Code

snippet: TuningFromCode


### Via a IConfigurationProvider

snippet: TuningFromConfigurationProvider


### Via app.config

By using raw xml.

snippet: TuningFromAppConfig


## Run time settings

Version 5 and below allowed both concurrency and throughput throttling to be changed and read at run time using the code below.


## Optimizing at run time

snippet: ChangeTuning


## Reading current values at run time

snippet: ReadTuning
