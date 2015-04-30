---
title: Handling failures 
summary: How to subscribe to notifications regarding critical errors which adversely affect messaging in your host.
tags:
- NServiceBus Host
- Self Hosting
---

NServiceBus offers an API that you can use in order to receive notifications when something undesirable happens which involves the ability to receive or send messages. e.g. your persistence storage that is essential for messaging goes down. 

If the bus is hosted via NServiceBus, a circuit breaker is triggered and if the faulty condition has not been rectified within a short period of time then the process is shutdown.

However, this is slightly different if you are hosting the bus in your own process. In this scenario it may not be possible for us to determine the right time to shutdown the process. For example, the bus might be hosted within IIS in a website and bringing down the process will mean bringing down your website which may not be desirable.

It is for this reason, we offer the following API which gives you the better control when you are self hosting or if you want better control of the shut down process in a NServiceBus.Host.
 
NSB v4

First, you specify your intent to listen to these critical error notifications regarding the bus

```c#
Configure.Instance.DefineCriticalErrorAction(OnCriticalError);
```

Next you define what action you want to take when this scenario occurs:

```c#
private void OnCriticalError(string errorMessage, Exception exception)
{
    Logger.Fatal(string.Format("CRITICAL: {0}",errorMessage), exception);

    // If you want the process to be active, dispose the bus. 
    // Keep in mind that when the bus is disposed, sending messages will throw with an ObjectDisposedException.
    bus.Dispose();

    // If you want to kill the process, raise a fail fast error as shown below. 
    // Environment.FailFast(String.Format("The following critical error was encountered by NServiceBus:\n{0}\nNServiceBus is shutting down.", errorMessage), exception);
}
```

