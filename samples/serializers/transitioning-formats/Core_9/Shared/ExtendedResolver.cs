using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;

#region ExtendedResolver

public class ExtendedResolver :
    DefaultContractResolver
{
    protected override JsonContract CreateContract(Type objectType)
    {
        if (objectType.GetInterfaces().Any(IsDictionary))
        {
            return CreateArrayContract(objectType);
        }

        return base.CreateContract(objectType);
    }

    bool IsDictionary(Type type)
    {
        return type == typeof(IDictionary) ||
               type.IsGenericType &&
               type.GetGenericTypeDefinition() == typeof(IDictionary<,>);
    }
}

#endregion