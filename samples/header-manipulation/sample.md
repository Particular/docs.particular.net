---
title: Header Mutation
summary: Illustrates all the extension points for mutating messages.
tags:
- Headers
- Mutators
- Pipeline
related:
- samples/messagemutators
- samples/pipeline
- nservicebus/messaging/message-headers
- nservicebus/pipeline
---

Headers can be read and manipulated at many extension points. This Sample shows a minimal usage of that manipulation at each of those points.

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

<!--
## For all outgoing Messages

Static headers can be added to a list that will be appended to all outgoing messages.

 import global-all-outgoing -->


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


## The Handler

While the current contextual headers can be read in any of the above scenarios in this sample all headers will be written from the receiving Handler.

<!-- import handler-->