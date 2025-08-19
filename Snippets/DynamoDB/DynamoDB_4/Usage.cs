namespace DynamoDB_4;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using NServiceBus;
using NServiceBus.Persistence.DynamoDB;

public class Usage
{
    void Configuration(EndpointConfiguration endpointConfiguration)
    {
        #region DynamoDBUsage
        var persistence = endpointConfiguration.UsePersistence<DynamoPersistence>();
        // optional client
        persistence.DynamoClient(new AmazonDynamoDBClient());
        #endregion
    }

    public void EventualConsistent(PersistenceExtensions<DynamoPersistence> persistence)
    {
        #region DynamoDBSagaEventualConsistentReads

        var sagas = persistence.Sagas();
        sagas.UseEventuallyConsistentReads = true;

        #endregion
    }

    public void SagaOptions(PersistenceExtensions<DynamoPersistence> persistence)
    {
        #region DynamoDBSagaOptions

        var sagas = persistence.Sagas();
        sagas.MapperOptions = new JsonSerializerOptions(Mapper.Default)
        {
            // customize serialization options
            Converters = { new JsonStringEnumConverter() }
        };

        #endregion
    }

    public void MapperContextOptions(object customer)
    {
        #region DynamoDBMapperContextUsageWithJsonOptions

        var serializerOptions = new JsonSerializerOptions(Mapper.Default)
        {
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers = { SupportObjectModelAttributes }
            }
        };

        var customerMap = Mapper.ToMap(customer, serializerOptions);

        // sample modifier showing advanced extensibility using the json type info resolver
        // In source generator mode the custom attributes cannot be introspected
        static void SupportObjectModelAttributes(JsonTypeInfo typeInfo)
        {
            if (typeInfo.Kind != JsonTypeInfoKind.Object)
            {
                return;
            }

            foreach (var property in typeInfo.Properties)
            {
                if (property.AttributeProvider?.GetCustomAttributes(typeof(DynamoDBRenamableAttribute), true)
                        .SingleOrDefault() is DynamoDBRenamableAttribute renamable &&
                    !string.IsNullOrEmpty(renamable.AttributeName))
                {
                    property.Name = renamable.AttributeName;
                }
                else if (property.AttributeProvider?.GetCustomAttributes(typeof(DynamoDBIgnoreAttribute), true)
                             .SingleOrDefault() is DynamoDBIgnoreAttribute)
                {
                    property.ShouldSerialize = (_, __) => false;
                }
                else
                {
                    property.ShouldSerialize = (_, __) => false;
                }
            }
        }

        #endregion
    }
}