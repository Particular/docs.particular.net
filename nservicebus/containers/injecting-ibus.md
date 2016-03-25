---
title: Using IBus in a Message Handler
summary: Use setter injection or constructor injection.
redirects:
 - nservicebus/how-do-i-get-a-reference-to-ibus-in-my-message-handler
---

NOTE: In Version 6, the `IBus` interface has been deprecated and removed. Use the `IMessageHandlerContext` interface within a handler, or the `IEndpointInstance`/`IMessageSession` interface instead see example [Handle Message](/nservicebus/handlers).

Use constructor injection:

    public class MyHandler : IHandleMessages<MyMessage>
    {
         IBus bus;
         public H1(IBus bus)
         {
              this.bus = bus;
         }

         public void Handle(MyMessage message)
         {
              // use bus for something
         }
    }


Use setter injection:

    public class MyHandler : IHandleMessages<MyMessage>
    {
         public IBus Bus { get; set; }

         public void Handle(MyMessage message)
         {
              // use Bus for something
         }
    }
