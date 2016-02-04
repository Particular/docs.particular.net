﻿#region sqlsubscriber-handler

using System;
using System.Threading.Tasks;
using NServiceBus;
using Shared;

class SomethingHappenedHandler : IHandleMessages<SomethingHappened>
{
    public async Task Handle(SomethingHappened message, IMessageHandlerContext context)
    {
        Console.WriteLine("Sql Relay has now received this event from the MsmqToSqlRelay. This was originally published by MSMQ publisher. ");

        // You can now relay this event to other interested SQL subscribers
        await context.Publish(message);
    }
}

#endregion