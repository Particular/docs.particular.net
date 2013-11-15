<!--
title: "test"
tags: ""
summary: "test"
-->

test

~~~~ {.brush:csharp;collapse:true;} using System; using NServiceBus;

namespace Messages
{
    [Serializable]
    public class Command : IMessage
    {
        public int Id { get; set; }
    }
}



