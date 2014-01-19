---
title: How to Get a Reference to IBus in My Message Handler?
summary: Use setter injection or constructor injection.
originalUrl: http://www.particular.net/articles/how-do-i-get-a-reference-to-ibus-in-my-message-handler
tags: []
createdDate: 2013-05-22T05:14:35Z
modifiedDate: 2013-07-29T14:14:34Z
authors: []
reviewers: []
contributors: []
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

