---
title: Header Manipulation
summary: Illustrates all the extension points for mutating messages.
tags:
- Header
- Mutator
- Pipeline
related:
- samples/messagemutators
- samples/pipeline
- nservicebus/messaging/message-headers
- nservicebus/pipeline
---

Headers can be read and manipulated at many extension points. This Sample shows a minimal usage of that manipulation at each of those points.


## Reading incoming headers inside the outgoing context

For all the below samples that mutate the outgoing pipeline they also (optionally) read from the incoming context. The reason it is "optionally" is that an incoming context will only exist when the current message being sent was triggered from inside a Saga or a handler. For all other scenarios it will be null.


## Adding headers when sending a Message

When performing the standard messaging actions (Send, Publish, Reply etc) headers can be appended to the message being dispatched.

<!-- import sending -->

## Using Mutators

Headers can be manipulated by implementing any of the message mutation interfaces.
 

### IMessageMutator

<!-- import message-mutator -->


### IMutateIncomingMessages

<!-- import mutate-incoming-messages -->


### IMutateIncomingTransportMessages

<!-- import mutate-incoming-transport-messages -->


### IMutateOutgoingMessages

<!-- import mutate-outgoing-messages -->


### IMutateOutgoingTransportMessages

<!-- import mutate-outgoing-transport-messages -->


### IMutateTransportMessages

<!-- import mutate-transport-messages -->


## Using the Pipeline 

Headers can be manipulated at any step in the pipeline.


### Configuring the Pipeline

Configure the pipeline changes as follows.

<!-- import pipeline-config -->

Note that the injection is contextual to the other exiting steps in the pipeline. Do in this case the injection is happening after Transport message mutation has occurred.


### The outgoing Behavior

<!-- import outgoing-header-behavior-->


### The incoming Behavior

<!-- import incoming-header-behavior-->


## Globally for all outgoing messages

A list of headers can be defined that are automatically appended to all messages sent though a give instance of the Bus.

<!-- import global-all-outgoing -->


## The Handler

While the current contextual headers can be read in any of the above scenarios in this sample all headers will be written from the receiving Handler.

<!-- import handler-->