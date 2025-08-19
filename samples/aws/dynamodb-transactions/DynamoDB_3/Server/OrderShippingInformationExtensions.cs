using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using NServiceBus.Persistence.DynamoDB;

public static class OrderShippingInformationExtensions
{
    public static Dictionary<string, AttributeValue> ToMap(this OrderShippingInformation orderShippingInformation)
    {
        var attributeMap = Mapper.ToMap(orderShippingInformation);
        var orderShippingInformationId = orderShippingInformation.Id.ToString();
        attributeMap["PK"] = new AttributeValue(orderShippingInformationId);
        attributeMap["SK"] = new AttributeValue(orderShippingInformationId);
        return attributeMap;
    }
}