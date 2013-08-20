<!--
title: "How to Handle a Message?"
tags: 
-->
Write a class that implements IMessageHandler<t> where T is the message type:

    public class H1 : IMessageHandler
    {
         public void Handle(MyMessage message)
         {
         }
    }

How do I handle all kinds of messages?
--------------------------------------

Write a handler for IMessage as follows:

    public class H2 : IMessageHandler
    {
         public void Handle(IMessage message)
         {
              // do something relevant for all messages
         }
    }

