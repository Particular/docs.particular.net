---
title: "NServiceBus Step-by-step: Retrying errors"
reviewed: 2020-08-19
summary: In this 25-30 minute tutorial, you'll learn the different causes of errors and how to manage them with NServiceBus.
redirects:
- tutorials/intro-to-nservicebus/5-retrying-errors
- tutorials/nservicebus-101/lesson-5
extensions:
- !!tutorial
  nextText: "Start: Replaying failed messages"
  nextUrl: tutorials/message-replay
---

In software systems, exceptions will occur. Even with perfect, bug-free code, problems will arise when we have to deal with the issue of connectivity. If a database is overloaded, or a web service is down, we have no recourse except to try again.

It's how we respond to exceptions that is important. When a database is deadlocked, or a web service is down, do we lose data, or do we have the ability to recover? Do our users get an error message and have to figure out how to recover on their own, or can we make it appear as though nothing ever went wrong?

In the next 25-30 minutes, you will learn the different causes of errors and see how to manage them with NServiceBus.


## Causes of errors

Where connectivity is a major concern, there are generally three broad categories of exceptions:


### Transient exceptions

Transient exceptions are those that, if immediately retried, would likely succeed.

Let's consider a common scenario: code that updates a record in the database. Two threads attempt to lock the row at the same time, resulting in a deadlock. The database chooses one transaction to succeed and the other fails. The exception message Microsoft SQL Server returns for a deadlock is this:

WARNING: Transaction (Process ID 58) was deadlocked on lock resources with another process and has been chosen as the deadlock victim. Rerun the transaction.

This is an example of a **transient exception**. Transient exceptions appear to be caused by random quantum fluctuations in the ether. If the failing code is immediately retried, it will probably succeed. Indeed, the exception message above tells us to do exactly that.


### Semi-transient exceptions

The next category involves failures such as connecting to a web service that goes down intermittently. An immediate retry will likely not succeed, but retrying after a short time (from a few seconds up to a few minutes) might.

These are **semi-transient exceptions**. Semi-transient exceptions are persistent for a limited time but still resolve themselves relatively quickly.

Another common example involves the failover of a database cluster. If a database has enough pending transactions, it can take a minute or two for all of those transactions to resolve before the failover can complete. During this time, queries are executed without issue, but attempting to modify data will result in an exception.

It can be difficult to deal with this type of failure, as it's often not possible for the calling thread to wait around long enough for the failure to resolve itself.


### Systemic exceptions

Outright flaws in your system cause **systemic exceptions**, which are straight-up bugs. They will fail every time given the same input data. These are our good friends NullReferenceException, ArgumentException, dividing by zero, and a host of other common mistakes we've all made.

In short, these are the exceptions that a developer needs to look at, triage, and fix —- preferably without all the noise from the transient and semi-transient errors getting in the way of our investigation.


## Automatic retries

In order to deal with exceptions that arise, the code for each handler is wrapped in a `try/catch` block, and [if the message transport supports it, a transaction as well](/transports/transactions.md). This means that only one of two things can happen:

 1. The message is processed successfully. All database calls succeed, all outgoing messages are dispatched to the message transport, and the incoming message is removed from the queue.
 1. The message fails. All database transactions are rolled back, any calls to `.Send()` or `.Publish()` are cancelled, and the incoming message remains in the queue to attempt processing again.

With this kind of protection in place, we're free to try to process a message as many times as we need, or at least as many times as makes sense.

