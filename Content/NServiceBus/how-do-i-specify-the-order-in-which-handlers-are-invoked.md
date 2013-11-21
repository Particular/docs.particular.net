---
title: How to Specify the Order in which Handlers Are Invoked?
summary: Writing your own host? Using generic host? Specifying a single handler?
originalUrl: http://www.particular.net/articles/how-do-i-specify-the-order-in-which-handlers-are-invoked
tags: []
---

If you are writing your own host:

    NServiceBus.Configure.With()
         ...
         .UnicastBus()
              .LoadMessageHandlers(First.Then().AndThen().AndThen())
         ...

If you are using the generic host:

    public class EndpointConfig : ISpecifyMessageHandlerOrdering
    {
         public void SpecifyOrder(Order order)
         {
              order.Specify(First.Then().AndThen().AndThen());
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

