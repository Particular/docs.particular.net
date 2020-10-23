---
title: "NServiceBus sagas: Integrations"
reviewed: 2020-10-22
isLearningPath: true
summary: "third party services integration."
---

## Introduction to NServiceBus sagas: Third-party integration

**TODO: Include downloadable solution to date**

Sagas designed so far were awaiting for events and timeouts to participate in a business transaction. Those sagas were rather passive. Sagas can also take on a much more active role and execute several steps of a business process.

This lesson focuses on third party services integration as, for example, shipping couriers web services. You'll make the system talk to two external fictional express couriers, called Alpine Delivery and Maple Shipping Services. Maple is currently cheaper, so it's the preferred delivery option. However, it also seems to be less reliable. There is a 24-hour delivery SLA with our customers, if Maple doesn't respond to our shipment request on time, we need to ask Alpite to deliver the package.

In this lesson, you will learn how to orchestrate such a business process using NServiceBus sagas. We'll also see how we can react to failures from one or more of our third-party services.

**TODO: We want to start from the solution at the end of the last lesson so we need an introductory section here to add placeholder handlers for Maple and Alpine**

## Commander sagas

The __Introduction to NServiceBus__ (**TODO: "introduction to NServiceBus"? Or "messaging basics"?**) tutorial covered how to send messages, configure routing and how the publish/subscribe pattern works. This lesson will focus on orchestrating a more complex business process that needs to call an external web service.

