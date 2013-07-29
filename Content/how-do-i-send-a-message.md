---
layout:
title: "How to Send a Message?"
tags: 
origin: http://www.particular.net/Articles/how-do-i-send-a-message
---
    us.Send(messageObject);

OR instantiate and send all at once:

    Bus.Send( m => { m.Prop1 = v1; m.Prop2 = v2; }); 

