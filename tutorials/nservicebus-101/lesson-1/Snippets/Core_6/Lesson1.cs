namespace Core_6
{
    using System;
    using NServiceBus;
    using System.Threading.Tasks;

#pragma warning disable 1998

    #region EmptyProgram
    class Program
    {
        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {

        }
    }
    #endregion

    class StepByStep
    {
        #region AsyncMain
        static async Task AsyncMain()
        {
            Console.Title = "ClientUI";

            var endpointConfiguration = new EndpointConfiguration("ClientUI");
            endpointConfiguration.UseTransport<MsmqTransport>();
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();
        }
        #endregion

        static async Task Steps()
        {
            #region ConsoleTitle
            Console.Title = "ClientUI";
            #endregion

            #region EndpointName
            var endpointConfiguration = new EndpointConfiguration("ClientUI");
            #endregion

            #region Transport
            endpointConfiguration.UseTransport<MsmqTransport>();
            #endregion

            #region Serializer
            endpointConfiguration.UseSerialization<JsonSerializer>();
            #endregion

            #region Persistence
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            #endregion

            #region ErrorQueue
            endpointConfiguration.SendFailedMessagesTo("error");
            #endregion

            #region EnableInstallers
            endpointConfiguration.EnableInstallers();
            #endregion

            #region Startup
            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            await endpointInstance.Stop().ConfigureAwait(false);
            #endregion
        }
    }

#pragma warning restore 1998
}