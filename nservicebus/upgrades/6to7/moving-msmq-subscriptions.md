---
title: Migrating MSMQ subscription messages
summary: Information on migrating MSMQ's subscription storage from an earlier version of the transport to a later one
reviewed: 2021-08-12
component: MsmqTransport
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

If a subscription queue is not configurate when using MSMQ subscription storage in NServiceBus versions 5 and 6, the subscription storage queue automatically defaults to `NServiceBus.Subscriptions`. However, as this queue name is not endpoint specific, endpoints deployed to the same server could potentially share the same subscriptions queue which is not a good practice.

Therefore, starting from NServiceBus.Transport.Msmq version 1, if a queue called `NServiceBus.Subscriptions` is detected, an exception will be thrown to prevent potential loss of messages.

To move messages from the `NServiceBus.Subscriptions` queue to the new queue, refer to the instructions outlined below.

## Create the subscriptions queue

Create a transactional queue called {EndpointName}.Subscriptions, substituting the actual name of the endpoint. This can be done by with PowerShell scripts, [Windows MMC Snap-in, or other tools like QueueExplorer](/transports/msmq/viewing-message-content-in-msmq.md#windows-native-tools). When creating queues manually using other tools, ensure that the queues are marked **transactional**.

### Using PowerShell

Use the `CreateQueue` function that's part of `CreateQueues.ps1`. This PowerShell script comes with the NServiceBus.Transport.Msmq NuGet package and is copied to a subfolder called `NServiceBus.Transport.Msmq` in the output folder of any project referencing it. Browse to the output folder to locate the scripts, for example, `bin\Debug\net472\NServiceBus.Transport.Msmq`.

2. [Load the `CreateQueues.ps1` PowerShell script](https://technet.microsoft.com/en-us/library/bb613481.aspx) and run the `CreateQueue` function as shown below:

```
    # CreateQueue -QueueName "EndpointName.Subscriptions" -Account $env:USERNAME
```

## Move the subscription messages

Once the new queue is created, use a tool like [QueueExplorer](https://www.cogin.com/mq/index.php) to locate the messages in the `NServiceBus.Subscriptions` queue and move them to the newly created subscriptions queue. If the `NServiceBus.Subscriptions`queue is shared among multiple endpoints, select only the messages intended for the endpoint that is being upgraded to the `NServiceBus.Transport.Msmq` package. Identify which messages must be moved by inspecting the message body and looking for the event information and by inspecting the subscriber queue name in the `LABEL` column.

Select the identified messages in the right-hand pane, then right-click and select `Cut`. Now select the newly created subscriptions queue by clicking on the name of the queue. Right-click on the messages pane and select `Paste` to move the messages.

Once all the subscription messages have been moved, delete the `NServiceBus.Subscriptions` queue.

WARNING: If more than one endpoint is sharing the same queue, ensure that individual subscription queues are first created and all the relevant messages are either moved or copied to the appropriate newly-created queues before deleting the `NServiceBus.Subscriptions` queue. QueueExplorer can also be used to copy messages, if a copy of the same subscription message must be added to more subscription queues.

![Moving Messages using QueueExplorer](moving-messages.png)
