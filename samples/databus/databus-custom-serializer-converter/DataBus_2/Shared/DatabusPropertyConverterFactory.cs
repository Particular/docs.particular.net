using NServiceBus.ClaimCheck;
using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using NServiceBus;

namespace Shared;

#region DatabusPropertyConverterFactory

public class ClaimCheckPropertyConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType)
        {
            return false;
        }

        if (typeToConvert.GetGenericTypeDefinition() != typeof(ClaimCheckProperty<>))
        {
            return false;
        }

        return true;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type keyType = typeToConvert.GetGenericArguments()[0];

        JsonConverter converter = (JsonConverter)Activator.CreateInstance(
            typeof(ClaimCheckPropertyConverter<>).MakeGenericType(
                new Type[] { keyType }),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: new object[] { options },
            culture: null)!;

        return converter;
    }
}

#endregion