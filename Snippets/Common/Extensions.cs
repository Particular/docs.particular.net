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
            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<string, string> header in headers.OrderBy(x=>x.Key))
            {
                string value = header.Value;
                if (value != null)
                {
                    value = value
                        .Replace("\r\n","\n")
                        .Replace("\n", "\r\n   ")
                        .Replace("`","")
                        .Replace(Environment.MachineName, "MACHINENAME")
                        .Replace(username, "USERNAME");
                }
                stringBuilder.AppendFormat("{0} = {1}\r\n", header.Key, value);
            }
            Type type = typeof(TRootTypeToReplace);
            return stringBuilder.ToString()
                .Replace(type.Name + "+", "MyNamespace.")
                .Replace(", "+type.Assembly.GetName().Name + ",", ", MyAssembly,");
        }
    }
}