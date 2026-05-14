using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NServiceBus;

namespace OuterNS.InnerNS;

public record MyMessage();
public record Msg1();
public record Msg2();

#region SimpleConventionBasedHandler

public class ConventionHandler
{
    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        // do something with the message data
    }
}

#endregion

#region ConventionsBasedHandlerExtraParams

public class ConventionHandlerWithExtraParams
{
    public async Task Handle(MyMessage message,
        IMessageHandlerContext context,
        DatabaseService database,
        CancellationToken cancellationToken)
    {
        // do something with the message data
    }
}

#endregion

#region DecoratedConventionHandler

[Handler]
public class DecoratedConventionHandler
{
    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
    }
}

#endregion


#region ConventionHandlersTldrSample

[Handler]
public class HandlersByConvention
{
    public async Task Handle(Msg1 message,
        IMessageHandlerContext context,
        CancellationToken cancellationToken)
    {
    }

    public static async Task Handle(Msg2 message,
        IMessageHandlerContext context,
        DatabaseService database,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        CancellationToken cancellationToken)
    {
    }
}
#endregion

public class DatabaseService;

public class ConventionHandlerRegistration
{
    public static void Setup(EndpointConfiguration endpointConfiguration)
    {
        #region ConventionHandlerRegistrationWithoutAttribute

        endpointConfiguration.AddHandler<ConventionHandler>();

        #endregion

        #region ConventionHandlerAddAllFromAssembly
        endpointConfiguration.Handlers.SampleProject.AddAll();
        #endregion

        #region ConventionHandlerAllGeneratedAddMethods

        // Add just one handler
        endpointConfiguration.Handlers.SampleProject.OuterNS.InnerNS
            .AddDecoratedConventionHandler();

        // Add all handlers or sagas from a namespace…
        endpointConfiguration.Handlers.SampleProject.OuterNS.InnerNS.AddAllHandlers();
        endpointConfiguration.Handlers.SampleProject.OuterNS.InnerNS.AddAllSagas();
        endpointConfiguration.Handlers.SampleProject.OuterNS.InnerNS.AddAll();
        // …at any point in the namespace hierarchy
        endpointConfiguration.Handlers.SampleProject.OuterNS.AddAllHandlers();
        endpointConfiguration.Handlers.SampleProject.OuterNS.AddAllSagas();
        endpointConfiguration.Handlers.SampleProject.OuterNS.AddAll();

        // Or add all from an entire assembly
        endpointConfiguration.Handlers.SampleProject.AddAllHandlers();
        endpointConfiguration.Handlers.SampleProject.AddAllSagas();
        endpointConfiguration.Handlers.SampleProject.AddAll();

        #endregion
    }
}

[HandlerRegistryExtensions(EntryPointName = "SampleProject")]
public static partial class RegistrationExtensions;