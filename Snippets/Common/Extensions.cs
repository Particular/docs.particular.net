using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Common
{
    public static class HeaderWriter
    {
        static string username = WindowsIdentity.GetCurrent().Name;

        public static string ToFriendlyString<TRootTypeToReplace>(IDictionary<string, string> headers)
        {
            var stringBuilder = new StringBuilder();
            foreach (var header in headers.OrderBy(x=>x.Key))
            {
                var value = header.Value;
                value = value?.Replace("\r\n","\n")
                    .Replace("\n", "\r\n   ")
                    .Replace("`","")
                    .Replace(Environment.MachineName, "MACHINENAME")
                    .Replace(username, "USERNAME");
                stringBuilder.Append($"{header.Key} = {value}\r\n");
            }
            var type = typeof(TRootTypeToReplace);
            return stringBuilder.ToString()
                .Replace($"{type.Name}+", "MyNamespace.")
                .Replace($", {type.Assembly.GetName().Name},", ", MyAssembly,");
        }
    }
}