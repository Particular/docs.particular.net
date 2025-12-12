using System.Collections.Generic;
using NServiceBus;

public class MyCommand : ICommand
{
    public string CorrelationID { get; set; }
    public int Sequence { get; set; }
    public List<int> CurrentProcessedNumbers { get; set; }
}