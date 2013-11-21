---
title: How to Specify Store and Forward for a Message?
summary: Store and Forward is not always the preferred method.
originalUrl: http://www.particular.net/articles/how-do-i-specify-store-forward-for-a-message
tags: []
---

How to specify not writing a message to disk?
=============================================

This will make the message vulnerable to server crashes or restarts.

    [Express]
    public class MyMessage : IMessage { }

OR

    [Express]
    public interface IMyMessage : IMessage { }

