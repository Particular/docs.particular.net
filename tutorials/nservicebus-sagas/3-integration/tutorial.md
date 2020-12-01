---
title: "NServiceBus sagas: Integrations"
reviewed: 2020-10-22
## Once published, add to Learning Path pages on Website and remove comment here, and remove hidden attribute
#isLearningPath: true
summary: In this tutorial, learn how to use NServiceBus sagas to manage integration with external systems that communicate via HTTP.
previewImage: https://img.youtube.com/vi/BHlKPgY2xxg/maxresdefault.jpg
---

<div style="display:block;width:100%;margin:20px auto;position:relative;height:0;padding-bottom:56.25%;border:solid 1px #ccc;"><iframe src="https://www.youtube.com/embed/BHlKPgY2xxg" allow="accelerometer; encrypted-media; gyroscope; picture-in-picture" allowfullscreen frameborder="0" style="position:absolute;top:0;left:0;width:100%;height:100%;"></iframe></div>

The need for orchestration of a business process arises quickly when integrating with third parties. We'll frequently need to call a third-party service, and then depending on the result, kick off a new process locally, or perhaps even turn around and call a different third party.

We can't sit around passively waiting for events to float by to decide what needs to happen next. We need a process to take charge and execute several steps of a business process in a coordinated fashion.

In this tutorial, let's consider shipping couriers used in a retail system. To avoid any unpleasant uses of registered trademarks, let's call our two fake shipping services **Alpine Delivery** and **Maple Shipping Service**. In our fictional world, Maple is currently cheaper, so it's our preferred delivery option. However, it also seems to be less reliable. There is a 24-hour delivery SLA with our customers, so if Maple doesn't respond to our shipment request on time, we need to ask Alpine to deliver the package instead.

In this tutorial, we'll learn how to orchestrate this type of business process using an NServiceBus saga. We'll also see how we can react to failures from one or more of our third-party services.

## Exercise

In the exercises so far, we had a `ShippingPolicy` saga that was rather passive—it waited for `OrderPlaced` and `OrderBilled` to arrive (which could happen out of order) and then the order is ready to ship. In this exercise, we'll continue by implementing the actual shipment via one of our fictional shipping carriers, Alpine or Maple.

{{NOTE:
**What if I didn't do the previous tutorial?**

No problem! You can get started learning sagas with the completed solution from the [previous lesson](/tutorials/nservicebus-sagas/2-timeouts/):

downloadbutton(Download Previous Solution, /tutorials/nservicebus-sagas/2-timeouts)

The solution contains 5 projects. **ClientUI**, **Sales**, **Billing**, and **Shipping** define endpoints that communicate with each other using messages. The **ClientUI** endpoint mimics a web application and is an entry point to the system. **Sales**, **Billing**, and **Shipping** contain business logic related to processing, fulfilling, and shipping orders. Each endpoint references the **Messages** assembly, which contains the classes that define the messages exchanged in our system. To see how to start building this system from scratch, check out the [NServiceBus step-by-step tutorial](/tutorials/nservicebus-step-by-step/).

Although NServiceBus only requires .NET Framework 4.5.2, this tutorial assumes at least Visual Studio 2017 and .NET Framework 4.6.1.
}}

### A new saga

