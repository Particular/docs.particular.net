---
title: How to Instantiate a Message?
summary: Two options for instantiating a message.
originalUrl: http://www.particular.net/articles/how-do-i-instantiate-a-message
tags: []
---

If the message is a class:


```C#
var msg = new MyMessage();
```

 OR

If your message is an interface:


```C#
var msg = Bus.CreateInstance<IMyMessage>();
```




