using System.Collections.Generic;
using NServiceBus;

public class MySagaData : ContainSagaData
{
    public string MyCorrelationID { get; set; }
    public List<int> MySequenceNumbers { get; set; } = [];
}