using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using NServiceBus;
using NServiceBus.AzureServiceBus;
using NServiceBus.AzureServiceBus.Addressing;
using NServiceBus.Settings;

[SuppressMessage("ReSharper", "UnusedMember.Local")]
public class Sanitization
{
    void ThrowOnFailedValidationOverrides(EndpointConfiguration endpointConfiguration)
    {
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();

        #region asb-ThrowOnFailedValidation-sanitization-overrides

        transport.Sanitization().UseStrategy<ThrowOnFailedValidation>()
            .QueuePathValidation(queuePath => new ValidationResult())
            .TopicPathValidation(topicPath => new ValidationResult())
            .SubscriptionNameValidation(subscriptionName => new ValidationResult())
            .RuleNameValidation(ruleName => new ValidationResult());

        #endregion
    }

    void ValidateAndHashIfNeededOverrides(EndpointConfiguration endpointConfiguration)
    {
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();

        #region asb-ValidateAndHashIfNeeded-sanitization-overrides

        transport.Sanitization().UseStrategy<ValidateAndHashIfNeeded>()
            .QueuePathSanitization(queuePath => "sanitized queuePath")
            .TopicPathSanitization(topicPath => "sanitized topicPath")
            .SubscriptionNameSanitization(subscriptionName => "sanitized subscriptionName")
            .RuleNameSanitization(ruleName => "sanitized ruleName")
            .Hash(pathOrName => "hashed pathOrName");

        #endregion
    }


    void CustomSanitization(EndpointConfiguration endpointConfiguration)
    {
        #region asb-custom-sanitization

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();

        transport.Sanitization().UseStrategy<CustomSanitization>();

        #endregion
    }
}

#region asb-custom-sanitization-strategy

class CustomSanitization : ISanitizationStrategy
{
    public string Sanitize(string entityPathOrName, EntityType entityType)
    {
        // apply sanitization on entityPathOrName
        return entityPathOrName;
    }
}

#endregion

#region custom-sanitization-strategy-with-settings

public class CustomSanitizationWithSettings : ISanitizationStrategy
{
    ReadOnlySettings settings;

    public CustomSanitizationWithSettings(ReadOnlySettings settings)
    {
        this.settings = settings;
    }

    public string Sanitize(string entityPathOrName, EntityType entityType)
    {
        throw new NotImplementedException();
    }
}

#endregion

#region asb-backward-compatible-custom-sanitiaztion-strategy

class V6Sanitization : ISanitizationStrategy
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
            //use MD5 hash to get a 16-byte hash of the string
            using (var provider = new MD5CryptoServiceProvider())
            {
                var inputBytes = Encoding.Default.GetBytes(input);
                var hashBytes = provider.ComputeHash(inputBytes);

                return new Guid(hashBytes).ToString();
            }
        }
    }

}

#endregion