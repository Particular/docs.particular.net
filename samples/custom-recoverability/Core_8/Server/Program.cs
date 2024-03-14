using NServiceBus;
using NServiceBus.Transport;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.CustomRecoverability.Server";
        var endpointConfiguration = new EndpointConfiguration("Samples.CustomRecoverability.Server");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport(new LearningTransport());
        var recoverability = endpointConfiguration.Recoverability();

        #region disable
        //recoverability.Delayed(settings =>
        //{
        //    settings.NumberOfRetries(0);
        //});
        #endregion

        #region addcustompolicy
        recoverability.CustomPolicy(MyCustomRetryPolicy());
        #endregion

        #region addcustomheaders
        recoverability.Failed(
        failed =>
        {
            failed.HeaderCustomization(headers =>
            {
                if (headers.ContainsKey("NServiceBus.ExceptionInfo.Message"))
                {
                    headers["NServiceBus.ExceptionInfo.Message"] = "message override";
                }
            });
        });
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }

    #region mycustomretrypolicy
    private static Func<RecoverabilityConfig, ErrorContext, RecoverabilityAction> MyCustomRetryPolicy()
    {
        return (config, errorContext) =>
          errorContext.Exception is ArgumentNullException
              ? DefaultRecoverabilityPolicy.Invoke(config, errorContext)
              : RecoverabilityAction.MoveToError("error");
    }
    #endregion

}
