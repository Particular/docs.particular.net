---
title: How to Instantiate a Message?
summary: Two options for instantiating a message.
originalUrl: http://www.particular.net/articles/how-do-i-instantiate-a-message
tags: []
---

<div class="brush:csharp;"> If the message is a class


    var msg = new MyMessage();

OR if your message is an interface:

    var msg = Bus.CreateInstance();



