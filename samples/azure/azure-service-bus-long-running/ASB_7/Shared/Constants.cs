namespace Shared
{
    using System;

    public class Constants
    {
        public const string TableName = "Requests";
        public const int PollingFrequencyInSeconds = 5;
        public const string PartitionKey = "LongRunningRequest";
        public static TimeSpan EstimatedProcessingTime = TimeSpan.FromMinutes(5);
    }
}