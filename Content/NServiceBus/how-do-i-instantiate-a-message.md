---
title: How to Instantiate a Message?
summary: Two options for instantiating a message.
originalUrl: http://www.particular.net/articles/how-do-i-instantiate-a-message
tags: []
createdDate: 2013-05-22T05:02:12Z
modifiedDate: 2013-11-21T07:46:48Z
authors: []
reviewers: []
contributors: []
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




