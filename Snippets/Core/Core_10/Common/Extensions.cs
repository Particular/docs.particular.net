namespace Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class HeaderWriter
{
    public static string ToFriendlyString<TRootTypeToReplace>(IReadOnlyDictionary<string, string> headers)
    {
        var stringBuilder = new StringBuilder();
        foreach (var header in headers.OrderBy(x => x.Key))
        {
            var value = header.Value;
            value = value?.Replace("\r\n", "\n")
                .Replace("\n", "\r\n   ")
                .Replace("`", "")
                .Replace(Environment.MachineName, "MACHINENAME")
                .Replace(Environment.UserName, "USERNAME");
            stringBuilder.Append($"{header.Key} = {value}\r\n");
        }
        var type = typeof(TRootTypeToReplace);
        return stringBuilder.ToString()
            .Replace($"{type.Name}+", "MyNamespace.")
            .Replace($", {type.Assembly.GetName().Name},", ", MyAssembly,");
    }
}