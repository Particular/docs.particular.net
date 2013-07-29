---
layout:
title: "How to Publish/Subscribe to a Message"
tags: 
origin: http://www.particular.net/Articles/sub-with-NServiceBus
---
How to publish a message?
-------------------------

    Bus.Publish(messageObject);

OR instantiate and publish all at once:

    Bus.Publish( m => { m.Prop1 = v1; m.Prop2 = v2; });

How to subscribe to a message?
------------------------------

To manually subscribe and unsubscribe from a message:

    Bus.Subscribe();    

OR

    Bus.Unsubscribe();    

To subscribe to a message, you must have a UnicastBusConfig entry, as follows:






When subscribing to a message, you will probably have a [message handler](how-do-i-handle-a-message) for it. If you do, and have the UnicastBusConfig section mentioned above, you do not have to write Bus.Subscribe, as NServiceBus invokes it automatically for you.

You can also choose to **not** have the infrastructure automatically subscribe by calling .DoNotAutoSubscribe() after .UnicastBus() in the Fluent configuration API.

