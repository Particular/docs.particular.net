using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        var configuration = new EndpointConfiguration("FluentValidationSample");
        configuration.UsePersistence<LearningPersistence>();
        configuration.UseTransport<LearningTransport>();

        #region Enable
        var validation = configuration.UseFluentValidation(outgoing: false);
        validation.AddValidatorsFromAssemblyContaining<MyMessage>();
        #endregion

        var endpoint = await Endpoint.Start(configuration).ConfigureAwait(false);

        await endpoint.SendLocal(new MyMessage {Content = "sd"}).ConfigureAwait(false);
        await endpoint.SendLocal(new MyMessage()).ConfigureAwait(false);

        Console.WriteLine("Press any key to stop program");
        Console.Read();
        await endpoint.Stop().ConfigureAwait(false);
    }
}