---
title: Fault Tolerance
summary: Messaging approaches to help with failure scenarios.
reviewed: 2018-03-22
component: Core
redirects:
- nservicebus/nservicebus-step-by-step-guide-fault-tolerance-code-first
related:
- nservicebus/recoverability
- samples/errorhandling
---

include: recoverability-rename

Run the solution and press Enter on the 'Client' console a few times to make sure the messages are being processed.

**Client output**

```
Press 'Enter' to send a message.
Press any key to exit
Sent a new message with id: 5a1ca67b03ae4b38b99e1fd66ebc97eb
Sent a new message with id: 30f443c4ce454de5be8541cafb0da332
Sent a new message with id: 2c9f0f60763243aeb16e1688f31b1f53
```

**Server output**

```
Press any key to exit
Message received. Id: 00000000-0000-0000-0000-000000000000
Message received. Id: 5a1ca67b-03ae-4b38-b99e-1fd66ebc97eb
Message received. Id: 30f443c4-ce45-4de5-be85-41cafb0da332
Message received. Id: 2c9f0f60-7632-43ae-b16e-1688f31b1f53
```


### Queue up multiple messages

 * Close the 'Server' console (endpoint) but leave the 'Client' console (endpoint) running.
 * Press Enter on the 'Client' console a few times and note that the 'Client' application isn't blocked even when the server application is down. This makes it easier to upgrade the back-end even while the front-end is still running, resulting in a more highly-available system.
 * Leave the 'Client' console running and view the `Samples.FaultTolerance.Server` queue in MSMQ. Note that all the messages sent to the 'Server' endpoint are queued, waiting for the process to come back online. Select each message, press F4, and examine its properties, specifically `BodyStream`, where the data is.


### Consume those messages

Now bring the 'Server' endpoint back online by right-clicking the project, selecting Debug, then Start new instance.

Note that the 'Server' processes all those messages and the `Samples.FaultTolerance.Server` queue is empty.


## Fault tolerance


### Make the handler fail

In the 'Server' application, open `MyHandler.cs`.

snippet: MyHandler

Note the commented out `throw new Exception` line. Uncomment that line.

Run the solution again, but this time use `Ctrl-F5` so that Visual Studio does not break each time the exception is thrown.

The endpoint should scroll through a bunch of warnings, ultimately outputting an error, and stopping, as follows:

**Server Output**

```
at NServiceBus.Unicast.Transport.TransportReceiver.OnTransportMessageReceived(TransportMessage msg) in
\NServiceBus.Core\Unicast\Transport\TransportReceiver.cs:line 411
at NServiceBus.Unicast.Transport.TransportReceiver.ProcessMessage(TransportMessage message) in
\NServiceBus.Core\Unicast\Transport\TransportReceiver.cs:line 344
at NServiceBus.Unicast.Transport.TransportReceiver.TryProcess(TransportMessage message) in
\NServiceBus.Core\Unicast\Transport\TransportReceiver.cs:line 228
at NServiceBus.Transports.Msmq.MsmqDequeueStrategy.Action() in
\NServiceBus.Core\Transports\Msmq\MsmqDequeueStrategy.cs:line 266
2015-04-24 10:59:57.752 WARN  NServiceBus.Faults.Forwarder.FaultManager Message
with '15f99a26-fc38-4ce4-9bc1-a48400b5184c' ID has failed Immediate Retries and will be handed over to Delayed Retries for retry attempt 3.
```

While the endpoint can now continue processing other incoming messages (which will also fail in this case as the exception is thrown for all cases), the failed message has been diverted and is being held in one of the NServiceBus internal databases.

Leave the endpoint running a while longer, and notice that it tries processing the message again. After three retries, the message ends up in the error queue (in the default configuration this should occur after roughly one minute).

Turn off Delayed Retries by uncommenting the below line in the Server Program.cs:

snippet: disable

Re-run the sample and notice the behavior. After successive retries, the message is sent to the error queue right away.

Make sure that the exception code is removed to resume processing of messages.