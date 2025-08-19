---
title: Viewing MSMQ content
summary: Provides options for visualizing messages and queues in MSMQ
component: MsmqTransport
reviewed: 2025-08-14
redirects:
 - nservicebus/how-can-i-see-the-queues-and-messages-on-a-machine
 - nservicebus/viewing-message-content-in-msmq
 - nservicebus/msmq/viewing-message-content-in-msmq
---

There are several ways to view messages and queues in MSMQ, using both free and commercial tools.

## Visual Studio

Queues on the local machine can be listed using **Server Explorer** in Visual Studio:

![Server Explorer](server-explorer.png "Server Explorer")

If a queue contains messages, select one and open the **Properties** panel (<kbd>F4</kbd>) to view its details:

![Visual Studio properties](visual-studio-properties.png "Visual Studio properties")

The **BodyStream** property shows the message content:

![Message contents](body-stream.png "Message contents")

## Windows native tools

MSMQ can also be managed using the built-in **MMC snap-in**.

Depending on the Windows version, open via:

```
Start > Run > compmgmt.msc > Features > Message Queuing
```

or

```
Start > Run > compmgmt.msc > Services and Applications > Message Queuing
```

### QueueExplorer

[QueueExplorer](http://www.cogin.com/mq/) is a commercial third-party product for managing MSMQ.

> QueueExplorer provides features beyond the built-in console, such as copying, moving, or deleting messages; saving and loading; stress testing; and viewing/editing full message bodies (with support for .NET serialized objects).

![](queue-explorer.png 'width=500')


### Mqueue Viewer

[Mqueue Viewer](https://www.mqueue.net/) is a free tool for managing MSMQ queues and messages, with additional features available in the paid version.

> Mqueue Viewer allows viewing, editing, adding, and deleting messages, and supports multiple machines/servers.

![](mqueue.png 'width=500')

### Service Bus MQ Manager

[Service Bus MQ Manager](https://github.com/danielHalan/ServiceBusMQManager) is a free application for viewing and managing MSMQ messages.

![](service-bus-mq-manager.png)
