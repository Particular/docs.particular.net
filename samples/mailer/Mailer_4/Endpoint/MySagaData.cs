using System;
using NServiceBus;

public class MySagaData :
    ContainSagaData
{
    public Guid Number { get; set; }
}