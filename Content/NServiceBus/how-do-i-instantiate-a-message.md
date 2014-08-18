---
title: How to Instantiate a Message?
summary: Two options for instantiating a message.
tags: []
---

If the message is a class:


```C#
var msg = new MyMessage();
```

 OR

If your event is an interface:


```C#
var msg = Bus.Publish<IMyMessage>();
```




