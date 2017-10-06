---
title: Migrating Msmq subscription messages
reviewed: 2017-10-5
component: MsmqTransport
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

When using Msmq Subscription storage in NServiceBus Versions 5.x and Versions 6.x, when a subscription queue is not configured, the subscription storage queue automatically defaulted to "NServiceBus.Subscriptions". However, as this queue name was not endpoint specific, endpoints deployed to the same server could potentially share the same subscriptions queue which is undesirable. 

To rectify this, starting from Version 1.0 and above of NServiceBus.Transport.Msmq, if a queue called "NServiceBus.Subscriptions" is detected, an exception will be thrown to prevent potential loss of messages. 

1. Create a transactional queue called [EndpointName].Subscriptions. This can be done by executing the powershell script, `CreateQueues.ps1` that comes packaged with the NuGet package. Browse to the lib folder to locate the scripts, for example, `C:\Users\username\.nuget\packages\nservicebus.transport.msmq\1.0.0\lib\net452\Scripts`. 

2. [Load the `CreateQueues.ps1` powershell script](https://technet.microsoft.com/en-us/library/bb613481.aspx) and run the `CreateQueue` function as shown below:

```
    # CreateQueue -QueueName "EndpointName.Subscriptions" -Account $env:USERNAME
```

3. Use a tool like [QueueExplorer](http://www.cogin.com/mq/index.php) to locate the messages in the `NServiceBus.Subscriptions` queue to move them to the new subscriptions queue. Select the message in right hand pane. Right-Click and select the `Cut` option. Now select the newly created subscriptions queue. Right click on the messages pane and select the `Paste` option to move the message. 

4. Once all the subscription messages have been moved, delete the `NServiceBus.Subscriptions` queue.

WARNING: If more than one endpoint is sharing the same queue, ensure that individual subscription queues are first created and all the relevant messages are moved to the appropriate queues before deleting the `NServiceBus.Subscriptions` queue.

![Moving Messages using QueueExplorer](moving-messages.png)



