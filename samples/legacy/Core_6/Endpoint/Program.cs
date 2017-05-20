using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main(string[] args)
    {
        Start().GetAwaiter().GetResult();
    }

    static async Task Start()
    {
        var cultures = new[] {"pl-PL", "en-US", "fr-FR"};
        var users = new[] {"Bob", "Alice", "Carol"};
        var random = new Random();

        var config = new EndpointConfiguration("Legacy.Endpoint");
        config.UseTransport<MsmqTransport>();
        config.UsePersistence<InMemoryPersistence>();
        config.SendFailedMessagesTo("error");
        config.EnableInstallers();

        config.Pipeline.Register(new CaptureMessageSessionBehavior(), "Captures message processing context.");
        config.Pipeline.Register(new SetCultureBehavior(), "Sets culture value based on headers.");
        config.Pipeline.Register(new ForwardCultureBehavior(), "Copies the culture of incoming message to the outgoing messages.");
        config.Pipeline.Register(new SetPrincipalBehavior(), "Sets principal value based on headers.");
        config.Pipeline.Register(new ForwardUserBehavior(), "Copies the user name from the incoming message to the outgoing message.");
        config.Pipeline.Register(new DispatchLegacyBusMessgesBehavior(), "Dispatches messages sent by legacy bus replacement.");

        config.RegisterComponents(c =>
        {
            c.ConfigureComponent<MyFacade>(DependencyLifecycle.InstancePerCall);
            c.ConfigureComponent<MyService>(DependencyLifecycle.InstancePerCall);
            c.ConfigureComponent<MyOtherService>(DependencyLifecycle.InstancePerCall);
            c.ConfigureComponent<LegacyBus>(DependencyLifecycle.InstancePerUnitOfWork);
        });

        var endpoint = await Endpoint.Start(config);

        Console.WriteLine("Press <enter> to send a message");
        while (true)
        {
            Console.ReadLine();
            var culture = cultures[random.Next(cultures.Length)];
            var user = users[random.Next(users.Length)];

            Console.WriteLine($"Sending a message on behalf of {user} with culture set to {culture}");
            var options = new SendOptions();
            options.SetHeader("Culture", culture);
            options.SetHeader("User", user);
            options.RouteToThisEndpoint();

            await endpoint.Send(new MyMessage
            {
                Value = random.Next(1000)
            }, options);
        }
    }
}