---
title: Viewing MSMQ content
summary: See queues and message contents
tags: []
redirects:
- nservicebus/how-can-i-see-the-queues-and-messages-on-a-machine
- nservicebus/viewing-message-content-in-msmq
---


### Visual Studio

You can see all the queues on the local machine using Server Explorer in Visual Studio:

![Server Explorer](server-explorer.png "Server Explorer")

If there is a message in one of the queues, select it and view the properties of the message in the property panel in Visual Studio
(usually on the bottom right):

![Visual Studio properties](visual-studio-properties.png "Visual Studio properties")

The most interesting property is the BodyStream as it allows you to see the contents of the message:

![Message contents](body-stream.png "Message contents")


### Windows native tools

The MSMQ MMC snap-in can be used to manage queues.

Use one of the following based on your OS

    Start > Run > compmgmt.msc (Computer Management) > Features > Message Queuing

or

    Start > Run > compmgmt.msc (Computer Management) > Services and Applications > Message Queuing.


### Queue Explorer

Queue Explorer is a commercial 3rd party product for managing MSMQ.

http://www.cogin.com/mq/

> With QueueExplorer you can do much more than with built-in management console - copy, move or delete messages, save and load, stress test, view and edit full message bodies (with special support for .Net serialized objects), and much more. And it works with remote queues too!

![](queue-explorer.png 'width=400')


### Service Bus MQ Manager

A free application to view and manage MSMQ messages.

https://github.com/danielHalan/ServiceBusMQManager

![](service-bus-mq-manager.png)
