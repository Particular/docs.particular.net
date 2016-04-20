---
title: Azure Storage Queues Transport Transaction Support
summary:  Understanding transactions in Azure Storage Queue Transport.
tags:
- Azure
- Cloud
- Transactions
reviewed: 2016-04-20
---


## Version 6 and above

Azure Storage Queues Transport supports `ReceiveOnly` and `Unreliable` levels.


### Transport transaction - Receive Only

The message is not removed from the queue directly after receive, but it's hidden for 30 seconds. That prevents other instances from picking it up. If the receiver fails to process the message withing that timeframe or explicitly abandons the message, then the message will become visible again. Other instances will be able to pick it up.


### Unreliable (Transactions Disabled)

The message is deleted from the queue directly after receive operation completes, before it is processed.
