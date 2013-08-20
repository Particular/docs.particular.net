<!--
title: "How to Specify Store and Forward for a Message?"
tags: 
-->
This is the default mode in V2.0 and V2.5, but not in V1.9:

    [Recoverable]
    public class MyMessage : IMessage { }

OR

    [Recoverable]
    public interface IMyMessage : IMessage { }

How to specify not writing a message to disk?
---------------------------------------------

This is the default mode in V1.9, but not in V2.0 and later:

    [Express]
    public class MyMessage : IMessage { }

OR

    [Express]
    public interface IMyMessage : IMessage { }

