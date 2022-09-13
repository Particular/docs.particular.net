using Elsa.Options;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace NServiceBus.Activities
{
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
