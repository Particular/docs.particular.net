using System;
using System.Runtime.Serialization;

public class PartitionMappingFailedException : Exception
{
    public PartitionMappingFailedException()
    {
    }

    public PartitionMappingFailedException(string message) : base(message)
    {
    }

    public PartitionMappingFailedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected PartitionMappingFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}