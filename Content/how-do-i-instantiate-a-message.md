---
layout:
title: "How to Instantiate a Message?"
tags: 
origin: http://www.particular.net/Articles/how-do-i-instantiate-a-message
---
    var msg = new MyMessage();

OR if your message is an interface:

    var msg = Bus.CreateInstance();



