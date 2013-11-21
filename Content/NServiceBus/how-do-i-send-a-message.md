---
title: How to Send a Message?
summary: Send a message, or instantiate and send all at once.
originalUrl: http://www.particular.net/articles/how-do-i-send-a-message
tags: []
---

    us.Send(messageObject);

OR instantiate and send all at once:

    Bus.Send( m => { m.Prop1 = v1; m.Prop2 = v2; }); 

