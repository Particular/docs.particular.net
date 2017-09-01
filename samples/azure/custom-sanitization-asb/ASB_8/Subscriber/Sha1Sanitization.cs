using System.Text.RegularExpressions;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

#region Sha1SanitiazationStrategy

class Sha1Sanitization :
    ISanitizationStrategy
{
    public string Sanitize(string entityPathOrName, EntityType entityType)
    {
        // remove invalid characters
        if (entityType == EntityType.Queue || entityType == EntityType.Topic)
        {
            var regexQueueAndTopicValidCharacters = new Regex(@"[^a-zA-Z0-9\-\._\/]");
            var regexLeadingAndTrailingForwardSlashes = new Regex(@"^\/|\/$");

            entityPathOrName = regexQueueAndTopicValidCharacters.Replace(entityPathOrName, string.Empty);
            entityPathOrName = regexLeadingAndTrailingForwardSlashes.Replace(entityPathOrName, string.Empty);
        }

        if (entityType == EntityType.Subscription || entityType == EntityType.Rule)
        {
            var rgx = new Regex(@"[^a-zA-Z0-9\-\._]");
            entityPathOrName = rgx.Replace(entityPathOrName, "");
        }

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

        // hash if too long
        if (entityPathOrName.Length > entityPathOrNameMaxLength)
        {
            entityPathOrName = SHA1DeterministicNameBuilder.Build(entityPathOrName);
        }

        return entityPathOrName;
    }
}

#endregion