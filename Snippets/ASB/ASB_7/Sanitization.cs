using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using NServiceBus;
using NServiceBus.AzureServiceBus;
using NServiceBus.AzureServiceBus.Addressing;
using NServiceBus.Settings;

class Sanitization
{
    void ThrowOnFailedValidationOverrides(TransportExtensions<AzureServiceBusTransport> transport)
    {
        #region asb-ThrowOnFailedValidation-sanitization-overrides

        var sanitization = transport.Sanitization();
        var strategy = sanitization.UseStrategy<ThrowOnFailedValidation>();
        strategy.QueuePathValidation(queuePath => new ValidationResult());
        strategy.TopicPathValidation(topicPath => new ValidationResult());
        strategy.SubscriptionNameValidation(subscriptionName => new ValidationResult());
        strategy.RuleNameValidation(ruleName => new ValidationResult());

        #endregion
    }

    void ValidateAndHashIfNeededOverrides(TransportExtensions<AzureServiceBusTransport> transport)
    {
        #region asb-ValidateAndHashIfNeeded-sanitization-overrides

        var sanitization = transport.Sanitization();
        var strategy = sanitization.UseStrategy<ValidateAndHashIfNeeded>();
        strategy.QueuePathSanitization(queuePath => "sanitized queuePath");
        strategy.TopicPathSanitization(topicPath => "sanitized topicPath");
        strategy.SubscriptionNameSanitization(subscriptionName => "sanitized subscriptionName");
        strategy.RuleNameSanitization(ruleName => "sanitized ruleName");
        strategy.Hash(pathOrName => "hashed pathOrName");

        #endregion
    }


    void CustomSanitization(EndpointConfiguration endpointConfiguration)
    {
        #region asb-custom-sanitization

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var sanitization = transport.Sanitization();
        sanitization.UseStrategy<CustomSanitization>();

        #endregion
    }
}

#region asb-custom-sanitization-strategy

class CustomSanitization :
    ISanitizationStrategy
{
    public string Sanitize(string entityPathOrName, EntityType entityType)
    {
        // apply sanitization on entityPathOrName
        return entityPathOrName;
    }
}

#endregion

#region custom-sanitization-strategy-with-settings

public class CustomSanitizationWithSettings :
    ISanitizationStrategy
{
    ReadOnlySettings settings;

    public CustomSanitizationWithSettings(ReadOnlySettings settings)
    {
        this.settings = settings;
    }

    public string Sanitize(string entityPathOrName, EntityType entityType)
    {
        // implementation custom sanitization here
        return entityPathOrName;
    }
}

#endregion

#region asb-backward-compatible-custom-sanitiaztion-strategy

class V6Sanitization :
    ISanitizationStrategy
{
    public string Sanitize(string entityPathOrName, EntityType entityType)
    {
        // remove invalid characters
        var regex = new Regex(@"[^a-zA-Z0-9\-\._]");
        entityPathOrName = regex.Replace(entityPathOrName, string.Empty);

        var entityPathOrNameMaxLength = 0;

        switch (entityType)
        {
            case EntityType.Queue:
            case EntityType.Topic:
                entityPathOrNameMaxLength = 260;
                break;
            case EntityType.Subscription:
            case EntityType.Rule:
                entityPathOrNameMaxLength = 50;
                break;
        }

        // hash if still too long
        if (entityPathOrName.Length > entityPathOrNameMaxLength)
        {
            entityPathOrName = MD5DeterministicNameBuilder.Build(entityPathOrName);
        }

        return entityPathOrName;
    }

    static class MD5DeterministicNameBuilder
    {
        public static string Build(string input)
        {
            var inputBytes = Encoding.Default.GetBytes(input);
            //use MD5 hash to get a 16-byte hash of the string
            using (var provider = new MD5CryptoServiceProvider())
            {
                var hashBytes = provider.ComputeHash(inputBytes);
                return new Guid(hashBytes).ToString();
            }
        }
    }

}

#endregion