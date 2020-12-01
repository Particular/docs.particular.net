---
title: Getting Started
summary: Quick list of instructions and hints to get started with NServiceBus and the Particular Service Platform
reviewed: 2019-11-11
suppressRelated: true
---

NServiceBus makes it quick and easy to send, process, and publish messages across a wide variety of on-premises and cloud-based queuing technologies. With all the low-level serialization, threading, and transaction management handled out-of-the box, just grab the NuGet package and go.

NOTE: For more resources to get started, follow our [learning path](https://particular.net/learn/getting-started).


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
    public async Task Handle(SubmitOrder message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Order received {message.OrderId}");

        await context.Publish(new OrderAccepted { OrderId = message.OrderId });
    }
}
```


## See it in action

Jump into our [Quick Start](/tutorials/quickstart/) and build your first end-to-end NServiceBus solution in just 15 minutes. Covering all the elements of one-way messaging, publish-subscribe, and automatic recovery from exceptions, you'll know just enough to be dangerous.

<div class="text-center inline-download"><a href="/tutorials/quickstart/" class="btn btn-primary btn-lg">Let's go! <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span></a>
</div>

## Other resources

* [Tutorials](/tutorials/) - Step-by-step guides to help master NServiceBus and the Particular Service Platform
* [Samples](/samples/) - Learn by exploring code in fully functional solutions that can be run from Visual Studio. Go to the [introduction to samples](/samples/) or [skip to the list of samples](/samples/#related-samples) 
* [High-level content](/get-started/high-level-content.md) - Recommended articles to gain an understanding of the key concepts and technologies used within the Particular Service Platform.


## Getting Help

 * [Particular Software Support](https://particular.net/support)
 * [Licensing/Sales information](https://particular.net/licensing)
 * [Contact Particular Software](https://particular.net/contactus)
 * [Get help with a Proof-Of-Concept](https://particular.net/proof-of-concept)
 * [Community Discussion Group](https://discuss.particular.net)
