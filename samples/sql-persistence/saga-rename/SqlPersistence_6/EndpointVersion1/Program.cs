﻿using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    static async Task Main()
    {
        var defaultFactory = LogManager.Use<DefaultFactory>();
        defaultFactory.Level(LogLevel.Warn);

        Console.Title = "Version1";

        var endpointConfiguration = new EndpointConfiguration("Samples.RenameSaga");

        SharedConfiguration.Apply(endpointConfiguration);

        endpointConfiguration.PurgeOnStartup(true);
        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Version1 of Sagas starting. Will exit in 5 seconds. After exist start Phase 2 Endpoint.");

        #region startSagas
        var startReplySaga = new StartReplySaga
        {
            TheId = Guid.NewGuid()
        };
        await endpointInstance.SendLocal(startReplySaga);

        var startTimeoutSaga = new StartTimeoutSaga
        {
            TheId = Guid.NewGuid()
        };
        await endpointInstance.SendLocal(startTimeoutSaga);
        #endregion

        await Task.Delay(TimeSpan.FromSeconds(5));
        await endpointInstance.Stop();
    }
}