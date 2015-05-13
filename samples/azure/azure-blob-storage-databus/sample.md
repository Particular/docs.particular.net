---
title: Azure Blob Storage DataBus Sample
summary: 'Send large attachments with NServiceBus over Azure blob storage.'
tags:
- Azure
- DataBus
- Large messages
- Message size limit
related:
- nservicebus/messaging/databus
---

 1. Run the solution. Two console applications start.
 2. Find the Sender application by looking for the one with "Sender" in its path and pressing Enter in the window to send a message. You have just sent a message that is larger than the allowed 4MB by MSMQ. NServiceBus sends it as an attachment, allowing it to reach the Receiver application.
 
## Code walk-through

WIP