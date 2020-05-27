---
title: Header Manipulation
summary: All extension points for mutating messages.
reviewed: 2019-09-16
component: Core
related:
- samples/messagemutators
- samples/pipeline
- nservicebus/messaging/headers
- nservicebus/messaging/header-manipulation
- nservicebus/pipeline
---

## Introduction

Headers can be read and manipulated at many extension points. This sample shows a minimal usage of that manipulation at each of those points.


## Reading incoming headers inside the outgoing context

For all the below samples that mutate the outgoing pipeline they also (optionally) read from the incoming context. The reason it is optional is that an incoming context will only exist when the current message being sent was triggered from inside a saga or a handler. For all other scenarios it will be null.


## Adding headers when sending a Message

When performing the standard messaging actions (Send, Publish, Reply etc) headers can be appended to the message being dispatched.

snippet: sending

## Using Mutators

Headers can be manipulated by implementing any of the message mutation interfaces.


partial: mutator


### IMutateIncomingMessages

snippet: mutate-incoming-messages


### IMutateIncomingTransportMessages

snippet: mutate-incoming-transport-messages


### IMutateOutgoingMessages

snippet: mutate-outgoing-messages


### IMutateOutgoingTransportMessages

snippet: mutate-outgoing-transport-messages




## Using the Pipeline

Headers can be manipulated at any step in the pipeline.


### Configuring the Pipeline

Configure the pipeline changes as follows.

snippet: pipeline-config

Note that the injection is contextual to the other existing steps in the pipeline. In this case the injection is happening after transport message mutation has occurred.


### The outgoing Behavior

snippet: outgoing-header-behavior


### The incoming Behavior

snippet: incoming-header-behavior


## Globally for all outgoing messages

partial: globalheaders

snippet: global-all-outgoing


## The Handler

While the current contextual headers can be read in any of the above scenarios, in this sample all headers will be written from the receiving handler.

snippet: handler