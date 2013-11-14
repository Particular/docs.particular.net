<!--
title: "How to Specify Store and Forward for a Message?"
tags: ""
summary: "<h1>How to specify not writing a message to disk?</h1>
<p>This will make the message vulnerable to server crashes or restarts.</p>
"
-->

How to specify not writing a message to disk?
=============================================

This will make the message vulnerable to server crashes or restarts.

    [Express]
    public class MyMessage : IMessage { }

OR

    [Express]
    public interface IMyMessage : IMessage { }

