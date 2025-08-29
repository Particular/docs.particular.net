namespace Messages;

using System;

public class LargeMessage
{
    public Guid RequestId { get; set; }

    public byte[]? LargeClaimCheck { get; set; }
}
