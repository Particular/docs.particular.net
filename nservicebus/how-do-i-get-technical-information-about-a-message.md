---
title: How to Get Technical Information about a Message?
summary: Access the ID of the message as it is transmitted on the wire, the input queue of the sending process, and the headers provided with the message.
tags: []
---

The `CurrentMessageContext` property of `IBus` provides technical information about the message that is currently being processed. You can access the ID of the message as it is transmitted on the wire, the input queue of the sending process, and the headers provided with the message:

    public class H1 : IMessageHandler
    {
         public IBus Bus { get; set; }

         public void Handle(MyMessage message)
         {
              // use Bus.CurrentMessageContext
              // contains ID, ReturnAddress, and Headers
         }
    }

You can also manipulate header information using the `GetHeader` and `SetHeader` extension methods on `IMessage` without referencing `IBus` at all, like this:

    public class H1 : IMessageHandler
    {
         public void Handle(MyMessage message)
         {
              string someValue = message.GetHeader("someKey");
              message.SetHeader("someOtherKey", "someOtherValue");
         }
    }

