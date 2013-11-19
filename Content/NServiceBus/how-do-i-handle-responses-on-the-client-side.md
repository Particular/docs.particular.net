---
title: "How to Handle Responses on the Client Side"
tags: ""
summary: "To handle responses on the client, the client (or the sending process) must have its own queue. When messages arrive in this queue, they are handled just like on the server by a message handler:"
---

To handle responses on the client, the client (or the sending process) must have its own queue. When messages arrive in this queue, they are handled just like on the server by a message handler:

    public class H1 : IHandleMessages
    {
         public void Handle(MyMessage message)
         {
              // do something in the client process
         }
    }

You will probably need to access other objects in the client process in this handler. Use [dependency injection](containers.md), declaring a property or constructor argument of the necessary type in your message handler. Then, register the relevant object in the container (most likely as a singleton).

Handling responses in the context of the request
------------------------------------------------

When sending a message/request, you can register a callback that is invoked when a response arrives. This callback is invoked before any message handler as it is registered like this:

    Bus.Send(request).Register(asyncCallback, state);

If the server process returns multiple responses, NServiceBus cannot know which response message will be the last. To prevent memory leaks, the callback is invoked only for the first response. Callbacks won't survive a crash as they are held in memory, so they are less suitable for server-side development where fault-tolerance is required. In those cases, [sagas are preferred](sagas-in-nservicebus.md).

If your client is a web application, use the RegisterWebCallback method.

