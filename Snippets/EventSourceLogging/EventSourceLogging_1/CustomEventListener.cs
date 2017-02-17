using System;
using System.Diagnostics.Tracing;
using System.Linq;

#region EventListener
class CustomEventListener :
    EventListener
{
    protected override void OnEventWritten(EventWrittenEventArgs data)
    {
        var message = string.Format(data.Message, data.Payload?.ToArray() ?? new object[0]);
        Console.WriteLine($"{data.EventId} {data.Channel} {data.Level} {message}");
    }
}
#endregion
