using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NJsonSchema.Generation;
using Saunter;
using Saunter.AsyncApiSchema.v2;
using Saunter.Generation;
using Saunter.Generation.Filters;
using Saunter.Generation.SchemaGeneration;
using Saunter.Utils;

namespace Infrastructure;

public class ApiDocumentGenerator : IDocumentGenerator
{
    private IServiceProvider serviceProvider;

    public ApiDocumentGenerator(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public AsyncApiDocument GenerateDocument(TypeInfo[] asyncApiTypes, AsyncApiOptions options, AsyncApiDocument prototype,
        IServiceProvider serviceProvider)
    {
        var asyncApiSchema = prototype.Clone();

        var schemaResolver = new AsyncApiSchemaResolver(asyncApiSchema, options.JsonSchemaGeneratorSettings);

        var generator = new JsonSchemaGenerator(options.JsonSchemaGeneratorSettings);
        asyncApiSchema.Channels = GenerateChannels(schemaResolver, generator);

        var filterContext = new DocumentFilterContext(asyncApiTypes, schemaResolver, generator);
        foreach (var filterType in options.DocumentFilters)
        {
            var filter = (IDocumentFilter)serviceProvider.GetRequiredService(filterType);
            filter.Apply(asyncApiSchema, filterContext);
        }

        return asyncApiSchema;
    }

    IDictionary<string, ChannelItem> GenerateChannels(AsyncApiSchemaResolver schemaResolver, JsonSchemaGenerator schemaGenerator)
    {
        // Need to resolve here the feature specific type because of ordering issues with Saunter
        var typeCache = serviceProvider.GetRequiredService<TypeCache>();
        var channels = new Dictionary<string, ChannelItem>();

        channels.AddRange(GenerateEventChannels(typeCache.PublishedEventCache.Select(kvp => (kvp.Key, kvp.Value)), schemaResolver, schemaGenerator));
        return channels;
    }

    IDictionary<string, ChannelItem> GenerateEventChannels(IEnumerable<(Type Key, Type Value)> eventTypes, AsyncApiSchemaResolver schemaResolver, JsonSchemaGenerator schemaGenerator)
    {
        var publishChannels = new Dictionary<string, ChannelItem>();
        foreach (var (actualType, publishedType) in eventTypes)
        {
            // TODO: Is there a better way to handle the version?
            var operationId = publishedType.FullName;
            var subscribeOperation = new Operation
            {
                OperationId = operationId,
                Summary = string.Empty,
                Description = string.Empty,
                Message = GenerateMessageFromType(actualType, publishedType, schemaResolver, schemaGenerator),
                Bindings = null,
            };

            var channelItem = new ChannelItem
            {
                Description = actualType.FullName,
                Parameters = new Dictionary<string, IParameter>(),
                Publish = null,
                Subscribe = subscribeOperation,
                Bindings = null,
                Servers = null,
            };

            publishChannels.Add(operationId, channelItem);
        }

        return publishChannels;
    }

    private static IMessage GenerateMessageFromType(Type actualType, Type publishedType,
        AsyncApiSchemaResolver schemaResolver, JsonSchemaGenerator jsonSchemaGenerator)
    {
        var message = new Message
        {
            Payload = jsonSchemaGenerator.Generate(actualType, schemaResolver),
            // TODO Should this also use the operation id?
            Name = publishedType.FullName
        };

        return schemaResolver.GetMessageOrReference(message);
    }
}