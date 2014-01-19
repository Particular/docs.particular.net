---
title: NServiceBus Step by Step Guide - Publish Subscribe Communication - code first
summary: See how NServiceBus messaging can get past all sorts of failure scenarios.
originalUrl: http://www.particular.net/articles/nservicebus-step-by-step-publish-subscribe-communication-code-first
tags:
- pub sub
- publish subscribe
createdDate: 2013-10-24T15:50:35Z
modifiedDate: 2014-01-17T19:33:34Z
authors: []
reviewers: []
contributors: []
---

In the previous section: [NServiceBus Step by Step Guide - Fault Tolerance - code first](NServiceBus-Step-by-Step-Guide-fault-tolerance-code-first.md) we learnt about fault tolerance.

1.  [Creating an event message](#CreatingEvent)
2.  [Publishing the event](#Publishing)
3.  [Creating the Subscriber project](#Subscriber)
4.  [Handling the event](#Handeling)
5.  [Running the solution](#Running)
6.  [Next Steps](#Next)

The complete solution code can be found
[here](https://github.com/Particular/docs.particular.net/tree/master/Samples/003_OrderingPubSub)

Now that we've gone through the basics of NServiceBus communication, configuration and fault tolerance, let's move on to publish/subscribe.


[![](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding_pubsub/001_pubsub.png)](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding_pubsub/001_pubsub.png)

<a id="CreatingEvent" name="CreatingEvent"> </a>

Creating an event message
-------------------------

There are only a few steps needed to introduce pub/sub and make your solution look like the one appearing above.


Right click your Messages Project and add a class file, and create a OrderCreated event:


[![](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding_pubsub/002_pubsub.png)](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding_pubsub/002_pubsub.png)

The message class will implement the IEvent marker interface



```C#
namespace Ordering.Messages
{
    using System;
    using NServiceBus;

    public class OrderPlaceed : IEvent
    {
        public Guid OrderId { get; set; }
    }
}

```



<a id="Publishing" name="Publishing"> </a>

Publishing an event
-------------------


In order to publish the 'OrderCreated' event we will modify the
'PlaceOrderHandler', add a
<span style="font-family:courier new,courier,monospace;">Bus.Publish<placeorderhandler>()</span> as shown below



```C#
namespace Ordering.Server
{
    using System;
    using Messages;
    using NServiceBus;

    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        public IBus Bus { get; set; }

        public void Handle(PlaceOrder message)
        {
            Console.WriteLine(@"Order for Product:{0} placed with id: {1}", message.Product, message.Id);

            // throw new Exception("Uh oh - something went wrong....");

            Console.WriteLine(@"Publishing: OrderPlaceed for Order Id: {0}", message.Id);

            Bus.Publish<OrderPlaced>(e => {e.OrderId = message.Id;});
        }
    }
}
```




As the 'Ordering.Server' endpoint is now a publisher, we need to change the endpointConfig.cs file to implement the AsAPublisher marker interface that will configure the endpoint with a Publisher profile



```C#
namespace Ordering.Server
{
    using NServiceBus;

    /*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher
    {
    }
}
```




To learn more about profiles go check out: [Profiles For NServiceBus Host](profiles-for-nservicebus-host.md)


<a id="Subscriber" name="Subscriber"> </a>

Creating the Subscriber project
-------------------------------

Now we can go ahead and create a subscriber endpoint that will subscribe and handle the 'OrderCreated' event.

Right click the Ordering solution and select 'Add' \> 'New Project...'


[![](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding_pubsub/003_pubsub.png)](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding_pubsub/003_pubsub.png)

Create a class library project and name the project Subscriber.


[![](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding_pubsub/004_pubsub.png)](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding_pubsub/004_pubsub.png)

We will use nuget to install the an NServiceBus.Host, in the package manager window and type

<div class="nuget-badge">
`Install-Package NServiceBus.Host -ProjectName Subscriber`{style="background-color: rgb(32, 32, 32); border: 4px solid rgb(192, 192, 192); border-top-left-radius: 5px; border-top-right-radius: 5px; border-bottom-right-radius: 5px; border-bottom-left-radius: 5px; box-shadow: rgb(110, 110, 110) 2px 2px 3px; color: rgb(226, 226, 226); display: block; font-size: 1.2em; font-family: 'andale mono', 'lucida console', monospace; line-height: 1.2em; overflow: auto; padding: 1px;"}




Click reload all


[![](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding_pubsub/005_pubsub.png)](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding_pubsub/005_pubsub.png)

<a id="Handeling" name="Handeling"> </a>

Handling the event
------------------

In our new Subscriber project

-   Add a new class file, name it 'OrderCreatedHandler'
-   Add a reference to the 'messages' project
-   Implement the
    <span style="font-family:courier new,courier,monospace;">IHandle<ordercreated></span>
    interface
-   Add an IBus auto property and implement the handler as shown below






```C#
namespace Ordering.Subscriber
{
    using System;
    using Messages;
    using NServiceBus;

    public class OrderCreatedHandler : IHandleMessages<OrderPlaced>
    {
        public IBus Bus { get; set; }

        public void Handle(OrderPlaced message)
        {
            Console.WriteLine(@"Handling: OrderPlaceed for Order Id: {0}", message.OrderId);
        }
    }
}

```




For the Host will auto subscribe to the event we need to add the message publisher



In the Odering.Subscriber project we will add MessageEndpointMappings in the app.config file as shown below:



```XML
  <UnicastBusConfig>
    <MessageEndpointMappings>
      <add Messages="Ordering.Messages" Type="Ordering.Messages.OrderPlaced" Endpoint="Ordering.Server" />
    </MessageEndpointMappings>
  </UnicastBusConfig>
```



<a id="Running" name="Running"> </a>

Running the solution
--------------------

Now it's time to run the solution and see it all working together we will run the Client, Server and the Subscriber projects:

Right click on the 'Ordering' solution and select 'Set StartUp Projects...'


[![](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding_pubsub/006_pubsub.png)](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding_pubsub/006_pubsub.png)

in that screen select 'Multiple startup projects' and set the
'Ordering.Client', 'Ordering.Server' and 'Ordering.Subscriber' action to be 'Start'.


[![](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding_pubsub/007_pubsub.png)](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding_pubsub/007_pubsub.png)

Finally click 'F5' to run the solution.

Three console application windows should start up

Notice the Subscriber is subscribing the Ordering.Messages.OrderPlaced


[![](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding_pubsub/008_pubsub.png)](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding_pubsub/008_pubsub.png)

Hit enter (while the Client console is in focus) and you should see
'Order for Product: New shoes placed' in one of them.


[![](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding_pubsub/009_pubsub.png)](https://liveparticularwebstr.blob.core.windows.net/media/Default/images/documentation/GettingStartedCoding_pubsub/009_pubsub.png)

And there you are: publish/subscribe messaging is working!

As you see, it's very easy to get started with NServiceBus. You're all set now and can build your own distributed systems with NServiceBus.

<a id="Next" name="Next"> </a>

Next step
---------

-   Read about [NServiceBus and SOA Architectural
    Principles](architectural-principles.md)
-   Try our [Hands on Labs](http://particular.net/HandsOnLabs)
-   Check out our [Videos and
    Presentations](http://particular.net/Videos-and-Presentations)
-   See the
    [Documentation](http://particular.net/documentation/NServiceBus)
-   Join our [community](http://particular.net/DiscussionGroup)


