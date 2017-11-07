namespace Core_6
{
    using System;
    using NServiceBus;
    using System.Threading.Tasks;

#pragma warning disable 1998

    #region EmptyProgram
    class Program
    {
        static async Task Main()
        {

        }
    }
    #endregion

    class StepByStep
    {
        #region AsyncMain
        static async Task Main()
        {
            Console.Title = "ClientUI";

            var endpointConfiguration = new EndpointConfiguration("ClientUI");

            var transport = endpointConfiguration.UseTransport<LearningTransport>();

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

            #region LearningTransport
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            #endregion

            #region Startup
            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
            #endregion
        }
    }

#pragma warning restore 1998
}
