using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared
{
#pragma warning disable CS0618 // Type or member is obsolete
    #region DatabusPropertyConverterFactory
    public class DatabusPropertyConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
            {
                return false;
            }
            if (typeToConvert.GetGenericTypeDefinition() != typeof(DataBusProperty<>))
            {
                return false;
            }
            return true;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Type keyType = typeToConvert.GetGenericArguments()[0];

            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                typeof(DatabusPropertyConverter<>).MakeGenericType(
                    new Type[] { keyType }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { options },
                culture: null)!;

            return converter;
        }
    }
    #endregion
#pragma warning restore CS0618 // Type or member is obsolete
}
