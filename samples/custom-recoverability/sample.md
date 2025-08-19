---
title: Custom Recoverability
summary: Custom Recoverability Messaging Policy approaches to help with failure scenarios.
reviewed: 2025-06-20
component: Core
redirects:
- nservicebus/nservicebus-step-by-step-guide-custom-recoverability
related:
- nservicebus/recoverability
- nservicebus/recoverability/custom-recoverability-policy
- samples/errorhandling
- samples/faulttolerance

---

This sample demonstrates how to use custom recoverability policy. Here the default recoverability is invoked when a specific exception type happens and at all other times it uses a custom recoverability action.

downloadbutton

## Sample structure

The sample contains three projects:

- Shared - The Shared project is for shared classes including message definitions
- Client - A console application responsible for sending the messages.
- Server - A console application responsible for receiving the messages from the client.

## Running the sample

To start, the sample is configured to run without failing:

1. Run the solution. Two console applications start, the Client and the Server.
1. In the Client console, press "Enter" to send a message.
1. In the Server console, this message is received and an entry is logged in the console

### Client output

```
Press 'Enter' to send a message.
Press any key to exit
Sent a new message with id: 87283b82a892456d81eb1bfa05fb72e2
```

### Server output

```
Press any key to exit
Message received. Id: 87283b82a892456d81eb1bfa05fb72e2
```

## Fault tolerance with custom recoverability

In the 'Server' application, open `Program.cs`. There is a custom policy `MyCustomRetryPolicy` where the default NServiceBus [recoverability policy](/nservicebus/recoverability/) is invoked when an `ArgumentNullException` exception is encountered and at all other times the message is not retried and sent directly to the "error" queue.

snippet: mycustomretrypolicy

The custom policy is added to the recoverability as below

snippet: addcustompolicy

Custom headers are added to the message before sending to the "error" queue.

snippet: addcustomheaders

## Make the handler fail

In the 'Server' application, open `MyHandler.cs`. Uncomment the `throw new ArgumentNullException` line.

snippet: MyHandler

As per the custom recoverability policy, when the solution is run, the exception is thrown and the default [recoverability policy](/nservicebus/recoverability/) will be invoked before finally moving to the "error" queue.


### Server Output

```
INFO  Message received. Id: 76d8b5c3-c41b-4179-b995-4e68f3c5b7eb
INFO  Immediate Retry is going to retry message '1e0d3ee9-3ab0-4be9-bf45-b0330060b9e8' because of an exception:
System.ArgumentNullException: Value cannot be null. (Parameter 'Uh oh - something went wrong....')
   at MyHandler.Handle(MyMessage message, IMessageHandlerContext context) in C:\Particular\docs.particular.net\samples\custom-recoverability\Core_8\Server\MyHandler.cs:line 15
   at NServiceBus.Pipeline.MessageHandler.Invoke(Object message, IMessageHandlerContext handlerContext) in /_/src/NServiceBus.Core/Pipeline/Incoming/MessageHandler.cs:line 43

WARN  Delayed Retry will reschedule message '1e0d3ee9-3ab0-4be9-bf45-b0330060b9e8' after a delay of 00:00:30 because of an exception:
System.ArgumentNullException: Value cannot be null. (Parameter 'Uh oh - something went wrong....')
   at MyHandler.Handle(MyMessage message, IMessageHandlerContext context) in C:\Particular\docs.particular.net\samples\custom-recoverability\Core_8\Server\MyHandler.cs:line 15
   at NServiceBus.Pipeline.MessageHandler.Invoke(Object message, IMessageHandlerContext handlerContext) in /_/src/NServiceBus.Core/Pipeline/Incoming/MessageHandler.cs:line 43
   at NServiceBus.InvokeHandlerTerminator.Terminate(IInvokeHandlerContext context) in /_/src/NServiceBus.Core/Pipeline/Incoming/InvokeHandlerTerminator.cs:line 33
   at NServiceBus.LoadHandlersConnector.Invoke(IIncomingLogicalMessageContext context, Func`2 stage) in /_/src/NServiceBus.Core/Pipeline/Incoming/LoadHandlersConnector.cs:line 40

INFO  Message received. Id: 76d8b5c3-c41b-4179-b995-4e68f3c5b7eb
ERROR Moving message '1e0d3ee9-3ab0-4be9-bf45-b0330060b9e8' to the error queue 'error' because processing failed due to an exception:
System.ArgumentNullException: Value cannot be null. (Parameter 'Uh oh - something went wrong....')
```

Delayed Retries can be turned off by uncommenting the below line in the Server Program.cs:

snippet: disable

Now when the sample is re-run, the message is sent to the error queue without the delayed retries after successive immediate retries.

Now, in the "Server" application, open `MyHandler.cs`. Comment out `throw new ArgumentNullException` and uncomment the `throw new DivideByZeroException` line. When the solution is run and the exception is thrown, the message is directly moved to the "error" queue without any retries (as per the custom policy).

```
 INFO  Message received. Id: 5570fc08-098d-4784-8902-0205ab0ae594
2023-07-01 23:09:03.270 ERROR Moving message 'e6bd7712-cf5a-4927-923e-b0330065571a' to the error queue 'error' because processing failed due to an exception:
System.DivideByZeroException: DivideByZeroException - something went wrong....
   at MyHandler.Handle(MyMessage message, IMessageHandlerContext context) in samples

```

