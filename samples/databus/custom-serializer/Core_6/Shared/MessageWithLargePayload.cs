using System;
using NServiceBus;

#region MessageWithLargePayload

//the data bus is allowed to clean up transmitted properties older than the TTBR
[TimeToBeReceived("00:01:00")]
public class MessageWithLargePayload :
    ICommand
{
    public string SomeProperty { get; set; }
    public DataBusProperty<Measurement[]> LargeData { get; set; }
}

#endregion

public class Measurement
{
    public DateTimeOffset Timestamp { get; set; }
    public string MeasurementName { get; set; }
    public decimal MeasurementValue { get; set; }
}
