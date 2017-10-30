using System;
using NServiceBus;
#region messagev1

public class CreateOrderPhase1 :
    IMessage
{
    public DateTime OrderDate { get; set; }
}

#endregion