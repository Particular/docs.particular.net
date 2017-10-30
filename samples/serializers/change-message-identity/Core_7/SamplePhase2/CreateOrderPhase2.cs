using System;
using NServiceBus;
#region messagev2

public class CreateOrderPhase2 :
    IMessage
{
    public DateTime OrderDate { get; set; }
}

#endregion