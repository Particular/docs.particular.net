using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region Behavior
class ReplaceStackTraceBehavior : Behavior<IFaultContext>
{
    public override Task Invoke(IFaultContext context, Func<Task> next)
    {
        Dictionary<string, string> headers = context.Message.Headers;
        string stackTraceKey = "NServiceBus.ExceptionInfo.StackTrace";
        string stackTrace = headers[stackTraceKey];

        StringBuilder builder = new StringBuilder();
        using (StringReader reader = new StringReader(stackTrace))
        {
            while (true)
            {
                string line = reader.ReadLine();
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
        return next();
    }
}

#endregion