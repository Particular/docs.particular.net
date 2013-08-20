<!--
title: "How to Send a Message?"
tags: 
-->
    us.Send(messageObject);

OR instantiate and send all at once:

    Bus.Send( m => { m.Prop1 = v1; m.Prop2 = v2; }); 

