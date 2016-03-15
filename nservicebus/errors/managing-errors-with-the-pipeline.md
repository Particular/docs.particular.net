---
title: Managing errors with the pipeline
summary: Instructions on how to perform logic during FLR and SLR with the pipeline
tags:
 - upgrade
 - example
---

In Versions 5 and below to add logic to FLR and SLR processing required implementing the `IManageMessageFailures` interface. Instead add a new behavior which is invoked during the message processing pipeline. In this behavior, both the deserialization exception and the general exception can be handled as well as any custom action that needs to be performed when an exception occurs. 

In Version 6 SLR can either be disabled or customized as necessary. See also [disabling SLR](/nservicebus/errors/automatic-retries.md#second-level-retries-disabling-slr-through-code).

### Create a new behavior that has access to the message at the transport message context

snippet: 5to6-customerrorhandlingbehaviour

As illustrated in the above example, by choosing to throw in the catch block, this behavior can then let NServiceBus forward the message to the error queue. If that's not the desired behavior, remove the throw to let NServiceBus know that the message handling is complete.


### Create a new message processing step in the pipeline.

Specify where and how this behavior is invoked. 

snippet: 5to6-newmessageprocessingpipelinestep


### Register the new step in the pipeline

snippet: 5to6-registercustomerrorhandling