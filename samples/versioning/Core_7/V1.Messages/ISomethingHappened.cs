﻿using NServiceBus;
#region V1Message
namespace V1.Messages
{
    public interface ISomethingHappened :
        IEvent
    {
        int SomeData { get; set; }
    }
}
#endregion