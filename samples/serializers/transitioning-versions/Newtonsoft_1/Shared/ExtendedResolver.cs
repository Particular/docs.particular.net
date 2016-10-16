using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;

#region ExtendedResolver

public class ExtendedResolver : DefaultContractResolver
{
    protected override JsonContract CreateContract(Type objectType)
    {
        if (objectType.GetInterfaces()
            .Any(i => i == typeof(IDictionary) ||
                      (i.IsGenericType &&
                       i.GetGenericTypeDefinition() == typeof(IDictionary<,>))))
        {
            return CreateArrayContract(objectType);
        }

        return base.CreateContract(objectType);
    }
}

#endregion