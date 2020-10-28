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

snippet: ShipOrderWorkflowShipOrder

This creates a saga that is started by `ShipOrder` messages and uses `ShipOrderData` to store its data. Because the saga data is tightly coupled to the saga implementation, we include it as an internal class. The `Handle` method is currently empty—we will need to complete that in just a bit.

NOTE: For a more in-depth explanation of the basic saga concepts, check out [NServiceBus sagas: Getting started](/tutorials/nservicebus-sagas/1-getting-started/).

The `OrderId` is what makes the saga unique. We need to show the saga how to identify the unique **correlation property** `OrderId` in the incoming `ShipOrder` message, and how to relate it to the `OrderId` property in the saga data.

To do that, implement the saga base class's `ConfigureHowToFindSaga` class as shown here, or let the compiler generate the method and fill it in:

snippet: ShipOrderWorkflowMapper

You can now delete the `ShipOrderHandler` class that was created in a previous tutorial, since this newly created saga will replace its functionality.

### Calling web services

Recall that while Maple Shipping Service is cheaper, and thus our preferred vendor, they are sometimes unreliable, in which case we want to use Alpine as a backup. In order to accomplish this, we'll send a `ShipWithMaple` command immediately, and request a `ShippingEscalation` timeout in case Maple happens to be unreliable today.

To do that, we'll first need to add a class for the command and for the timeout.

In the **Messages** project, add a class for `ShipWithMaple`:

snippet: ShipWithMapleCommand

Then, back in the **Shipping** project, add `ShippingEscalation` as an internal class of `ShipOrderWorkflow`:

snippet: ShippingEscalationTimeout

The `ShippingEscalation` class doesn't need any special interface or contents. A timeout, after all, is just a fancy alarm clock—we get all we need to know just from the type name. Everything else will already exist in the saga's stored data. We create it as an `internal` class because it is very tightly coupled to the implementation of the saga. There's no need to use it anywhere else.

NOTE: For more on saga timeouts, check out [NServiceBus sagas: Timeouts](/tutorials/nservicebus-sagas/3-integration/).

Now, in our `ShipOrderWorkflow` class we can implement the `Handle` method as follows:

snippet: HandleShipOrder

DAVID LEFT OFF HERE

NOTE: **Why not contact the web service directly within the saga?** Well, …TODO

As mentioned before, the integration to the Maple web service is delegated to a separate handler. The `ShipWithMapleHandler` is already included in the project, so you only need to send a message to it.


Another thing that needs to be done is to issue a timeout, to be able to fallback to Alpine, if Maple does not respond in a timely manner. For testing purposes the system will wait only 20 seconds for a response.



Have a look at the integration handler `ShipWithMapleHandler`. As you can see it emulates the fact that it might take a while for the webservice to respond. If everything goes well the `Reply()` method is used and the message `ShipmentAcceptedByMaple` will be received by your saga.

snippet: ShipmentAcceptedByMaple

To be able to use the `ShipmentAcceptedByMaple` flag, it needs to be added to the saga state class.

snippet: ShipmentAcceptedByMapleData

Right now, a notification that Maple accepted the shipment is logged, and the flag `ShipmentAcceptedByMaple` is set. The saga does nothing else.

In a real world scenario, perhaps another message needs to be send so that the customer can be notified and a tracking code can be provided, or we can simply end the saga using `MarkAsComplete()`. If the saga is marked as completed, when the timeout message arrives it will be simply ignored, since the saga instance is no longer active.

### Alternative shipping provider

If the Maple integration handler does not respond in time, the timeout message will arrive.

snippet: ShippingEscalation

If the shipment was not accepted by Maple, the system needs to execute the shipment via Alpine. It's less likely something will go wrong, since the web service is more reliable. But expect that anything can happen and be prepared for this. Therefore we also request another timeout.

Make sure to add the `ShipmentOrderSentToAlpine` flag to the `ShipOrderData` class, as you did with the `ShipmentAcceptedByMaple` flag.

{{NOTE:

The code also checks with `ShipmentOrderSentToAlpine` if an order Alpine to ship our package wasn't already sent. This is because the same timeout message is used twice. There is also the option to create a separate timmeout message type. Using one single type allows to verify several possible scenarios in the same method making the end-result is easier to read. You will get back to this method later in this lesson.

}}

If the handler `ShipWithAlpineHandler` is able to ship the package using Alpine, it will reply with the `ShipmentAcceptedByAlpine` message. You will need to be able to handle this as well.

snippet: ShipmentAcceptedByAlpine

