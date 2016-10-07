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

            var endpointConfig = new EndpointConfiguration("ClientUI");
            endpointConfig.UseTransport<MsmqTransport>();
            endpointConfig.UseSerialization<JsonSerializer>();
            endpointConfig.UsePersistence<InMemoryPersistence>();
            endpointConfig.SendFailedMessagesTo("error");
            endpointConfig.EnableInstallers();
        }
        #endregion

        static async Task Steps()
        {
            #region ConsoleTitle
            Console.Title = "ClientUI";
            #endregion

            #region EndpointName
            var endpointConfig = new EndpointConfiguration("ClientUI");
            #endregion

            #region Transport
            endpointConfig.UseTransport<MsmqTransport>();
            #endregion

            #region Serializer
            endpointConfig.UseSerialization<JsonSerializer>();
            #endregion

            #region Persistence
            endpointConfig.UsePersistence<InMemoryPersistence>();
            #endregion

            #region ErrorQueue
            endpointConfig.SendFailedMessagesTo("error");
            #endregion

            #region EnableInstallers
            endpointConfig.EnableInstallers();
            #endregion

            #region Startup
            var endpointInstance = await Endpoint.Start(endpointConfig).ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            await endpointInstance.Stop().ConfigureAwait(false);
            #endregion
        }
    }

#pragma warning restore 1998
}