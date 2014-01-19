---
title: How to Handle Responses on the Client Side
summary: The client (or sending process) has its own queue. When messages arrive in the queue, they are handled by a message handler.
originalUrl: http://www.particular.net/articles/how-do-i-handle-responses-on-the-client-side
tags: []
createdDate: 2013-05-22T08:07:11Z
modifiedDate: 2014-01-18T09:52:35Z
authors: []
reviewers: []
contributors: []
---

To handle responses on the client, the client (or the sending process) must have its own queue and cannot be configured as a SendOnly endpoint. When messages arrive in this queue, they are handled just like on the server by a message handler:


```C#
public class MyHandler : IHandleMessages<MyMessage>
{
     public void Handle( MyMessage message )
     {
          // do something in the client process
     }
}
```

 You will probably need to access other objects in the client process in this handler. Use [dependency injection](containers.md) , declaring a property or constructor argument of the necessary type in your message handler. Then, register the relevant object in the container (most likely as a singleton).

Handling responses in the context of the request
------------------------------------------------

When sending a message/request, you can register a callback that is invoked when a response arrives. This callback is invoked before any message handler as it is registered like this:


```C#
busInstance.Send( messageInstance ).Register( asyncCallback, state );
```

 If the server process returns multiple responses, NServiceBus cannot know which response message will be the last. To prevent memory leaks, the callback is invoked only for the first response. Callbacks won't survive a crash as they are held in memory, so they are less suitable for server-side development where fault-tolerance is required. In those cases, [sagas are preferred](sagas-in-nservicebus.md) .

If your client is a web application, use the
**RegisterWebCallback**method.

