﻿using Microsoft.Extensions.Hosting;

using NServiceBus;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.DependencyInjection;


var builder = Host.CreateApplicationBuilder(args);

/*
#region generic-host-service-lifetime
  builder.Services.AddWindowsService();
#endregion
*/

#region generic-host-nservicebus

var endpointConfiguration = new EndpointConfiguration("Samples.Hosting.GenericHost");

var routing = endpointConfiguration.UseTransport(new LearningTransport());

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);

endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);

#endregion

#region generic-host-worker-registration

builder.Services.AddHostedService<Worker>();

#endregion

var app = builder.Build();
app.Run();

 #region generic-host-critical-error

static async Task OnCriticalError(ICriticalErrorContext context, CancellationToken cancellationToken)
{
     var fatalMessage =
            $"The following critical error was encountered:{Environment.NewLine}{context.Error}{Environment.NewLine}Process is shutting down. StackTrace: {Environment.NewLine}{context.Exception.StackTrace}";

    try
    {
        await context.Stop(cancellationToken).ConfigureAwait(false);
    }
    finally
    {
       Environment.FailFast(fatalMessage, context.Exception);
    }
}

#endregion
