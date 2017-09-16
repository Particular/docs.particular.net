using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        Console.Title = "Samples.Pipeline.UnitOfWork.Endpoint";

        var endpointConfiguration = new EndpointConfiguration("Samples.Pipeline.UnitOfWork.Endpoint");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region configuration
        var sessionProvider = new MySessionProvider();

        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(new MyUowBehavior(sessionProvider), "Manages the session");
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        var key = default(ConsoleKeyInfo);

        Console.WriteLine("Press any key to send messages, 'q' to exit");
        while (key.KeyChar != 'q')
        {
            key = Console.ReadKey();

            for (var i = 1; i < 4; i++)
            {
                var options = new SendOptions();
                options.SetHeader("tenant", "tenant" + i);
                options.RouteToThisEndpoint();
                await endpointInstance.Send(new MyMessage(), options)
                    .ConfigureAwait(false);
            }
        }

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}