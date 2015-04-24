---
title: Fault Tolerance
summary: See how NServiceBus messaging can get past all sorts of failure scenarios.
tags: []
redirects:
- nservicebus/nservicebus-step-by-step-guide-fault-tolerance-code-first
---

### Durable Messaging

*  Run the solution and hit Enter on the 'Client' console a couple of times to make sure the messages are being processed. 
   

**Client Output**

```
Press 'Enter' to send a message.To exit, Ctrl + C
Sent a new message with id: 5a1ca67b03ae4b38b99e1fd66ebc97eb
Sent a new message with id: 30f443c4ce454de5be8541cafb0da332
Sent a new message with id: 2c9f0f60763243aeb16e1688f31b1f53
```

**Server Output**

```
Press any key to stop program

Message received. Id: 00000000-0000-0000-0000-000000000000
Message received. Id: 5a1ca67b-03ae-4b38-b99e-1fd66ebc97eb
Message received. Id: 30f443c4-ce45-4de5-be85-41cafb0da332
Message received. Id: 2c9f0f60-7632-43ae-b16e-1688f31b1f53
```

#### Queue up multiple messages

* Then, kill the 'Server' console (endpoint) but leave the 'Client' console (endpoint) running.
* Hit Enter on the 'Client' console a couple of times to see that the 'Client' application isn't blocked even when the other process it's trying to communicate with is down. This makes it easier to upgrade the backend even while the front-end is still running, resulting in a more highly-available system.
*  Now, leaving the 'Client' console running, view the `Samples.FaultTolerance.Server` queue in MSMQ.  Note that All the messages sent to the 'Server' endpoint are queued, waiting for the process to come back online. You can click each message, press F4, and examine its properties specifically BodyStream, where the data is.

#### Consume those messages

Now bring the 'Server' endpoint back online by right clicking the project, Debug, Start new instance.

As you can see the 'Server' processes all those messages, and if you go back to the `Samples.FaultTolerance.Server` queue it is empty.

### Fault tolerance

Consider scenarios where the processing of a message fails. This could be due to something transient like a deadlock in the database, in which case some quick retries overcome this problem, making the message processing ultimately succeed. NServiceBus automatically retries immediately when an exception is thrown during message processing, up to five times by default (which is configurable).

If the problem is something more protracted, like a third party web service going down or a database being unavailable, it makes sense to try again sometime later.

This is called the [Second Level Retries](/nservicebus/errors/second-level-retries.md) (SLR) functionality of NServiceBus.

SLR is enabled by default, the default policy will defer the message `10*N` (where N is number of retries) seconds 3 times (60 sec total), resulting in a wait of 10s, then 20s, and then 30s; after which the message moves to the configured ErrorQueue.

#### Make the handler fail 

So, let's make the handling of messages in the 'Server' endpoint fail. Open `MyHandler.cs`.

<!-- import MyHandler -->

Note the commented out `throw new Exception`. Uncomment that line.

Run your solution again, but this time use `Ctrl-F5` so that Visual Studio does not break each time the exception is thrown, sending a message from the 'Client' console.

You should see the endpoint scroll a bunch of warnings, ultimately putting out an error, and stopping, like this:

**Server Output**

```
BuildAgent\work\3206e2123f54fce4\src\NServiceBus.Core\Unicast\UnicastBus.cs:line 826
   at NServiceBus.Unicast.Transport.TransportReceiver.OnTransportMessageReceived(TransportMessage msg) in c:\BuildAgent\
work\3206e2123f54fce4\src\NServiceBus.Core\Unicast\Transport\TransportReceiver.cs:line 411
   at NServiceBus.Unicast.Transport.TransportReceiver.ProcessMessage(TransportMessage message) in c:\BuildAgent\work\320
6e2123f54fce4\src\NServiceBus.Core\Unicast\Transport\TransportReceiver.cs:line 344
   at NServiceBus.Unicast.Transport.TransportReceiver.TryProcess(TransportMessage message) in c:\BuildAgent\work\3206e21
23f54fce4\src\NServiceBus.Core\Unicast\Transport\TransportReceiver.cs:line 228
   at NServiceBus.Transports.Msmq.MsmqDequeueStrategy.Action() in c:\BuildAgent\work\3206e2123f54fce4\src\NServiceBus.Co
re\Transports\Msmq\MsmqDequeueStrategy.cs:line 266
2015-04-24 10:59:57.752 WARN  NServiceBus.Faults.Forwarder.FaultManager Message with '15f99a26-fc38-4ce4-9bc1-a48400b518
4c' id has failed FLR and will be handed over to SLR for retry attempt 3.
```

While the endpoint can now continue processing other incoming messages (which will also fail in this case as the exception is thrown for all cases), the failed message has been diverted and is being held in one of the NServiceBus internal databases.

If you leave the endpoint running a while longer, you'll see that it tries processing the message again. After three retries, the retries stop and the message ends up in the error queue (in the default configuration this should be after roughly one minute).

NOTE: When a message cannot be deserialized, it bypasses all retry and moves directly to the error queue.

### Retries, errors, and auditing

If a message fails continuously (due to a bug in the system, for example), it ultimately moves to the error queue that is configured for the endpoint after all the various retries have been performed.

Since administrators must monitor these error queues, it is recommended that all endpoints use the same error queue.

Read more about [how to configure retries](/nservicebus/errors/second-level-retries.md).

Make sure you remove the code which throws an exception before going on.