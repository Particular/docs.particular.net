<!--
title: "How to Get a Reference to IBus in My Message Handler?"
tags: ""
summary: "Use setter injection:"
-->

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

