---
layout:
title: "How to Define a Message?"
tags: 
origin: http://www.particular.net/Articles/how-do-i-define-a-message
---
    public class MyMessage : IMessage { }

OR

    public interface IMyMessage : IMessage { }

One advantage of using interfaces to define messages instead of classes is that you get "multiple inheritance"; i.e., one message can extend multiple other messages. This is useful for solving a specific class of versioning problems.

Say that your business logic represents a state machine with states X and Y. When your system gets into state X, it publishes the message EnteredStateX. When your system gets into state Y, it publishes the message EnteredStateY. (For more information on how to publish a message, see below.)

In the next version of your system, you add a new state Z, which represents the co-existence of both X and Y. So, you define the message EnteredStateZ, which inherits both EnteredStateX and EnteredStateY.

When your system publishes EnteredStateZ, clients subscribed to EnteredStateX and/or EnteredStateY are notified.

Without the ability for a message extending to multiple others, you would have to use composition, thereby preventing the infrastructure from automatically routing messages to pre-existing subscribers of the composed messages.

