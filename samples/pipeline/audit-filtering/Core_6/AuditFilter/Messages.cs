﻿using NServiceBus;

public class AuditThisMessage : IMessage
{
    public string Content { get; set; }
}

public class DoNotAuditThisMessage : IMessage
{
    public string Content { get; set; }
}