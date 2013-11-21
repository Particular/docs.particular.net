---
title: 'How to Reply to a Message? '
summary: Use the Reply method on IBus.
originalUrl: http://www.particular.net/articles/how-do-i-reply-to-a-message
tags: []
---

The simplest way to reply to a message is using the Reply method on IBus, like this:

    public class H1 : IMessageHandler
    {
         public IBus Bus { get; set; }

         public void Handle(MyMessage message)
         {
              Bus.Reply(new SomeResponse());
              // can call reply multiple times
              // useful for "streaming" back a lot of data
         }
    }

Underneath the covers, this method calls

    Bus.Send(Bus.CurrentMessageContext.ReturnAddress, Bus.CurrentMessageContext.Id, responseMessage);

If the process that sent the message does not have its own queue, ReturnAddress is null, which means that you cannot reply to the message.

