<!--
title: "test"
tags: ""
summary: "<p>test</p>
<p>~~~~ {.brush:csharp;collapse:true;} using System; using NServiceBus;</p>
"
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



