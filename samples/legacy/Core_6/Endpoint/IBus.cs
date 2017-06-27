using System.Collections.Generic;

/// <summary>
/// Represents the legacy IBus
/// </summary>
interface IBus
{
    void SendLocal(object message);
    IReadOnlyDictionary<string, string> Headers { get; }
    string MessageId { get; }
}