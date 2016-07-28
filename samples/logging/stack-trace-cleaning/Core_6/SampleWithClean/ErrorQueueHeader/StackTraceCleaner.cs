using System.Collections.Generic;
using System.IO;
using System.Text;

#region StackTraceCleaner
class StackTraceCleaner
{
    public static void CleanUp(Dictionary<string, string> headers)
    {
        var stackTraceKey = "NServiceBus.ExceptionInfo.StackTrace";
        var stackTrace = headers[stackTraceKey];

        var builder = new StringBuilder();
        using (var reader = new StringReader(stackTrace))
        {
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                if (
                    line.Contains("System.Runtime.CompilerServices.TaskAwaiter") ||
                    line.Contains("End of stack trace from previous location where exception was thrown")
                    )
                {
                    continue;
                }
                builder.AppendLine(line);
            }
        }
        headers[stackTraceKey] = builder.ToString();
    }
}

#endregion