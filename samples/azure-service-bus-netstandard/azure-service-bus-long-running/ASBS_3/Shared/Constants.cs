using System;

public class Constants
{
    public const string TableName = "Requests";
    public static TimeSpan PollingFrequency = TimeSpan.FromSeconds(5);
    public const string PartitionKey = "LongRunningRequest";
    public static TimeSpan EstimatedProcessingTime = TimeSpan.FromMinutes(5);
}