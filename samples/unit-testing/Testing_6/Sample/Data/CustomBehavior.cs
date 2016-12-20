﻿using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region SampleBehavior
public class CustomBehavior : Behavior<IOutgoingLogicalMessageContext>
{
    public override Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
    {
        if (context.Message.MessageType == typeof(MyResponse))
        {
            context.Headers.Add("custom-header", "custom header value");
        }

        return next();
    }
}
#endregion