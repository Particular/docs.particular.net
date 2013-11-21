---
title: How to Get a Reference to IBus in My Message Handler?
summary: Use setter injection or constructor injection.
originalUrl: http://www.particular.net/articles/how-do-i-get-a-reference-to-ibus-in-my-message-handler
tags: []
---

Use setter injection:

    public class H1 : IMessageHandler
    {
         public IBus Bus { get; set; }

         public void Handle(MyMessage message)
         {
              // use Bus for something
         }
    }

Use constructor injection:

    public class H1 : IMessageHandler
    {
         private IBus _bus;
         public H1(IBus bus)
         {
              _bus = bus;
         }

         public void Handle(MyMessage message)
         {
              // use _bus for something
         }
    }

