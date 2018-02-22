---
title: Getting Started
summary: Quick list of instructions and hints to get started with NServiceBus and the Particular Service Platform
reviewed: 2018-02-19
suppressRelated: true
---

The core principles of NServiceBus can be distilled to a simple API. For a deeper look into the API, check out the [Quick Start Tutorial](/tutorials/quickstart/) for a quick look at what NServiceBus can do, or the [Messaging Basics Tutorial](/tutorials/intro-to-nservicebus/) to really dig in and learn how NServiceBus works.


### Installing


```ps
PM> Install-Package NServiceBus
```


### Sending a message

```cs
await endpoint.Send(new SubmitOrder { OrderId = orderId });
```

### Processing a message

```cs
public class OrdersHandler : IHandleMessages<SubmitOrder>
{
    public async Task Handle(SubmitOrder message, IMessageContext context)
    {
        Console.WriteLine($"Order received {message.OrderId}");

        await context.Publish(new OrderAccepted { OrderId = message.OrderId });
    }
}
```


## Other resources

* [High-level content](/get-started/high-level-content.md) - Recommended articles to gain an understanding of the key concepts and technologies used within the Particular Service Platform.
* [Tutorials](/tutorials/) - Step-by-step guides to help master NServiceBus and the Particular Service Platform
* [Samples](/samples/) - Learn by exploring code in fully functional solutions that can be run from Visual Studio. Go to the [introduction to samples](/samples/) or [skip to the list of samples](/samples/#related-samples) 


## Getting Help

 * [Particular Software Support](https://particular.net/support)
 * [Licensing/Sales information](https://particular.net/licensing)
 * [Contact Particular Software](https://particular.net/contactus)
 * [Get help with a Proof-Of-Concept](https://particular.net/proof-of-concept)
 * [Community Discussion Group](https://discuss.particular.net)
