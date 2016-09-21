using System;
using System.Text.RegularExpressions;
using NServiceBus;
using NServiceBus.Azure.Transports.WindowsAzureServiceBus.QueueAndTopicByEndpoint;
using NServiceBus.Settings;

class NamingConventionsUsage
{
    NamingConventionsUsage()
    {
        #region ASB-NamingConventions-entity-sanitization 6.4

        NamingConventions.EntitySanitizationConvention = (entityName, entityType) =>
        {
            if (entityType == EntityType.Queue || entityType == EntityType.Topic)
            {
                var regexQueueAndTopicValidCharacters = new Regex(@"[^a-zA-Z0-9\-\._\/]");
                var regexLeadingAndTrailingForwardSlashes = new Regex(@"^\/|\/$");

                var result = regexQueueAndTopicValidCharacters.Replace(entityName, "");
                return regexLeadingAndTrailingForwardSlashes.Replace(result, "");
            }

            // Subscription
            var regexSubscriptionValidCharacters = new Regex(@"[^a-zA-Z0-9\-\._]");
            return regexSubscriptionValidCharacters.Replace(entityName, "");
        };

        #endregion

        Func<ReadOnlySettings, Type, string, bool, string> DetermineQueueNameUsingCustomLogic = null;
        Func<ReadOnlySettings, Type, string, string> DetermineTopicNameUsingCustomLogic = null;
        Func<ReadOnlySettings, Type, string, string> DetermineSubscriptionNameUsingCustomLogic = null;
        Func<ReadOnlySettings, Address, Address> DeterminePublisherNameUsingCustomLogic = null;
        Func<ReadOnlySettings, Address, bool, Address> DetermineQueueAddressUsingCustomLogic = null;
        Func<ReadOnlySettings, Address, Address> DeterminePublisherAddressUsingCustomLogic = null;

        #region ASB-NamingConventions-entity-creation-conventions 6.4

        NamingConventions.QueueNamingConvention = (settings, messageType, queue, doNotIndividualize) =>
        {
            return DetermineQueueNameUsingCustomLogic(settings, messageType, queue, doNotIndividualize);
        };

        NamingConventions.TopicNamingConvention = (settings, messageType, endpointName) =>
        {
            return DetermineTopicNameUsingCustomLogic(settings, messageType, endpointName);
        };

        NamingConventions.SubscriptionNamingConvention = (settings, eventType, endpointName) =>
        {
            return DetermineSubscriptionNameUsingCustomLogic(settings, eventType, endpointName);
        };

        NamingConventions.PublisherAddressConvention = (settings, address) =>
        {
            return DeterminePublisherNameUsingCustomLogic(settings, address);
        };

        NamingConventions.QueueAddressConvention = (settings, address, doNotIndividualize) =>
        {
            return DetermineQueueAddressUsingCustomLogic(settings, address, doNotIndividualize);
        };

        NamingConventions.PublisherAddressConvention = (settings, address) =>
        {
            return DeterminePublisherAddressUsingCustomLogic(settings, address);
        };

        #endregion
    }
}