### Handling edge cases

**//TODO: introduce `business time top be processed` concept by adding to messages a `DoNotProcessAfter` date time property so that we make sure handlers discard messages that for whatever reason are delayed. If the `timeout` is longer than `DoNotProcessAfter` there should not be any edge case and we can adjust the following paragraph. For example: We first try courier A with a `DoNotProcessAfter` of 2 hours and set a timeout for 2 hours and 15 minutes. The edge case we now need to handle is only releated to the reply and not the original request, so the reply can be late and we need to deal wioth the fact that the second courier has already been contacted. We're reducing the failure surface.**

So far we've handled two scenarios

1. We request shipment via Maple and it responds in time that the shipment was accepted.
2. We request shipment via Maple, it doesn't reply in time and we request another shipment via Alpine.

Another possible situation is that none of the shipping providers accepts the shipment and our package will be never shipped. To ensure that never happens, we request the `ShippingEscalation` timeout. When that timeout expires the same `Timeout()` method is executed again, however, this time the following line of code is executed:

snippet: IfShipmentAccepted

In this case the shipment was not accepted by Maple and we sent the request to Alpine, so the condition above will be satisfied. It means that both attempts to request delivery have failed. That could happen for a number of reasions, for example Alpine webservice could be down for maintenance or there was network issue and our request was never received.

One way to handle such situation is to notify the sales department that will handle the issue manually, for example, can call either shipment provider and request delivery:

snippet: ShippingEscalationAlt

In our code saga does not send any emails or saves information about the failure to a database. It merely publishes an event, so another handler can take the appropriate action. Our saga is only orchestrating the process.

There could be more scenarios similar to the one mentioned above. As developers we're tempted to solve such problems via code, ensuring consistency and avoiding race conditions. However, in reality deciding how to handle such edge cases is a business decision.

For example, it's not up to developers to decide if it's acceptable that once in a while customers will receive duplicate packages, if that significantly improves system throughput and allows to handle more orders. This is a business decision that at first sight looks like a technical one, you'll find even more examples in the [Race conditions don't exist](http://udidahan.com/2010/08/31/race-conditions-dont-exist/) blog post. 

Be careful and ensure you discuss such edge cases with business before you jump straight to the implementation.


## Summary

In this lesson, we learned about commander sagas that execute several steps within a business process. Sagas orchestrate and delegate the work to other handlers. The reason for delegation is to adhere to the Single Responsibility Principle and to avoid potential contention. We've also taken another look at timeouts. And finally we've seen how different scenarios in our business process can be modeled and implemented using sagas.

In the next lesson we'll learn about sagas that never end.


----------

Moved stuff (for now) from above the Exercise below

----------

## Commander sagas

**TODO: These 2 sections seem out of place. "Hey by the way, two really weird and random tidbits because we'd rather be confusing than get right to the exercise??**

The __Introduction to NServiceBus__ (**TODO: "introduction to NServiceBus"? Or "messaging basics"?**) tutorial covered how to send messages, configure routing and how the publish/subscribe pattern works. This lesson will focus on orchestrating a more complex business process that needs to call an external web service.

As a good rule of thumb, sagas should be simple process coordinators delegating calling webservices or accessing databases to other classes. That allows to [avoid contention on the saga state](/nservicebus/sagas/#accessing-databases-and-other-resources-from-a-saga). Delegating the work is done by sending commands to other handlers, which is why these type of sagas are called `commander sagas`.


## Replying

When sending a message as a request to another handler, the receiver might reply with a response. With NServiceBus it is very easy to work with these types of messages. Where normally you'd need to configure routing to specify where messages should be send, with replies it's much easier. In fact it is as easy as:

snippet: Replay

Whenever a message is sent using NServiceBus additional [message headers](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-replytoaddress) are appended. As a result you can use the `Reply()` method and NServiceBus will use information from the headers to return this message. The message metadata also includes identifier for that saga instance, so you don't need to map the response message to a saga instance in the `ConfigureHowToFindSaga()` method. NServiceBus will map it automatically.

Another powerful method is the `ReplyToOriginator()` method available to sagas. It will send a message to the endpoint instance that originally started given saga.

snippet: ReplyToOriginator

#### Reply message type

Up until now all our messages were either commands or events, we've been using `ICommand` and `IEvent` interfaces to indicate that. Some messages though, are neither commands nor events. For example, in this exercise we'll have a message `ShipmentAcceptedByMaple` which is a response to a request. We'll mark such messages with the `IMessage` interface.