---
title: Using IBus in a Message Handler?
summary: Use setter injection or constructor injection.
tags: []
redirects:
 - nservicebus/how-do-i-get-a-reference-to-ibus-in-my-message-handler
---

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