While it would be possible to implement the new functionality in our existing `ShippingPolicy` saga, it's probably not a good idea. That saga is about deciding whether or not to ship, while we are now dealing with the process of executing that shipment. It's best to keep the [single responsibility principle](https://en.wikipedia.org/wiki/Single-responsibility_principle) in mind and keep them separate. The result will be simpler sagas that are easier to test and easier to evolve in the future.

In the `ShippingPolicy` saga class (inside the **Shipping** endpoint project) we already have the `ShipOrder` being sent from the `ProcessOrder` method at the end of the saga. Currently, this is being processed by the `ShipOrderHandler` class, also in the **Shipping** endpoint. Our aim is to replace that handler with a new saga.

While the previous saga `ShippingPolicy` was a passive observer, this new saga is commanding an active workflow, so we will call it `ShipOrderWorkflow`.

To get started, create a new class in the **Shipping** project called `ShipOrderWorkflow`:

snippet: Creation-SagaStart

This creates a saga that is started by `ShipOrder` messages and uses `ShipOrderData` to store its data. Because the saga data is tightly coupled to the saga implementation, we include it as an internal class. The `Handle` method is currently empty—we will need to complete that in just a bit.

NOTE: For a more in-depth explanation of the basic saga concepts, check out [NServiceBus sagas: Saga basics](/tutorials/nservicebus-sagas/1-saga-basics/).

The `OrderId` is what makes the saga unique. We need to show the saga how to identify the unique **correlation property** `OrderId` in the incoming `ShipOrder` message, and how to relate it to the `OrderId` property in the saga data.

To do that, implement the saga base class's `ConfigureHowToFindSaga` class as shown here, or let the compiler generate the method and fill it in:

snippet: Creation-ConfigureHowToFindSaga

You can now delete the `ShipOrderHandler` class that was created in a previous tutorial since this newly created saga will replace its functionality.

### Calling web services

Recall that while Maple Shipping Service is cheaper, and thus our preferred vendor, they are sometimes unreliable, in which case we want to use Alpine as a backup. To accomplish this, we'll send a `ShipWithMaple` command immediately and request a `ShippingEscalation` timeout in case Maple happens to be unreliable today.

To do that, we'll first need to add a class for the command and for the timeout.

In the **Messages** project, add a class for `ShipWithMaple`:

snippet: ShipWithMapleCommand

When we create a command, we need to create routing information for it so that when we send it, we know where it needs to go. For this tutorial, we'll keep all of the activity within the **Shipping** endpoint.

In the **Shipping** project, open the **Program.cs** file and add the last routing entry for `ShipWithMaple`:

snippet: ShipWithMaple-Routing

Next, let's create the `ShippingEscalation` timeout class, which will serve as the alarm clock or reminder service for our saga.

In the **Shipping** project, go to the `ShipOrderWorkflow` and add `ShippingEscalation` as a *nested internal* class.

snippet: ShippingEscalationTimeout

Note that the `ShippingEscalation` timeout class should be **nested inside** the `ShipOrderWorkflow` class and marked as `internal`. It is very tightly coupled to the `ShipOrderWorkflow`—there's no need to use it anywhere else. It also doesn't need any special interface or contents. A timeout, after all, is just an alarm clock—we get all we need to know just from the type name. Everything else will already exist in the saga's stored data.

NOTE: For more on saga timeouts, check out [NServiceBus sagas: Timeouts](/tutorials/nservicebus-sagas/3-integration/).

Now, in our `ShipOrderWorkflow` class we can implement the `Handle` method as follows:

snippet: HandleShipOrder

We've sent a `ShipWithMaple` command and requested a `ShippingEscalation` timeout of 20 seconds so that if Maple doesn't respond within that time we can ship with Alpine instead. Also, notice how we can use `Data.OrderId` immediately—because of the mapping in our `ConfigureHowToFindSaga` method, NServiceBus already knows that the saga data's `OrderId` property needs to be filled using the message's `OrderId` property, so it helpfully prefills this for us.

{{NOTE:
**Why 20 seconds?**

In real life, timeouts like these would more likely be measured in hours, days, or even months. In this tutorial, we want to keep it to a matter of seconds so we can watch the entire process play out immediately.
}}

Now that we've created this command and timeout, we need to do something with them, starting with the command.

### Shipping with Maple

We're going to use a separate message handler to communicate with the Maple web service—or at least, we would in real life. For this tutorial, we'll use a fake message handler.

{{NOTE:
**Why not contact the web service directly within the saga?**

While the saga is processing the message, it holds a database lock on your saga data so that if multiple messages from the same saga try to modify the data at the same time, only one of them will succeed. This presents two problems for a web service request. First, a web request can't be added to a database transaction, meaning that if a concurrency exception occurs, the web request can't be undone. The second is that the time it takes for the web request to complete will hold the saga database transaction open longer, making it even more likely that another message will be processed concurrently, creating more contention.

This is why a saga should be only a message-driven state machine: a message comes in, decisions are made, and messages go out. Leave all the other processing to external message handlers as shown in this tutorial.
}}

We'll need to create the message handler as well as configure the routing so the saga knows where to send the `ShipWithMaple` command.

Rather than call a real web service, our example will fake it by delaying for a random time and then replying to the `ShipOrderWorkflow` saga with a `ShipmentAcceptedByMaple` message.

First, let's add the `ShipmentAcceptedByMaple` message to our **Messages** project:

snippet: ShipmentAcceptedByMapleMessage

Then, add the following message handler to the **Shipping** project:

snippet: ShipWithMapleHandler

There are a few things to point out here:

* We're not really calling a web service, we're just faking it. The random delay of up to 60 seconds emulates how Maple is known to be unreliable. If you recall, the timeout on our saga will only wait 20 seconds before the `ShippingEscalation` timeout is triggered.
* `ShipmentAcceptedByMaple` is marked as an `IMessage`, not an `ICommand` or `IEvent`. Reply messages, created by using `context.Reply(…)`, are not command or events, they're just messages.
* `ShipmentAcceptedByMaple` doesn't have any properties, at all.

The second point is due to a process called **auto-correlation**. When the saga sends the `ShipWithMaple` command, it includes a header containing the saga's SagaId. The `ShipWithMapleHandler` will then reflect that SagaId header back in the reply message when we call `context.Reply(…)`. This means we don't need to propagate any identifying information (in this case, our `OrderId`) in the response message. It also means that we don't have to do anything in the saga's `ConfigureHowToFindSaga` for it to know how to handle the returning `ShipmentAcceptedByMaple` reply message. Because it's a reply message, we also don't have to specify routing for it—because it's a reply it goes back to the saga that sent the `ShipWithMaple` command.

In essence, because of the tight coupling between the `ShipOrderWorkflow` saga, the `ShipWithmaple` command, `ShipWithmapleHandler`, and `ShipmentAcceptedByMaple` reply message, we get to take a few shortcuts and leave the routing and correlation duties up to NServiceBus.

Another option would have been to publish `ShipmentAcceptedByMaple` as an event, but then we would have needed to include `OrderId` as a property. This makes sense, because while a reply message is only meant for the saga, any endpoint could subscribe to an event, and in that case, the event message wouldn't make sense without containing the `OrderId` identifying it.

### Success with Maple

If the Maple service can respond quickly enough, our saga will receive the `ShipmentAcceptedByMaple` reply message. Let's update our saga to handle that reply message.

In the **ShipOrderWorkflow** saga class, implement the additional interface `IHandleMessages<ShipmentAcceptedByMaple>` and implement as shown here:

snippet: ShipWithMaple-ShipmentAccepted

In this handler, we record that the shipment was accepted by Maple in our saga data. To store that data, we need to update our saga state in the `ShipOrderData` class:

snippet: ShipWithMaple-Data

Right now, a notification that Maple accepted the shipment is logged, and the flag `ShipmentAcceptedByMaple` is set. The saga does nothing else.

In a real-world scenario, perhaps another message needs to be sent so that the customer can be notified and a tracking code can be provided, or we can simply end the saga using `MarkAsComplete()`. If we did mark the saga as completed, then when the timeout message we requested arrived it would be ignored, since the saga instance is no longer active, and a timeout cannot start a saga.

### Shipping with Alpine

If the Maple integration handler does not respond in time, the timeout message will arrive, and we need to handle it. It's important to remember that this timeout might be triggered either before or after Maple responds, so we need to be able to handle either circumstance. If we haven't heard back from Maple yet, we're going to want to record that we're sending the order to Alpine, because it's still possible for the Maple service to respond late.

So first, let's update our saga data again. Inside the **ShipOrderWorkflow**, update the **ShipOrderData** class to add a `ShipmentOrderSentToAlpine` property:

snippet: ShipWithAlpine-Data

We're also going to need a message to send to the Alpine adapter:

snippet: ShipWithAlpineCommand

And we're going to continue to keep all of this within the **Shipping** service, so let's add routing instructions for this new message type:

snippet: ShipWithAlpine-Routing

Now that we have those building blocks, we can return to the **ShipOrderWorkflow** class and implement `IHandleTimeouts<ShipOrderWorkflow.ShippingEscalation>` as shown here:

snippet: ShippingEscalation

If the shipment was not accepted by Maple, the system needs to execute the shipment via Alpine. It's less likely something will go wrong since the web service is more reliable. But we need to expect that anything could happen and be prepared for it. Therefore we also request another timeout.

Note that this timeout handler also checks `Data.ShipmentOrderSentToAlpine` to see if we have already attempted to send the order to Alpine. This is because the same timeout message type is used twice. We could have created a separate timeout message type, but using a single type allows us to verify several possible scenarios with if/then logic in the same method, making the end result easier to read. We'll return to this method later in this tutorial.

### Late arrivals

Before we move on, it's important to remember that even once the timeout handler executes and we move on to requesting shipment through Alpine, it's _still possible for Maple to respond with `ShipmentAcceptedByMaple`_, but just later than we were willing to wait. This raises some additional concerns which we will discuss in the [Edge cases](#exercise-edge-cases) section toward the end, but for now, we need to change the handler so that the late arrival of a `ShipmentAcceptedByMaple` will not end the saga.

In the **ShipOrderWorkflow**, modify the handler for `ShipmentAcceptedByMaple` like this:

snippet: ShipWithMaple-ShipmentAcceptedRevision

With this chane, if the `ShipmentAcceptedByMaple` message arrives after we've already set `Data.ShipmentOrderSentToAlpine` to `true`, the message handler will be a no-op and the message will essentially be ignored. It's possible in real life that we would want to send additional messages to cancel the shipment via Maple. This will be discussed in more detail in [Edge cases](#exercise-edge-cases).

Now let's move on to handling the response from Alpine.

### Alpine response

Just like with Maple, we'll need an external handler to mimic contacting Alpine Delivery, and a response message type so that Alpine can deliver its answer back to our saga.

Add this message to your **Messages** project:

snippet: ShipmentAcceptedByAlpineMessage

Then, add this message handler to your **Shipping** project, which should look very similar to the Maple handler from earlier:

snippet: ShipWithAlpineHandler

Just like with the Maple case, we don't need to add any routing for the `ShipmentAcceptedByAlpine` response message. As a reply, NServiceBus will know where to send the message, and it will also include the `SagaId` with the message headers, just as before.

### Success with Alpine

If the package can be shipped via Alpine, our saga will receive a `ShipmentAcceptedByAlpine` reply message. Let's handle that now. First, we'll want to add a property to our saga data to show that the order was accepted by Alpine:

snippet: AcceptedByAlpine-Data

Then, in the **ShipOrderWorkflow** class, implement `IHandleMessages<ShipmentAcceptedByAlpine>` as follows:

snippet: ShipmentAcceptedByAlpine

In the same way as with Maple, we're not currently taking any action once the package is successfully shipped by Alpine other than to mark the saga as complete, which removes the saga data from the database. It might be appropriate to publish an event such as `ShipmentAccepted` at this point. But first, let's look at some edge cases that could arise.

### Edge cases

So far we've handled two scenarios:

1. We request shipment via Maple and it responds in time that the shipment was accepted.
2. We request shipment via Maple, it doesn't reply in time and we request another shipment via Alpine.

But what happens if *none* of the shipping providers accept the shipment? In that case, the saga will be stalled and our package will never ship. When we sent the request to Alpine (the second choice) we did request another `ShippingEscalation` timeout, but when that timeout comes due the same `Timeout()` method is executed again, and this line of code will be executed:

snippet: EdgeCases-IfShipmentAccepted

In this case, the shipment was not accepted by Maple, but we *already sent the request to Alpine*, and now we have returned to the timeout handler. It means that both attempts to request delivery have failed. That could happen for several reasons, such as the Alpine web service being down for maintenance, or perhaps a network issue prevented our request from being received.

One way to handle this situation is to notify the sales department who can handle the issue manually by calling either shipment provider and request delivery.

The best way to notify the sales department is via an event. Add `ShipmentFailed` to your **Messages** project:

snippet: ShipmentFailedEvent

Then, in the **ShipOrderWorkflow**, modify the `Timeout` method to handle the new case. Because both cases depend on `!Data.ShipmentAcceptedByMaple` we'll also refactor a bit for clarity:

snippet: EdgeCases-ShipmentFailed

The saga does not send any emails or save information about the failure to a database. Remember, it's a message-driven state machine! It merely publishes an event, so another handler can take the appropriate action. Our saga is only orchestrating the process.

There could be more scenarios similar to the one mentioned above. As developers we're tempted to solve such problems via code, ensuring consistency and avoiding race conditions. However, in reality, deciding how to handle such edge cases is a business decision.

For example, consider that as the saga currently stands, it's possible for Maple to accept a shipment but too late to stop the saga from requesting shipment via Alpine. In that circumstance, both providers could attempt to ship the package!

Dealing with these sorts of edge cases is not necessarily a technical decision, but a business one. Perhaps generating a shipment record that is never fulfilled is an acceptable solution. Perhaps once orders are accepted by Alpine, a `CancelShipment` command needs to be sent to Maple to ensure no shipment is created. Perhaps the commands to the shipment providers need to include a `DoNotProcessAfter` property so that messages that arrive "too late" are discarded. It depends on the exact business requirements.

In software, timeframes between business decisions can scale down to the millisecond, leading to apparent race conditions. But in real life, [race conditions don't exist](https://udidahan.com/2010/08/31/race-conditions-dont-exist/). It's important to ask business stakeholders what would happen in real life if the events had happened by phone rather than milliseconds apart, and use that to guide your workflows. Be careful and ensure you discuss such edge cases with business stakeholders before you jump straight to the implementation.

## Running the solution

The solution is configured to have [multiple startup projects](https://docs.microsoft.com/en-us/visualstudio/ide/how-to-set-multiple-startup-projects), so when we run the solution (**Debug** > **Start Debugging** or press <kbd>F5</kbd>) it should open the four console applications, one window for each messaging endpoint.

In the **ClientUI** application, press <kbd>P</kbd> to place an order. To see what our saga does, we want to watch the **Shipping** endpoint and don't care too much about what hapepns in **Sales** or **Billing**, but we do need them running to do their part.

It will take some time after placing an order to see anything in **Shipping** because the **BuyersRemorsePolicy** in **Sales** (from the [saga timeouts tutorial](../2-timeouts/) is implementing a 20-second timeout. If you'd like to speed this up for testing, you can edit **BuyersRemorsePolicy.cs** in the **Sales** project by temporarily setting the requested timeout to `TimeSpan.FromSeconds(1)`.

Based on the randomized timeouts that occur in the Maple and Alpine handlers, there are a few different ways the saga could execute.

### Happy path

The happy path for this workflow is for Maple, our preferred provider, to respond quickly. When that occurs, the output in the **Shipping** console application will look like this:

```
12:48:02.112 INFO  ShipOrderWorkflow for Order [19da7b54-36c0-4f68-9db5-713738dccfcf] - Trying Maple first.
12:48:02.152 INFO  ShipWithMapleHandler: Delaying Order [19da7b54-36c0-4f68-9db5-713738dccfcf] 7 seconds.
12:48:09.196 INFO  Order [19da7b54-36c0-4f68-9db5-713738dccfcf] - Successfully shipped with Maple
12:48:23.532 INFO  No saga found for timeout message c6dad340-8ee7-4d54-b23b-ac7c0135d2fd, ignoring since the saga has been marked as complete before the timeout fired
```

In this case, shipping via Maple was attempted first, and Maple responded in 7 seconds, which is shorter than the requested 20-second timeout.

Note that the last `INFO` line mentions that a saga could not be found for a timeout message. This was our 20-second `ShippingEscalation` timeout, but by the time the timeout had arrived, the `ShipmentAcceptedByMaple` response had already been processed, resulting in the saga ending with the call to `MarkAsComplete()`. The saga data was deleted, and as a result, the timeout was ignored.

_**This is perfectly fine.**_

Timeouts are designed to be reminders for the saga to take action. If the saga determines before that time that its work is done, that's OK. That's why the log message (which comes from NServiceBus, not the code in ShipOrderWorkflow) is presented as `INFO` and not a warning or error.

### Maple is slow, Alpine is fast

The second case is when Maple takes longer than the 20-second timeout, but Alpine responds quickly.

```
13:24:47.214 INFO  ShipOrderWorkflow for Order [a4bd57a8-e835-49b4-b507-90d1a23cd84e] - Trying Maple first.
13:24:47.275 INFO  ShipWithMapleHandler: Delaying Order [a4bd57a8-e835-49b4-b507-90d1a23cd84e] 42 seconds.
13:25:29.324 INFO  Order [a4bd57a8-e835-49b4-b507-90d1a23cd84e] - No answer from Maple, let's try Alpine.
13:25:29.386 INFO  ShipWithAlpineHandler: Delaying Order [a4bd57a8-e835-49b4-b507-90d1a23cd84e] 3 seconds.
13:25:32.421 INFO  Order [a4bd57a8-e835-49b4-b507-90d1a23cd84e] - Successfully shipped with Alpine
13:25:50.592 INFO  No saga found for timeout message 9cc0f019-f529-4c00-8eeb-ac7c01401c70, ignoring since the saga has been marked as complete before the timeout fired
```

Here, we see that Maple was attempted, but took 42 seconds to respond, which is past our requested 20-second timeout. So instead, the order was shipping via Alpine, which responded in 3 seconds, which was successful.

Once again, a timeout was discarded after the saga completed its work, but in this case, it was the second timeout designed to make sure Alpine responded on time.

### Everything is slow

In the last case, both Maple and Alpine will take longer than their configured timeouts to respond:

```
13:36:21.825 INFO  ShipOrderWorkflow for Order [54775217-49ee-4856-82e5-1ccc432d1d60] - Trying Maple first.
13:36:21.861 INFO  ShipWithMapleHandler: Delaying Order [54775217-49ee-4856-82e5-1ccc432d1d60] 42 seconds.
13:37:03.914 INFO  Order [54775217-49ee-4856-82e5-1ccc432d1d60] - No answer from Maple, let's try Alpine.
13:37:03.965 INFO  ShipWithAlpineHandler: Delaying Order [54775217-49ee-4856-82e5-1ccc432d1d60] 24 seconds.
13:37:27.998 WARN  Order [54775217-49ee-4856-82e5-1ccc432d1d60] - No answer from Maple/Alpine. We need to escalate!
13:37:28.032 INFO  Could not find a started saga for 'Messages.ShipmentAcceptedByAlpine' message type. Going to invoke SagaNotFoundHandlers.
```

We can see from the output that both timeouts were reached, resulting in the `WARN` statement, which we know would be accompanied by the `ShipmentFailed` event being published, but since we don't have any handler for that we don't see any evidence in the log.

We also see a new message at the end, similar what happened when timeout messages were being ignored:

```
INFO  Could not find a started saga for 'Messages.ShipmentAcceptedByAlpine' message type. Going to invoke SagaNotFoundHandlers.
```

This is caused by the `ShipmentAcceptedByAlpine` message being returned _after_ the second timeout has already given up on Alpine, published `ShipmentFailed`, and marked the saga as complete, removing it from storage. As we've defined the saga thus far, this is working as intended, but this is another place where it all comes down to business requirements.

It is possible to handle these instances by [creating an `IHandleSagaNotFound` implementation](/nservicebus/sagas/saga-not-found.md). Another possibility would be to keep the saga alive for longer (by not calling `MarkAsComplete()`) and setting another timeout to clean up the saga later on. It really depends on what the exact rules are for your specific scenario, which you can only discover through consultation with business stakeholders.


## Summary

In this lesson, we learned about commander sagas that execute several steps within a business process. Sagas orchestrate and delegate the work to other handlers. The reason for delegation is to adhere to the Single Responsibility Principle and to avoid potential contention. We've also taken another look at timeouts. And finally, we've seen how different scenarios in our business process can be modeled and implemented using sagas.

For more information on sagas, check out the [saga documentation](/nservicebus/sagas/) or our [other saga tutorials](/tutorials/nservicebus-sagas/). If you've got questions, you could also [talk to us about a proof of concept](https://particular.net/proof-of-concept), or chat with one of our developers using the chat box in the corner.