namespace Common;

using System;
using System.IO;
using System.Linq;
using System.Text;

public static class StackTraceCleaner
{

    public static string CleanStackTrace(string stackTrace)
    {
        if (stackTrace == null)
        {
            return string.Empty;
        }
        using (var stringReader = new StringReader(stackTrace))
        {
            var stringBuilder = new StringBuilder();
            while (true)
            {
                var line = stringReader.ReadLine();
                if (line == null)
                {
                    break;
                }

                if (line.Contains("System.Runtime") || line.Contains("---"))
                {
                    continue;
                }

                stringBuilder.AppendLine(line.Split(new[]
                {
                    " in "
                }, StringSplitOptions.RemoveEmptyEntries).First());
            }
            return stringBuilder.ToString().Trim();
        }
    }
}