---
title: "NServiceBus sagas: Integrations"
reviewed: 2020-10-22
## Once published, add to Learning Path pages on Website and remove comment here, and remove hidden attribute
#isLearningPath: true
hidden: true
summary: "third party services integration."
---

The need for orchestration of a business process arises quickly when integrating with third parties. We'll frequently need to call a third-party service, and then depending on the result, kick off a new process locally, or perhaps even turn around and call a different third party.

We can't sit around passively waiting for events to float by to decide what needs to happen next. We need a process to take charge and execute several steps of a business process.

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

While it would be possible to implement the new functionality in our existing `ShippingPolicy` saga, it's probably not a good idea. That saga is about making the decision whether or not to ship, while we are now dealing with the process of executing that shipment. It's best to keep the [single responsibility principle](https://en.wikipedia.org/wiki/Single-responsibility_principle) in mind and keep them separate. The result will be simpler sagas that are easier to test and easier to evolve in the future.

In the `ShippingPolicy` saga class (inside the **Shipping** endpoint project) we already have the `ShipOrder` being sent from the `ProcessOrder` method at the end of the saga. Currently, this is being processed by the `ShipOrderHandler` class, also in the **Shipping** endpoint. Our aim is to replace that handler with a new saga.

While the previous saga `ShippingPolicy` was a passive observer, this new saga is commanding an active workflow, so we will call it `ShipOrderWorkflow`.

To get started, create a new class in the **Shipping** project called `ShipOrderWorkflow`:

snippet: Creation-SagaStart

This creates a saga that is started by `ShipOrder` messages and uses `ShipOrderData` to store its data. Because the saga data is tightly coupled to the saga implementation, we include it as an internal class. The `Handle` method is currently empty—we will need to complete that in just a bit.

NOTE: For a more in-depth explanation of the basic saga concepts, check out [NServiceBus sagas: Getting started](/tutorials/nservicebus-sagas/1-getting-started/).

The `OrderId` is what makes the saga unique. We need to show the saga how to identify the unique **correlation property** `OrderId` in the incoming `ShipOrder` message, and how to relate it to the `OrderId` property in the saga data.

To do that, implement the saga base class's `ConfigureHowToFindSaga` class as shown here, or let the compiler generate the method and fill it in:

snippet: Creation-ConfigureHowToFindSaga

You can now delete the `ShipOrderHandler` class that was created in a previous tutorial, since this newly created saga will replace its functionality.

### Calling web services

Recall that while Maple Shipping Service is cheaper, and thus our preferred vendor, they are sometimes unreliable, in which case we want to use Alpine as a backup. In order to accomplish this, we'll send a `ShipWithMaple` command immediately, and request a `ShippingEscalation` timeout in case Maple happens to be unreliable today.

To do that, we'll first need to add a class for the command and for the timeout.

In the **Messages** project, add a class for `ShipWithMaple`:

snippet: ShipWithMapleCommand

When we create a command, we need to create routing information for it so that when we send it, we know where it needs to go. For the purposes of this tutorial, we'll keep all of the activity within the **Shipping** endpoint.

In the **Shipping** project, open the **Program.cs** file and add the last routing entry for `ShipWithMaple`:

snippet: ShipWithMaple-Routing

Next, let's create the `ShippingEscalation` timeout class, which will serve as the alarm clock or reminder service for our saga.

In the **Shipping** project, go to the `ShipOrderWorkflow` and add `ShippingEscalation` as a *nested internal* class.

snippet: ShippingEscalationTimeout

Note that the `ShippingEscalation` timeout class should be **nested inside** the `ShipOrderWorkflow` class and marked as `internal`. It is very tightly coupled ot the `ShipOrderWorkflow`—there's no need to use it anywhere else. It also doesn't need any special interface or contents. A timeout, after all, is just an alarm clock—we get all we need to know just from the type name. Everything else will already exist in the saga's stored data.

NOTE: For more on saga timeouts, check out [NServiceBus sagas: Timeouts](/tutorials/nservicebus-sagas/3-integration/).

Now, in our `ShipOrderWorkflow` class we can implement the `Handle` method as follows:

snippet: HandleShipOrder

We've sent a `ShipWithMaple` command and requested a `ShippingEscalation` timeout of 20 seconds, so that if Maple doesn't respond within that time we can ship with Alpine instead. Now that we've created this command and timeout, we need to do something with them, starting with the command.

### Shipping with Maple

We're going to use a separate message handler to communicate with the Maple web service—or at least, we would in real life. For the purposes of this tutorial, we'll use a fake message handler.

{{NOTE:
**Why not contact the web service directly within the saga?**

While the saga is processing the message, it holds a database lock on your saga data so that if multiple messages from the same saga try to modify the data at the same time. This presents two problems for a web service request. First the web request can't be added to the database transaction, meaning that if a concurrency exception occurs, the web request can't be undone. The second is that the time it takes for the web request to complete hold the saga database transaction open longer, making it even more likely that another message will be processed concurrently, creating more contention.

This is why it's best for a saga to be only a message-driven state machine: a message comes in, decisions are made, and messages go out. Leave all the other processing to external message handlers as shown in this tutorial.
}}

We'll need to create the message handler as well as configure the routing so the saga knows where to send the `ShipWithMaple` command.

Rather than call a real web service, our example will fake it by delaying for a random period of time, and then replying to the `ShipOrderWorkflow` saga with a `ShipmentAcceptedByMaple` message.

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

Another option would have been to publish `ShipmentAcceptedByMaple` as an event, but then we would have needed to include `OrderId` as a property. This makes sense, because while a reply message is only meant for the saga, any endpoint could subscribe to an event, and in that case the event message wouldn't make sense without containing the `OrderId` identifying it.

### Success with Maple

If the Maple service is able to respond quickly enough, our saga will receive the `ShipmentAcceptedByMaple` reply message. Let's update our saga to handle that reply message.

In the **ShipOrderWorkflow** saga class, implement the additional interface `IHandleMessages<ShipmentAcceptedByMaple>` and implement as shown here:

snippet: ShipWithMaple-ShipmentAccepted

In this handler, we record that the shipment was accepted by Maple in our saga data. In order to store that data, we need to update our saga state in the `ShipOrderData` class:

snippet: ShipWithMaple-Data

Right now, a notification that Maple accepted the shipment is logged, and the flag `ShipmentAcceptedByMaple` is set. The saga does nothing else.

In a real world scenario, perhaps another message needs to be sent so that the customer can be notified and a tracking code can be provided, or we can simply end the saga using `MarkAsComplete()`. If we did mark the saga as completed, when the timeout message we requested arrives it will be ignored, since the saga instance is no longer active, and a timeout cannot start a saga.

### Alternative shipping provider

If the Maple integration handler does not respond in time, the timeout message will arrive, and we need to handle it. It's important to remember that this timeout might be triggered either before or after Maple responds, so we need to be able to handle either circumstance. If we haven't heard back from Maple yet, we're going to want to record that we're sending the order to Alpine, because it's still possible for the Maple service to respond late.

So first, let's update our saga data again. Inside the **ShipOrderWorkflow**, update the **ShipOrderData** class to add a `ShipmentOrderSentToAlpine` property:

snippet: ShipWithAlpine-Data

We're also going to need a message to send to the Alpine adapter:

snippet: ShipWithAlpineCommand

And we're going to continue to keep all of this within the **Shipping** service, so let's add routing instructions for this new message type:

snippet: ShipWithAlpine-Routing

Now that we have those bulding blocks, we can return to the **ShipOrderWorkflow** class and implement `IHandleTimeouts<ShipOrderWorkflow.ShippingEscalation>` as shown here:

snippet: ShippingEscalation

If the shipment was not accepted by Maple, the system needs to execute the shipment via Alpine. It's less likely something will go wrong, since the web service is more reliable. But expect that anything can happen and be prepared for this. Therefore we also request another timeout.

Note that this timeout handler also checks `Data.ShipmentOrderSentToAlpine` to see if we have already attempted to send the order to Alpine. This is because the same timeout message type is used twice. We could have created a separate timeout message type, but using a single type allows us to verify several possible scenarios with if/then logic in the same method, making the end-result is easier to read. We'll return to this method later in this tutorial.

### Alpine response

Just like with Maple, we'll need an external handler to mimic contacting Alpine Delivery, and a response message type so that Alpine can deliver its answer back to our saga.

Add this message to your **Messages** project:

snippet: ShipmentAcceptedByAlpineMessage

Then, add this message handler to your **Shipping** project, which should look very similar to the Maple handler from earlier:

snippet: ShipWithAlpineHandler

Just like with the Maple case, we don't need to add any routing for the `ShipmentAcceptedByAlpine` response message. As a reply, NServiceBus will know where to send the message, and it will also include the `SagaId` with the message headers, just as before.

### Success with Alpine

If the package can be shipped via Alpine, our saga will receive a `ShipmentAcceptedByAlpine` reply message. Let's handle that now. First we'll want to add a property to our saga data to show that the order was accepted by Alpine:

snippet: AcceptedByAlpine-Data

Then, in the **ShipOrderWorkflow** class, implement `IHandleMessages<ShipmentAcceptedByAlpine>` as follows:

snippet: ShipmentAcceptedByAlpine

In the same way as with Maple, we're not currently taking any action once the package is successfully shipped by Alpine. It might be appropriate to publish an event such as `ShipmentAccepted` at this point and then to call `MarkAsComplete()` to complete the saga, deleting it from the database. But first, let's look at some edge cases that could arise.

### Edge cases

So far we've handled two scenarios:

1. We request shipment via Maple and it responds in time that the shipment was accepted.
2. We request shipment via Maple, it doesn't reply in time and we request another shipment via Alpine.

But what happens if *none* of the shipping providers accepts the shipment? In that case, the saga will be stalled and our package will never ship. When we sent the request to Alpine (the second choice) we did request another `ShippingEscalation` timeout, but when that timeout comes due the same `Timeout()` method is executed again, and this line of code will be executed:

snippet: EdgeCases-IfShipmentAccepted

In this case the shipment was not accepted by Maple, but we *already sent the request to Alpine*, and now we have returned to the timeout handler. It means that both attempts to request delivery have failed. That could happen for a number of reasions, such as the Alpine webservice being down for maintenance, or perhaps a network issue prevented our request from being received.

One way to handle this situation is to notify the sales department who can handle the issue manually by calling either shipment provider and request delivery.

The best way to notify the sales department is via an event. Add `ShipmentFailed` to your **Messages** project:

snippet: ShipmentFailedEvent

Then, in the **ShipOrderWorkflow**, modify the `Timeout` method to handle the new case:

snippet: EdgeCases-ShipmentFailed

The saga does not send any emails or save information about the failure to a database. Remember, it's a message-driven state machine! It merely publishes an event, so another handler can take the appropriate action. Our saga is only orchestrating the process.

There could be more scenarios similar to the one mentioned above. As developers we're tempted to solve such problems via code, ensuring consistency and avoiding race conditions. However, in reality deciding how to handle such edge cases is a business decision.

For example, consider that as the saga currently stands, it's possible for Maple to accept a shipment, but too late to stop the saga from requesting shipment via Alpine. In that circumstance, both providers could attempt to ship the package!

Dealing with these sorts of edge cases is not necessarily a technical decision, but a business one. Perhaps generating a shipment record that is never fulfilled is an acceptable solution. Perhaps once orders are accepted by Alpine, a `CancelShipment` command needs to be sent to Maple to ensure no shipment is created. Perhaps the commands to the shipment providers need to include a `DoNotProcessAfter` property so that messages that arrive "too late" are discarded. It depends on the exact business requirements.

In software, timeframes between business decisions can scale down to the millisecond, leading to apparent race conditions. But in real life, [race conditions don't exist](https://udidahan.com/2010/08/31/race-conditions-dont-exist/). It's important to ask business stakeholders what would happen in real life if the events had happened by phone rather than milliseconds apart, and use that to guide your workflows. Be careful and ensure you discuss such edge cases with business stakeholders before you jump straight to the implementation.

## Summary

In this lesson, we learned about commander sagas that execute several steps within a business process. Sagas orchestrate and delegate the work to other handlers. The reason for delegation is to adhere to the Single Responsibility Principle and to avoid potential contention. We've also taken another look at timeouts. And finally we've seen how different scenarios in our business process can be modeled and implemented using sagas.