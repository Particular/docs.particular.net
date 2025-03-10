using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Transport;

class Program
{

    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                Console.Title = "Server";
            }).UseNServiceBus(x =>
            {
                var endpointConfiguration = new EndpointConfiguration("Samples.CustomRecoverability.Server");
                endpointConfiguration.UsePersistence<LearningPersistence>();
                endpointConfiguration.UseTransport(new LearningTransport());
                endpointConfiguration.UseSerialization<SystemJsonSerializer>();
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

                return endpointConfiguration;
            });


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
