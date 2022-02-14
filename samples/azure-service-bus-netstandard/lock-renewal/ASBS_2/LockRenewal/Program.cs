using System;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Transactions;
using Azure.Messaging.ServiceBus.Administration;
using LockRenewal;
using NServiceBus;

class Program
{

    static readonly TimeSpan LockDuration = TimeSpan.FromMinutes(5);
    static readonly TimeSpan RenewalInterval = TimeSpan.FromMinutes(1.5);
    public static readonly TimeSpan ProcessingDuration = TimeSpan.FromMinutes(11);

    static async Task Main()
    {
        Console.Title = "Samples.ASB.LockRenewal";

        // if(RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework"))
        //     ConfigureTransactionTimeoutNetFramework(TimeSpan.FromHours(1));
        // else
        //     ConfigureTransactionTimeoutCore(TimeSpan.FromHours(1));

        Console.WriteLine("Version                       = {0}", Environment.Version);
        Console.WriteLine("OSVersion                     = {0}", Environment.OSVersion);
        Console.WriteLine("FrameworkDescription          = {0}", RuntimeInformation.FrameworkDescription);
        Console.WriteLine("OSDescription                 = {0}", RuntimeInformation.OSDescription);
        Console.WriteLine("LatencyMode                   = {0}", GCSettings.LatencyMode);
        Console.WriteLine("IsServerGC                    = {0}", GCSettings.IsServerGC);
        Console.WriteLine("LargeObjectHeapCompactionMode = {0}", GCSettings.LargeObjectHeapCompactionMode);
        Console.WriteLine("DefaultTimeout                = {0}", TransactionManager.DefaultTimeout);
        Console.WriteLine("MaximumTimeout                = {0}", TransactionManager.MaximumTimeout);

        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.SendReply.LockRenewal");
        endpointConfiguration.EnableInstallers();

        endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);

        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception(
                "Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
        }

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString(connectionString);
        transport.PrefetchCount(0);

        #region override-lock-renewal-configuration

        endpointConfiguration.LockRenewal(options =>
        {
            options.LockDuration = LockDuration;
            options.RenewalInterval = RenewalInterval;
        });

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

        await OverrideQueueLockDuration("Samples.ASB.SendReply.LockRenewal", connectionString).ConfigureAwait(false);

        if (ProcessingDuration > TransactionManager.MaximumTimeout)
        {
            throw new Exception("MaximumTimeout");
        }

        for (int i = 0; i < 100; i++)
        {
            await endpointInstance.SendLocal(new LongProcessingMessage { ProcessingDuration = ProcessingDuration });
        }

        do
        {
            Console.WriteLine("Press ESCy key to exit");
        }
        while (Console.ReadKey().Key != ConsoleKey.Escape);

        await endpointInstance.Stop().ConfigureAwait(false);
    }

    static async Task OverrideQueueLockDuration(string queuePath, string connectionString)
    {
        var managementClient = new ServiceBusAdministrationClient(connectionString);
        var queueDescription = await managementClient.GetQueueAsync(queuePath).ConfigureAwait(false);
        queueDescription.Value.LockDuration = LockDuration;

        await managementClient.UpdateQueueAsync(queueDescription.Value).ConfigureAwait(false);
    }

    #region override-transaction-manager-timeout-net-core

    static void ConfigureTransactionTimeoutCore(TimeSpan timeout)
    {
        SetTransactionManagerField("s_cachedMaxTimeout", true);
        SetTransactionManagerField("s_maximumTimeout", timeout);

        void SetTransactionManagerField(string fieldName, object value) =>
            typeof(TransactionManager)
                .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static)
                .SetValue(null, value);
    }

    #endregion

    #region override-transaction-manager-timeout-net-framework

    static void ConfigureTransactionTimeoutNetFramework(TimeSpan timeout)
    {
        SetTransactionManagerField("_cachedMaxTimeout", true);
        SetTransactionManagerField("_maximumTimeout", timeout);

        void SetTransactionManagerField(string fieldName, object value) =>
            typeof(TransactionManager)
                .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static)
                .SetValue(null, value);
    }

    #endregion
}


static class DateTimeRoundingExtensions
{
    public static DateTimeOffset RoundUp(this DateTimeOffset instance, TimeSpan period)
    {
        return new DateTimeOffset((instance.Ticks + period.Ticks - 1) / period.Ticks * period.Ticks, instance.Offset);
    }

    public static DateTimeOffset RoundDown(this DateTimeOffset instance, TimeSpan period)
    {
        var delta = instance.Ticks % period.Ticks;
        return new DateTimeOffset(instance.Ticks - delta, instance.Offset);
    }

    public static DateTimeOffset Round(this DateTimeOffset value, TimeSpan period, MidpointRounding style = default)
    {
        if (period <= TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(period), "value must be positive");

        var units = (decimal)value.Ticks / period.Ticks; // Conversion to decimal not to loose precision
        var roundedUnits = Math.Round(units, style);
        var roundedTicks = (long)roundedUnits * period.Ticks;
        var instance = new DateTimeOffset(roundedTicks,value.Offset);
        return instance;
    }

    public static TimeSpan RoundDown(this TimeSpan instance, TimeSpan period)
    {
        var delta = instance.Ticks % period.Ticks;
        return new TimeSpan(instance.Ticks - delta);
    }

    public static TimeSpan RoundUp(this TimeSpan instance, TimeSpan period)
    {
        return new TimeSpan((instance.Ticks + period.Ticks - 1) / period.Ticks * period.Ticks);
    }

    public static TimeSpan Round(this TimeSpan instance, TimeSpan period)
    {
        if (period == TimeSpan.Zero) return instance;

        var rndTicks = period.Ticks;
        var ansTicks = instance.Ticks + Math.Sign(instance.Ticks) * rndTicks / 2;
        return TimeSpan.FromTicks(ansTicks - ansTicks % rndTicks);
    }
}