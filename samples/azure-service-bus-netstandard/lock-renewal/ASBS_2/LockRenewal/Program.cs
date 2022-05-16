﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using Azure.Messaging.ServiceBus.Administration;
using LockRenewal;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ASB.LockRenewal";

        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.SendReply.LockRenewal");
        endpointConfiguration.EnableInstallers();

        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
        }

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>().ConnectionString(connectionString);

        #region override-lock-renewal-configuration

        var lockDuration = TimeSpan.FromSeconds(30);
        var renewalInterval = TimeSpan.FromSeconds(5);

        endpointConfiguration.LockRenewal(options =>
        {
            options.LockDuration = lockDuration;
            options.RenewalInterval = renewalInterval;
        });

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

        await OverrideQueueLockDuration("Samples.ASB.SendReply.LockRenewal", connectionString, lockDuration).ConfigureAwait(false);

        await endpointInstance.SendLocal(new LongProcessingMessage { ProcessingDuration = TimeSpan.FromSeconds(45) });

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop().ConfigureAwait(false);
    }

    static async Task OverrideQueueLockDuration(string queuePath, string connectionString, TimeSpan lockDuration)
    {
        var managementClient = new ServiceBusAdministrationClient(connectionString);
        var queueDescription = await managementClient.GetQueueAsync(queuePath).ConfigureAwait(false);
        queueDescription.Value.LockDuration = lockDuration;

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