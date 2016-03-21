namespace Snippets5.Azure.Transports.AzureServiceBus
{
    using System;
    using System.Text.RegularExpressions;
    using NServiceBus;
    using NServiceBus.Azure.Transports.WindowsAzureServiceBus.QueueAndTopicByEndpoint;
    using NServiceBus.Settings;

    class NamingConventionsUsage
    {
        NamingConventionsUsage()
        {
            #region ASB-NamingConventions-entity-sanitization [6.4,7)

            NamingConventions.EntitySanitizationConvention = (entityName, entityType) =>
            {
                if (entityType == EntityType.Queue || entityType == EntityType.Topic)
                {
                    Regex regexQueueAndTopicValidCharacters = new Regex(@"[^a-zA-Z0-9\-\._\/]");
                    Regex regexLeadingAndTrailingForwardSlashes = new Regex(@"^\/|\/$");

                    string result = regexQueueAndTopicValidCharacters.Replace(entityName, "");
                    return regexLeadingAndTrailingForwardSlashes.Replace(result, "");
                }

                // Subscription
                Regex regexSubscriptionValidCharacters = new Regex(@"[^a-zA-Z0-9\-\._]");
                return regexSubscriptionValidCharacters.Replace(entityName, "");
            };

            #endregion
          
            Func<ReadOnlySettings, Type, string, bool, string> DetermineQueueNameUsingCustomLogic = (settings, messagetype, queueName, doNotIndividualize) => "";
            Func<ReadOnlySettings, Type, string, string> DetermineTopicNameUsingCustomLogic = (settings, messageType, endpointName) => "";
            Func<ReadOnlySettings, Type, string, string> DetermineSubscriptionNameUsingCustomLogic = (settings, eventType, endpointName) => "";
            Func<ReadOnlySettings, Address, Address> DeterminePublisherNameUsingCustomLogic = (settings, address) => new Address("queue", "machine");
            Func<ReadOnlySettings, Address, bool, Address> DetermineQueueAddressUsingCustomLogic = (settings, address, doNotIndividualize) => new Address("queue", "machine");
            Func<ReadOnlySettings, Address, Address> DeterminePublisherAddressUsingCustomLogic = (settings, address) => new Address("queue", "machine");

            #region ASB-NamingConventions-entity-creation-conventions [6.4,7)

            NamingConventions.QueueNamingConvention = (settings, messageType, queue, doNotIndividualize) => DetermineQueueNameUsingCustomLogic(settings, messageType, queue, doNotIndividualize);

            NamingConventions.TopicNamingConvention = (settings, messageType, endpointName) => DetermineTopicNameUsingCustomLogic(settings, messageType, endpointName);

            NamingConventions.SubscriptionNamingConvention = (settings, eventType, endpointName) => DetermineSubscriptionNameUsingCustomLogic(settings, eventType, endpointName);

            NamingConventions.PublisherAddressConvention = (settings, address) => DeterminePublisherNameUsingCustomLogic(settings, address);

            NamingConventions.QueueAddressConvention = (settings, address, doNotIndividualize) => DetermineQueueAddressUsingCustomLogic(settings, address, doNotIndividualize);

            NamingConventions.PublisherAddressConvention = (settings, address) => DeterminePublisherAddressUsingCustomLogic(settings, address);

            #endregion
        }
    }
}