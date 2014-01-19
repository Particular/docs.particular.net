---
title: 'How to Reply to a Message? '
summary: Answer a message using the Reply method on IBus.
originalUrl: http://www.particular.net/articles/how-do-i-reply-to-a-message
tags: []
createdDate: 2013-05-22T08:05:31Z
modifiedDate: 2013-12-05T05:49:28Z
authors: []
reviewers: []
contributors: []
---

The simplest way to reply to a message is using the Reply method on IBus, like this:


```C#
public void Handle(RequestDataMessage message)
        {
            //Create a response message
            var response = Bus.CreateInstance<DataResponseMessage>(m => 
            { 
                m.DataId = message.DataId;
                m.String = message.String;
            });
            
            Bus.Reply(response); 
            //Try experimenting with sending multiple responses
            //Bus.Reply(response); 
            //Underneath the covers, this method sends a new message to the return address on the message being handled.
            //Bus.Send(Bus.CurrentMessageContext.ReturnAddress, Bus.CurrentMessageContext.Id, responseMessage);
        }
```

 You should only use a reply when you implement the Request-Response pattern (also called the Full Duplex pattern). In this pattern the originator of the message should expect a response message and have a handler for it. See the [Full Duplex Sample](http://www.particular.net/articles/full-duplex-sample-v3) provided with the install and the article [How to Handle Responses on the Client Side](how-do-i-handle-responses-on-the-client-side.md) for examples and more information.

When using the Publish-Subscribe pattern an endpoint handling an event shouldn't use Bus.Reply. This is because the publisher will not be expecting any reply and should not have a handler for it.