As a good rule of thumb, sagas should be simple process coordinators delegating calling webservices or accessing databases to other classes. That allows to [avoid contention on the saga state](/nservicebus/sagas/#accessing-databases-and-other-resources-from-a-saga). Delegating the work is done by sending commands to other handlers, which is why these type of sagas are called `commander sagas`.


## Replying

When sending a message as a request to another handler, the receiver might reply with a response. With NServiceBus it is very easy to work with these types of messages. Where normally you'd need to configure routing to specify where messages should be send, with replies it's much easier. In fact it is as easy as:

```
context.Reply(new ShipmentAcceptedByMaple());
```

Whenever a message is sent using NServiceBus additional [message headers](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-replytoaddress) are appended. As a result you can use the `reply()` method and NServiceBus will use information from the headers to return this message. The message metadata also includes identifier for that saga instance, so you don't need to map the response message to a saga instance in the `ConfigureHowToFindSaga()` method. NServiceBus will map it automatically.

Another powerful method is the `ReplyToOriginator()` method available to sagas. It will send a message to the endpoint instance that originally started given saga.

```
ReplyToOriginator(context, new PackageSuccessfullyShipped());
```

#### Reply message type

Up until now all our messages were either commands or events, we've been using `ICommand` and `IEvent` interfaces to indicate that. Some messages though, are neither commands nor events. For example, in this exercise we'll have a message `ShipmentAcceptedByMaple` which is a response to a request. We'll mark such messages with the `IMessage` interface.


## Exercise

You've seen that the current `ShippingPolicy` in the `Shipping` endpoint is rather passive. To be able to deal with out-of-order delivery of messages, it waits for messages to arrive. After that, the order is ready to be shipped. This lesson continues with the process implementation and executes the shipment.

### Multiple sagas for a process

With the Single Responsibility Principle in mind, this feature should not be added to the current `ShippingPolicy` saga. When this saga is done, it sends the command `ShipOrder` which we can use to create another saga to make this happen. We will call this saga `ShipPackagePolicy` and its responsibility is to orchestrate the shipment via Maple or Alpine. Add this saga to the `Shipping` project.

```c#
class ShipOrderPolicy : 
        Saga<ShipOrderPolicy.ShipOrderData>,
        IAmStartedByMessages<ShipOrder>
{
    public async Task Handle(ShipOrder message, IMessageHandlerContext context)
    {    
    }
    
    internal class ShipOrderData : ContainSagaData
    {
        public string OrderId { get; set; }
    }    
}
```

The saga state is added as an embedded class into our saga `ShipOrderPolicy`. The `OrderId` is what makes the saga unique and to which you'll need to map the incoming `ShipOrder` message:

```
protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShipOrderData> mapper)
{
    mapper.ConfigureMapping<ShipOrder>(m => m.OrderId).ToSaga(s => s.OrderId);
}
```

You can now delete the `ShipOrderHandler` that was created in lesson 1, since this newly created saga will replace its functionalities.

### Calling web services

As mentioned before, the integration to the Maple web service is delegated to a separate handler. The `ShipWithMapleHandler` is already included in the project, so you only need to send a message to it.

Another thing that needs to be done is to issue a timeout, to be able to fallback to Alpine, if Maple does not respond in a timely manner. For testing purposes the system will wait only 20 seconds for a response.

```
public async Task Handle(ShipOrder message, IMessageHandlerContext context)
{
    // Execute order to ship with Maple
    await context.Send(new ShipWithMaple() { OrderId = Data.OrderId}).ConfigureAwait(false);

    // Add timeout to escalate if Maple did not ship in time.
    await RequestTimeout(context, TimeSpan.FromSeconds(20),
                         new ShippingEscalation()).ConfigureAwait(false);
}
```

Have a look at the integration handler `ShipWithMapleHandler`. As you can see it emulates the fact that it might take a while for the webservice to respond. If everything goes well the `Reply()` method is used and the message `ShipmentAcceptedByMaple` will be received by your saga.

```
public Task Handle(ShipmentAcceptedByMaple message, IMessageHandlerContext context)
{
    log.Info($"Order [{Data.OrderId}] - Successfully shipped with Maple");

    Data.ShipmentAcceptedByMaple = true;

    return Task.CompletedTask;
}
```

To be able to use the `ShipmentAcceptedByMaple` flag, it needs to be added to the saga state class.

```
internal class ShipOrderData : ContainSagaData
{
    public string OrderId { get; set; }
    public bool ShipmentAcceptedByMaple { get; set; }
}
```

Right now, a notification that Maple accepted the shipment is logged, and the flag `ShipmentAcceptedByMaple` is set. The saga does nothing else.

In a real world scenario, perhaps another message needs to be send so that the customer can be notified and a tracking code can be provided, or we can simply end the saga using `MarkAsComplete()`. If the saga is marked as completed, when the timeout message arrives it will be simply ignored, since the saga instance is no longer active.

### Alternative shipping provider

If the Maple integration handler does not respond in time, the timeout message will arrive.

```
public async Task Timeout(ShippingEscalation state, IMessageHandlerContext context)
{
    if (!Data.ShipmentAcceptedByMaple && !Data.ShipmentOrderSentToAlpine)
    {
        log.Info($"We didn't receive answer from Maple, let's try Alpine.");
        Data.ShipmentOrderSentToAlpine = true;
        await context.Send(new ShipWithAlpine() { OrderId = Data.OrderId })
            .ConfigureAwait(false);
        await RequestTimeout(context, TimeSpan.FromSeconds(20), new ShippingEscalation())
            .ConfigureAwait(false);
    }
}
```

If the shipment was not accepted by Maple, the system needs to execute the shipment via Alpine. It's less likely something will go wrong, since the web service is more reliable. But expect that anything can happen and be prepared for this. Therefore we also request another timeout.

Make sure to add the `ShipmentOrderSentToAlpine` flag to the `ShipOrderData` class, as you did with the `ShipmentAcceptedByMaple` flag.

{{NOTE:

The code also checks with `ShipmentOrderSentToAlpine` if an order Alpine to ship our package wasn't already sent. This is because the same timeout message is used twice. There is also the option to create a separate timmeout message type. Using one single type allows to verify several possible scenarios in the same method making the end-result is easier to read. You will get back to this method later in this lesson.

}}

If the handler `ShipWithAlpineHandler` is able to ship the package using Alpine, it will reply with the `ShipmentAcceptedByAlpine` message. You will need to be able to handle this as well.

```
public Task Handle(ShipmentAcceptedByAlpine message, IMessageHandlerContext context)
{
    log.Info($"Order [{Data.OrderId}] - Succesfully shipped with Alpine");

    Data.ShipmentAcceptedByAlpine = true;

    return Task.CompletedTask;
}
```

### Handling edge cases

//TODO: introduce `business time top be processed` concept by adding to messages a `DoNotProcessAfter` date time property so that we make sure handlers discard messages that for whatever reason are delayed. If the `timeout` is longer than `DoNotProcessAfter` there should not be any edge case and we can adjust the following paragraph. For example: We first try courier A with a `DoNotProcessAfter` of 2 hours and set a timeout for 2 hours and 15 minutes. The edge case we now need to handle is only releated to the reply and not the original request, so the reply can be late and we need to deal wioth the fact that the second courier has already been contacted. We're reducing the failure surface.

So far we've handled two scenarios

1. We request shipment via Maple and it responds in time that the shipment was accepted.
2. We request shipment via Maple, it doesn't reply in time and we request another shipment via Alpine.

Another possible situation is that none of the shipping providers accepts the shipment and our package will be never shipped. To ensure that never happens, we request the `ShippingEscalation` timeout. When that timeout expires the same `Timeout()` method is executed again, however, this time the following line of code is executed:

```
if (!Data.ShipmentAcceptedByMaple && !Data.ShipmentOrderSentToAlpine)
```

In this case the shipment was not accepted by Maple and we sent the request to Alpine, so the condition above will be satisfied. It means that both attempts to request delivery have failed. That could happen for a number of reasions, for example Alpine webservice could be down for maintenance or there was network issue and our request was never received.

One way to handle such situation is to notify the sales department that will handle the issue manually, for example, can call either shipment provider and request delivery:

```
public async Task Timeout(ShippingEscalation state, IMessageHandlerContext context)
{
    if (!Data.ShipmentAcceptedByMaple && !Data.ShipmentOrderSentToAlpine)
    {
        log.Info($"We didn't receive answer from Maple, let's try Alpine.");
        Data.ShipmentOrderSentToAlpine = true;
        await context.Send(new ShipWithAlpine() { OrderId = Data.OrderId })
           .ConfigureAwait(false);
        await RequestTimeout(context, TimeSpan.FromSeconds(20), new ShippingEscalation())
           .ConfigureAwait(false);
    }

    // No response from Maple nor Alpine
    if (!Data.ShipmentAcceptedByMaple && !Data.ShipmentAcceptedByAlpine)
    {
        log.Warn($"We didn't receive answer from either Maple nor Alpine. We need to escalate!");
        // escalate to Warehouse Manager!
        await context.Publish<ShipmentFailed>().ConfigureAwait(false);
    }
}
```

In our code saga does not send any emails or saves information about the failure to a database. It merely publishes an event, so another handler can take the appropriate action. Our saga is only orchestrating the process.

There could be more scenarios similar to the one mentioned above. As developers we're tempted to solve such problems via code, ensuring consistency and avoiding race conditions. However, in reality deciding how to handle such edge cases is a business decision.

For example, it's not up to developers to decide if it's acceptable that once in a while customers will receive duplicate packages, if that significantly improves system throughput and allows to handle more orders. This is a business decision that at first sight looks like a technical one, you'll find even more examples in the [Race conditions don't exist](http://udidahan.com/2010/08/31/race-conditions-dont-exist/) blog post. 

Be careful and ensure you discuss such edge cases with business before you jump straight to the implementation.


## Summary

In this lesson, we learned about commander sagas that execute several steps within a business process. Sagas orchestrate and delegate the work to other handlers. The reason for delegation is to adhere to the Single Responsibility Principle and to avoid potential contention. We've also taken another look at timeouts. And finally we've seen how different scenarios in our business process can be modeled and implemented using sagas.

In the next lesson we'll learn about sagas that never end.
