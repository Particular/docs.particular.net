---
title: "NServiceBus 101 Lesson 4: Publishing events"
component: Core
---

So far in this course, we have only sent **commands** – one-way messages from a sender to a specific receiver. There's another type of message we have yet to cover called an **event**. In many ways events are just like commands. They're simple classes and you deal with them in much the same way. But from an architectural standpoint commands and events are *polar opposites*. This creates a useful dichotomy. We can take advantage of the properties of events to open up new possibilities in how we design software.

In this lesson, we'll learn all about publishing events, and more importantly, what that allows us to do.


## Objectives

By the end of this lesson, you will have learned:

* The difference between commands and events
* How the Publish/Subscribe pattern helps to create more maintainable code
* How to define and name events
* How to publish an event
* How to subscribe to an event


## What is an event?

An **event** is another type of message that is published to multiple receivers, unlike a command which is sent to one receiver. Let's take a look at the formal definitions for commands and events:

{{INFO:
A **command** is a message that can be sent from one or more senders and is processed by a single receiver.

An **event** is a message that is published from a single sender, and is processed by (potentially) many receivers.
}}

You can see that in many ways, commands and events are exact opposites, and the differences in their definition leads us to different uses for each.

A command can be sent from anywhere, but is processed by one receiver. This is very similar to a web service, or any other [RPC](https://en.wikipedia.org/wiki/Remote_procedure_call)-style service. The big difference is that a one-way message does not have any return value like a web service would have. This means that the handler for the command is doing work for whomever is calling it, and that the sender has a very good idea about what it expects to happen as a result of sending the command. It is the sender saying "Will you please do something for me?" and so commands should be named in the [imperative tense](https://en.wikipedia.org/wiki/Imperative_mood), like `PlaceOrder`, `UpdateCustomerStatus`, and `ChargeCreditCard`. This creates tight coupling between the sender and receiver, because while it is possible to reject a command, you can't have true autonomy if someone else can tell you what to do.

An event, on the other hand, is sent by one sender, and received by many receivers, or maybe one receiver, or even zero receivers.  This makes it an announcement that something has already happened. A subscriber can't reject or cancel an event any more than you could stop the New York Times from delivering newspapers to all of their subscribers. The publisher has no idea (and doesn't care) what receivers choose to do with the event; it's just making an announcement. So, events should be named in the [past tense](https://en.wikipedia.org/wiki/Past_tense), commonly ending with the **-ed** suffix, like `OrderPlaced`, `CustomerStatusUpdated`, and `CreditCardCharged`. This creates loose coupling, because while the contract (the content of the message) must be agreed upon, there is no requirement that subscribers of an event do anything.

Let's take a look at all of these differences side-by-side:

|   | Commands | Events |
|---|:--------:|:------:|
| Marker Interface | `ICommand` | `IEvent` |
| Logical Senders | Many | 1 |
| Logical Receivers | 1 | Many (or none) |
| Purpose | "Please do something" | "Something has happened" |
| Naming (Tense) | Imperative | Past |
| Examples | `PlaceOrder`<br/> `UpdateCustomerStatus`<br/>`ChargeCreditCard` | `OrderPlaced`<br/>`CustomerStatusUpdated`<br/>`CreditCardCharged` |
| Coupling Style | Tight | Loose |

From this comparison, it's clear that commands and events will sometimes come in pairs. A command will arrive, perhaps from a website UI, telling the system to `DoSomething`. The system does that work, and as a result, publishes a `SomethingHappened` event, which other components in the system can react to.

The loose coupling provided by publishing events gives us quite a bit of flexibility to design our software systems in a much more maintainable way.


## Better code through decoupling

Have you ever seen a codebase for a naive implementation of an e-commerce store? Commonly you'll find one gargantuan SubmitOrder method responsible for retrieving the shopping cart, creating an Order and OrderLines in the database, authorizing the credit card, capturing the credit card authorization, emailing a receipt, notifying a fulfillment agency, and then updating any sort of wish list, gift registry, or "frequently bought together" information the site might keep track of. If that method doesn't number in the hundreds of lines of code, then it likely calls into subroutine methods to handle each of these concerns. The sum total lines of code in such an example likely numbers in at least hundreds of lines of code, if not thousands.

What happens when one of the steps in that long chain fails? You're left with a partially completed process that requires manual intervention to fix, usually by mucking around manually in the database.

