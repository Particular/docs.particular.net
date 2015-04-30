---
title: Handling failures 
summary: How to subscribe to notifications regarding critical errors which adversely affect messaging in your endpoint.
tags:
- NServiceBus Host
- Self Hosting
---

NServiceBus offers an error event handler that you can use in order to receive notifications when something undesirable happens in the endpoint which involves the ability to receive or send messages. E.g. When your persistence storage that is essential for messaging goes down. 

If the bus is hosted via NServiceBus, a circuit breaker is triggered to monitor the situation for a short time and if the faulty condition has not been rectified within that time, then the process is shutdown automatically.

However, this is slightly different if you are hosting the bus in your own process. In this scenario it may not be possible for NServiceBus to determine if or when to shutdown the process. For example, the bus might be hosted within IIS in a website and bringing down the process will mean bringing down your website which may not be desirable.

It is for this reason, we offer the following API which gives you better control regardless of how you are hosting.
 
First, you specify your intent to listen to these critical error notifications regarding the bus.

<!-- import CustomHostErrorHandlingSubscription -->

Next you define what action you want to take when this scenario occurs:

<!-- import CustomHostErrorHandlingAction -->

## Why should you do this?

- If you are using NServiceBus Host, and you wish to take a custom action before the endpoint process is killed.
- If you are self hosting, by defining this action, you can get the same behavior as that of NServiceBus.host by calling `Environment.FailFast` which will kill the process. If this is a website and you don't wish to terminate the process, you can create a Fatal log entry and then dispose the bus, which you can then monitor and restart the website accordingly when convenient. By defining this action, if you're using [ServicePulse]() to monitor your web endpoint, then by doing so, it would appear `InActive`.

If you choose to not kill the process and just dispose the bus, please be aware that any `bus.Send` operations will result in `ObjectDisposedException` and you will need to cater for it.

NOTE:  When self hosting using NServiceBus Version4 and you did not explicitly define this action, while the bus was shutdown it wasn't disposed. Therefore the endpoint allowed you to send messages. However since the bus was shutdown, it did not process any messages that arrived in the queue. Therefore the process was running but not processing messages. By defining this action, you can now be aware of this situation and dispose the bus and optionally terminate the process based on your needs.

