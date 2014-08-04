---
title: Handler Ordering
summary: How to Specify the Order in which Handlers Are Invoked.
tags: []
---

If you are writing your own host:

    NServiceBus.Configure.With()
         ...
         .UnicastBus()
              .LoadMessageHandlers(First<H1>.Then<H2>().AndThen<H3>().AndThen<H4>())
         ...
Where H1-H4 are message handlers.

If you are using the generic host:

    public class EndpointConfig : ISpecifyMessageHandlerOrdering
    {
         public void SpecifyOrder(Order order)
         {
              order.Specify(First<H1>.Then<H2>().AndThen<H3>().AndThen<H4>());
         }
    }

If you only want to specify a single handler (with your own host):

    NServiceBus.Configure.With()
         ...
         .UnicastBus()
              .LoadMessageHandlers>()
         ...

If you only want to specify a single handler (with the generic host):

    public class EndpointConfig : ISpecifyMessageHandlerOrdering
    {
         public void SpecifyOrder(Order order)
         {
              order.Specify>();
         }
    }