By using events, we can better follow the [single responsibility principle](https://en.wikipedia.org/wiki/Single_responsibility_principle) and divide up these concerns into separate message handlers. Simply publish `OrderPlaced`, and all the other components that subscribe to it will take care of their own concerns.

This means that when the code for the credit card processing changes, we don't even need to touch (let alone test and redeploy) any of the code in the system except for that which is directly related to processing credit cards.


## Defining events

Creating an event message is just as easy as creating an event. We just create a class and mark it with the `IEvent` (rather than `ICommand`) marker interface.

snippet:Event

All the other considerations for command messages apply to events as well. Properties can be simple types, complex types, or collections - whatever the message serializer supports. However, with events, you should be even more careful to refrain from getting carried away and putting too much information in an event message. Since a publisher does not know (or care) how many subscribers it has, it may not be possible to modify all of them if a change is required.


## Handling events

Handling an event is the exact same as handling a command. Just create a handler class by implementing `IHandleMessages<T>` where `T` is the type of the event message.

snippet:EventHandler


## Subscribing to events

For the MSMQ transport (which we are using in this course) NServiceBus needs to know which endpoint is responsible for publishing an event. This is because the MSMQ transport does not have native publish/subscribe capability but uses **message-driven publish/subscribe**. This means that the subscribing endpoint will send a *subscription request message* to the publisher endpoint, requesting to be added to the subscriber list. The publisher endpoint will store the address of the subscriber (usually in a database, but for now we are still using in-memory persistence) so that when `.Publish()` is called, the list of subscribers can be retrieved and a copy of the event message can be delivered to each one.

You can configure the publisher endpoint via the Routing API like this:

snippet:RegisterPublisher

NOTE: Some other transports have built-in publish/subscribe capabilities, so all that is required to subscribe to an event is to create a message handler for the event. A subscription request message is not required. Because the routing configuration is scoped to the transport type, transports that don't require publisher configuration simply won't contain this API.


## Exercise

Now that we've learned about events and the Publish/Subscribe pattern, let's make use of it in our ordering system. When a user places an order, we're going to publish an OrderPlaced event, and then subscribe to it from two brand new endpoints: Billing and Shipping.

We'll also create a new OrderBilled event that will be published by the Billing endpoint once the credit card transaction is complete.

![Lesson 4 Diagram](https://cloud.githubusercontent.com/assets/427110/16881766/54da5b4e-4a81-11e6-8f08-57fbc9241af8.png)

When the Shipping endpoint receives both the OrderPlaced and OrderBilled, it will know that it is time to ship the product to the customer. How to handle that situation, however, will be the topic for another lesson.


### Create an event

Let's create our first event, `OrderPlaced`:

1. In the **Messages** project, create a new folder called **Events**.
2. In the **Events** folder, add a new class called `OrderPlaced`.
3. Mark `OrderPlaced` as `public` and implement `IEvent`.
4. Add a public property of type `string` named `OrderId`.

When complete, your `OrderPlaced` class should look like the following:

snippet:OrderPlaced

NOTE: Notice that because of our use of folders for **Commands** and **Events**, we now have our commands and events in namespaces called `Messages.Commands` and `Messages.Events`, respectively. In a future course, we will be able to [take advantage of this organization](/nservicebus/messaging/conventions.md) to identify commands and events without needing to use the `ICommand` and `IEvent` interfaces at all!


### Publish an event

Now that the `OrderPlaced` event is defined, we can publish it from the `PlaceOrderHandler`.

1. Locate the `PlaceOrderHandler` within the **Sales** endpoint.
2. Remove the `return Task.CompletedTask;` line.
3. Instead, modify the `Handle` method to look like the following:

snippet:UpdatedHandler

Like `.Send()`, `.Publish()` also returns a `Task`. We could mark this method as `async` and then `await` the return task, but since this is the only `Task`-returning operation we're using, we can just return this `Task` from our handler method.

If we ran the solution now (and you're welcome to do so) nothing new or exciting will happen, at least visibly. We're publishing a message, but there are no subscribers, so no physical messages actually get sent anywhere. We're like a newspaper with no circulation. To fix that, we need a subscriber.


### Create a subscriber

Unlike the command `PlaceOrder`, which is a request to do something, `OrderPlaced` is an announcement that something has actually happened. So who would be interested in hearing about placed orders?

When an order is placed, we will want to charge the credit card for that order. So we will create a **Billing** service, which will subscribe to `OrderPlaced` so that it can handle the payment transaction.

NOTE: Since this is the third endpoint we've created, the instructions will be a little more abbreviated. Feel free to refer back to [Lesson 2](../lesson-2/) where we created the Sales endpoint for more detailed instructions.

1. Create a new **Console Application** named **Billing**.
2. Add references for the **NServiceBus NuGet package** and the **Messages** assembly.
3. Copy the configuration from the **Program.cs** file in **Sales**, and paste it into the same file in **Billing**.
4. In the **Billing** endpoint's **Program.cs**, change the value of `Console.Title` and the endpoint name argument of the `EndpointConfiguration` constructor to `"Billing"`.
5. In the **Billing** endpoint, add a class named `OrderPlacedHandler`, mark it as `public`, and implement `IHandleMessages<OrderPlaced>`.
6. Modify the handler class to log the receipt of the event:

snippet:SubscriberHandler

And finally, modify the solution properties so that **Billing** will start when debugging.


### Subscribe to an event

We now have a handler in place for `OrderPlaced`, but just like in real life, having a mailbox isn't enough to get a newspaper delivered to your house. We need to let the publisher know we want to subscribe to get the message.

In the **Billing** endpoint, locate the **AsyncMain** method in the **Program.cs** file. Modify it to gain access to the Routing API:

snippet:BillingRouting

With the `routing` variable, configure the publisher for `OrderPlaced`:

snippet:OrderPlacedPublisher

Now when we run the solution, we'll see the following output in the **Billing** window:

    INFO  Billing.OrderPlacedHandler Received OrderPlaced, OrderId = 01698293-9da9-4606-8468-2b7f1b86b380 - Charging credit card...

That's great, but why stop there? The whole point of Publish/Subscribe is that we can have *multiple* subscribers.


### Create another subscriber

In a real system, after an order is placed and billed, we would need to ship the products. So let's add another event and two more subscribers. Once the credit card is charged, we'll publish an `OrderBilled` event. Then, we'll create a new endpoint **Shipping** that will subscribe to both events.

This is also a good opportunity to check your understanding. If you can complete these steps without looking back at previous steps or previous lessons, you can be sure you have a good understanding of everything we've covered so far! (Don't worry, you can always check your work against the solution.)

1. In **Messages**, create a new event called `OrderBilled`, implementing `IEvent` and containing a property for the `OrderId`.
2. In **Billing**, publish the `OrderBilled` event at the end of the `OrderPlacedHandler`.
3. Create a new endpoint named **Shipping** with the necessary dependencies. Be sure to set the console title and endpoint name to `"Shipping"`, and configure it to start when debugging.
4. In **Shipping**, create a message handler for `OrderPlaced`.
5. In **Shipping**, create a message handler for `OrderBilled`.
6. Configure **Shipping** to subscribe to `OrderPlaced` from **Sales**. (You may have this already if you copied the endpoint configuration from **Billing**.)
7. Configure **Shipping** to subscribe to `OrderBilled` from **Billing**. (This will be new no matter what.)


### Running the solution

If everything worked, you should now see output like this in your **Shipping** window:

    INFO  Shipping.OrderPlacedHandler Received OrderPlaced, OrderId = 96ee660a-5dd7-4772-9058-863d303ee0aa - Should we ship now?
    INFO  Shipping.OrderBilledHandler Received OrderBilled, OrderId = 96ee660a-5dd7-4772-9058-863d303ee0aa - Should we ship now?

Of course, these messages could appear out of order. With asynchronous messaging, there are no message ordering guarantees. Even though `OrderBilled` comes logically after `OrderPlaced`, it's possible that `OrderBilled` could arrive first!

You'll note that in the sample solution, the message for each handler says "Should we ship now?" This is because both message handlers are stateless. Like HTTP requests, message handlers have no intrinsic memory of what came before. In an upcoming course, we'll explore [Sagas](/nservicebus/sagas/), an NServiceBus feature that provides that memory in-between message handlers, similar to how ASP.NET Session State stores data between HTTP requests in ASP.NET web applications. Once we have that capability, we'll be able to publish an `OrderShipped` event once both the `OrderPlaced` and `OrderBilled` events arrive, but this is good enough for now.


## Summary

In this lesson we learned all about events, how they differ from commands, and how that enables us to create systems that are more decoupled and adhere better to the Single Responsibility Principle. We published an `OrderPlaced` event from the Sales endpoint, and created the Billing and Shipping endpoints to subscribe to that event. We also published the `OrderBilled` event from the Billing endpoint, and subscribed to it in Shipping.

In the final lesson for this course, we'll see what happens when we introduce errors into our system, and see how we can automatically retry those messages to make a truly resilient system.

Before moving on, you might want to check your code against the completed solution (below) to see if there's anything you may have missed.

When you're ready, move on to [**Lesson 5: Retrying errors**](../lesson-5/).
