using System.Diagnostics;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService();

#region generic-host-nservicebus

var endpointConfiguration = new EndpointConfiguration("Samples.Hosting.GenericHost");
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);


//  It is recommended to run least privilege and only run installers during deployment.
//  This also reduces startup time / time to first message.

var isDevelopment = Debugger.IsAttached;
var isSetup = args.Contains("--setup");

if (isSetup)
{
    // Provision resources like transport queue creation and persister schemas
    await NServiceBus.Installation.Installer.Setup(endpointConfiguration);
    return; // Exit application
}
else if (isDevelopment)
{
    endpointConfiguration.EnableInstallers();
}

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
        await context.Stop(cancellationToken);
    }
    finally
    {
        Environment.FailFast(fatalMessage, context.Exception);
    }
}

#endregion
