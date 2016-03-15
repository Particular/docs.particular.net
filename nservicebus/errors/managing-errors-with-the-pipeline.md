---
title: Managing errors with the pipeline
summary: Instructions on how to perform logic during FLR and SLR with the pipeline
tags:
 - upgrade
 - example
---

In versions of NServiceBus lower than v6 to add logic to FLR and SLR processing NServiceBus provided the `IManageMessageFailures` interface to implement. Now developers must instead add a new behavior class which will be invoked during the message processing pipeline. In this behavior, both the deserialization exception and the general exception can be handled as well as any custom action that needs to be performed when an exception happens. 

While Version 5 did not allow an easier way to invoke both the SLR and the custom fault handling policy, in Version 6, SLR can either be disabled or customized as necessary. For more details, read this article on how to [disable SLR](/nservicebus/errors/automatic-retries.md#second-level-retries-disabling-slr-through-code).


### Create a new behavior that has access to the message at the transport message context

snippet: 5to6-customerrorhandlingbehaviour

As illustrated in the above example, by choosing to throw in the catch block, this behavior can then let NServiceBus forward the message to the error queue. If that's not the desired behavior, remove the throw to let NServiceBus know that the message handling is complete.


### Create a new message processing step in the pipeline.

Specify where and how this behavior is invoked. 

snippet: 5to6-newmessageprocessingpipelinestep


### Register the new step in the pipeline

snippet: 5to6-registercustomerrorhandling