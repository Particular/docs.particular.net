using Elsa.Options;

using Microsoft.Extensions.DependencyInjection;

namespace NServiceBus.Activities
{
    /// <summary>
    /// This class will register the NServiceBus related Elsa activities and bookmark providers.
    /// </summary>
    public static class NServiceBusServicesCollectionExtensions
    {
        public static ElsaOptionsBuilder AddNServiceBusActivities(this ElsaOptionsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            
            return builder
                .AddActivity<SendNServiceBusMessage>()
                .AddActivity<PublishNServiceBusEvent>()
                .AddActivity<NServiceBusMessageReceived>();
        }

        public static IServiceCollection AddNServiceBusBookmarkProviders(this IServiceCollection services)
        {
            return services
                .AddBookmarkProvider<MessageReceivedBookmarkProvider>();
        }
    }
}
