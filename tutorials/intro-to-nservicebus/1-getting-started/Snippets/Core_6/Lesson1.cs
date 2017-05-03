namespace Core_6
{
    using System;
    using NServiceBus;
    using System.Threading.Tasks;

#pragma warning disable 1998

    #region EmptyProgram
    class Program
    {
        static void Main()
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

            #region Serializer
            endpointConfiguration.UseSerialization<XmlSerializer>();
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