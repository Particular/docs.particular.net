---
title: Learning Transport
reviewed: 2017-05-01
component: LearningTransport
related:
- nservicebus/transports
---


include: learning-transport-warning

This sample show how to use the Learning Transport and how the messages are persisted to the file system.


## Sample Structure

The sample has two endpoint, `Endpoint1` and `Endpoint2`. Both endpoitns are configured to use the Learning Transport:

snippet: UseTransport

There is also a `Shared` project that contains the message definition.


## Message Flow


### Endpoint1 sends messages

`Endpoint1` starts the message flow with a send of `TheMessage` to `Endpoint2`. The message can be optionally sent with a [delay](/nservicebus/messaging/delayed-delivery.md).

snippet: StartMessageInteraction


### Endpoint2 Handles and logs

`Endpoint2` has a Handler for `TheMessage` that logs and adds a delay to the message processing. The delay allows a window of time in which to analyze the file system.

snippet: Handler


## Running the Sample


### Start Endpoint1

Start `Endpoint1`

Note that a `.learningtransport` directory exists at the solution root. It will have the following structure:

<!-- tree /A /F |clip-->
```no-highlight
\---Endpoint1
    +---.bodies
    +---.committed
    +---.delayed
    \---.pending
```


### Send a message from Endpoint1

Press `S` to send a message or `D` to send a message with a delay.

The `.learningtransport` directory will now have an additional structure that stores the message for `Endpoint2`. The structure will differ slightly based if the message is delayed or not.

Note that `Endpoint2` does not yet have `.committed` or `.pending` as those directories are used for message processing and are only created at endpoint startup.


#### Immediate Message

```no-highlight
+---Endpoint1
    +---.bodies
    +---.committed
    +---.delayed
    \---.pending
\---Endpoint2
    |   d262f682-7508-4b7f-aef1-9cbfc6072240.metadata.txt
    \---.bodies
            d262f682-7508-4b7f-aef1-9cbfc6072240.body.txt
```


#### Delayed Message

```no-highlight
+---Endpoint1
    +---.bodies
    +---.committed
    +---.delayed
    \---.pending
\---Endpoint2
    +---.bodies
    |       d262f682-7508-4b7f-aef1-9cbfc6072240.body.txt
    \---.delayed
        \---20170502224103
                d262f682-7508-4b7f-aef1-9cbfc6072240.metadata.txt
```

Note that delayed messages are stored in a sub-directory named based on the timestamp for which the message should be processed.


### Receive a message in Endpoint2

Start `Endpoint2`. Processing of the message will begin immediately.


#### During message processing

During message processing the message will be moved into the `.pending` directory.

```no-highlight
+---Endpoint1
    +---.bodies
    +---.committed
    +---.delayed
    \---.pending
\---Endpoint2
    +---.bodies
    |       d262f682-7508-4b7f-aef1-9cbfc6072240.body.txt
    +---.committed
    +---.delayed
    \---.pending
        \---6c36d1a8-58a1-4dd5-861f-1439f3907fc9
                d262f682-7508-4b7f-aef1-9cbfc6072240.metadata.txt
```


#### After message processing

After message processing the directory structure will be cleaned up.

```no-highlight
+---Endpoint1
    +---.bodies
    +---.committed
    +---.delayed
    \---.pending
\---Endpoint2
    +---.bodies
    +---.committed
    +---.delayed
    \---.pending
```