[**Immediate retries**](/nservicebus/recoverability/#immediate-retries) deal with transient exceptions like deadlocks. By default, messages will be immediately retried up to 5 times. If a handler method continues to throw an exception after 5 consecutive attempts, it is clearly not a transient exception.

[**Delayed retries**](/nservicebus/recoverability/#delayed-retries) deal with semi-transient exceptions, like a flaky web service, or database failover. It uses a series of successively longer delays between retries in order to give the failing resource some breathing room. After immediate retries are exhausted, the message is moved aside for a short time – 10 seconds by default – and then another set of retries is attempted. If this fails, the time limit is increased and then the message handler will try again.

Between immediate and delayed retries, there can be [many attempts to process a message](/nservicebus/recoverability/#total-number-of-possible-retries) before NServiceBus gives up and moves the message to an error queue.

The last step, moving the message to an error queue, is how NServiceBus deals with **systemic exceptions**. In a messaging system, systemic exceptions are the cause of **poison messages**, messages that cannot be processed successfully under any circumstances. Poison messages have to be moved aside, otherwise they will clog up the queue and prevent valid messages from being processed.

We'll take a look at a few options for configuring retries in the exercise, but for all the details check out the [recoverability documentation](/nservicebus/recoverability/).


## Replaying messages

Once a message is sent to the error queue, this indicates that a systemic failure has occurred. When this happens, a developer needs to look at the message and figure out *why*.

NServiceBus embeds the exception details and stack trace into the message that it forwards to the error queue, so you don't need to search through a log file to find the details. Once the underlying issue is fixed, the message can be replayed. **Replaying a message** sends it back to its original queue in order to retry message processing after an issue has been fixed.

The [Particular Service Platform](/platform/), of which NServiceBus is a part, includes tools to make this kind of operational monitoring easy. If you'd like to learn more, check out the [message replay tutorial](/tutorials/message-replay/), which demonstrates how to use the platform tools to replay a failed message.

Sometimes, a new release will contain a bug in handler logic that isn't found until the code is deployed. When this happens, many errors can flood into the error queue at once. At these times, it's incredibly useful to be able to roll back to the old version of the endpoint, and then replay the messages through proven code. Then you can take the time to properly troubleshoot and fix the issue before attempting a new deployment.


## Exercise

In this exercise we'll throw an exception inside a message handler, and see how NServiceBus automatically retries the message.


### Throw an exception

First, let's throw an exception. For the purposes of this exercise, we'll create a specific bug in the Sales endpoint and watch what happens when we run the endpoint.

1. In the **Sales** endpoint, locate the **PlaceOrderHandler**.
1. After logging receipt of the message, throw an exception:

snippet: ThrowSystemic

Next, run the solution.

 1. In Visual Studio's **Debug** menu, select **Detach All** so that the system keeps running but does not break into the debugger when we throw our exception.
 1. In the **ClientUI** window, place an order by pressing <kbd>P</kbd>.

When we do these steps, we'll see a wall of exception messages in white text, which is log level INFO, followed by one in yellow text, which is log level WARN. The exception traces in white are the failures during immediate retries, and the last trace in yellow is the failure that hands the message over to delayed retries.

```
INFO  Sales.PlaceOrderHandler Received PlaceOrder, OrderId = e927667c-b949-47ee-8ea2-f29523909784
WARN  NServiceBus.RecoverabilityExecutor Delayed Retry will reschedule message '53ac6836-48ef-49dd-aabb-a67c0104a2a5' after a delay of 00:00:10 because of an exception:
System.Exception: BOOM
   at <stack trace>
```

Ten seconds later, the retries begin again, followed by another yellow trace, sending the message back to delayed retries. Twenty seconds after that, another set of traces. Finally, 30 seconds after that, the final exception trace will be shown in red, which is log level ERROR. This is where NServiceBus gives up on the message and redirects it to the error queue.

```
INFO  Sales.PlaceOrderHandler Received PlaceOrder, OrderId = e927667c-b949-47ee-8ea2-f29523909784
ERROR NServiceBus.RecoverabilityExecutor Moving message '53ac6836-48ef-49dd-aabb-a67c0104a2a5' to the error queue 'error' because processing failed due to an exception:
System.Exception: BOOM
   at < stack trace>
```


### Retry settings

If you like, you can experiment with changing the configuration for immediate and delayed retries. A good place to experiment is in the **Sales** endpoint's **Program.cs** file.

You can [configure immediate retries](/nservicebus/recoverability/configure-immediate-retries.md) to either change the number of retries, or disable them altogether. The default value is `5`, but you may want to set it to a higher or lower number. Many developers prefer to set it to `0` during development so that they can limit the "wall of text" effect when an exception is thrown, and then set it to a higher number for production use.

INFO: For further strategies to limit the "wall of text" effect in stack traces, especially with async code, check out the [Stack Trace Cleaning](/samples/logging/stack-trace-cleaning/) sample.

The number of retries supplied to the immediate retries API can be pulled from an appSetting to allow changing configuration between development/test/staging/production environments.

You can also [configure delayed retries](/nservicebus/recoverability/configure-delayed-retries.md) in much the same way. In addition to the number of rounds of delayed retries, you can also modify the time increase used for the delay between each round of retries.


### Transient exceptions

Throwing a big exception is an example of a systemic error. Let's see how NServiceBus reacts when we throw a more transient exception. To do this, let's introduce a random number generator so that we only throw an exception 20% of the time.

1. In the **Sales** endpoint, locate the **PlaceOrderHandler**.
1. Add a static **Random** instance to the class:

snippet: Random

3. Change the `throw` statement so that it's dependent on the random number:

snippet: ThrowTransient

4. Start the solution, and either select **Detach All** in the **Debug** menu, or just start the solution without debugging (<kbd>Ctrl</kbd>+<kbd>F5</kbd>).
4. In the **ClientUI** window, send one message at a time by pressing <kbd>P</kbd>, and watch the **Sales** window.

As you will see in the **Sales** window, 80% of the messages will go through as normal. When an exception occurs, the exception trace will be displayed once in white, and then generally succeed on the next try. After the successful retry, the other windows will continue to react as normal to complete the process.

With NServiceBus watching over your processes with automated retries, you don't have to worry about transient failures anymore. If an error is severe enough, it will progress through immediate and delayed retries and be delivered to an error queue. Then you know that it's a severe error that needs to be addressed.


## Summary

In this lesson, we explored different causes for exceptions and how NServiceBus makes those much easier to deal with by introducing automatic retries and message replay to make many transient and semi-transient exceptions just go away, and provide tools to deal with poison messages, all without our users noticing anything but perhaps a slight processing delay. This is a capability that will enable you to create truly resilient, self-healing systems that can keep running in the face of partial failure.

You've completed the last lesson in the [NServiceBus step-by-step tutorial](/tutorials/nservicebus-step-by-step). You've learned how to create endpoints, send and receive commands, publish events, and deal with message failures.

SUCCESS: Now that you've learned how to build messaging systems with NServiceBus, continue your learning and see how replaying failed messages transforms the way you build software. When a message fails, our tools let you see the exception details as well as contents of the message so you can pinpoint and fix the problem. Then you can replay the message as if nothing ever happened. Start the tutorial to experience it for yourself!
