using System;

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
}