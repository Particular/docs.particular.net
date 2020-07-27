namespace Core8.Headers.Writers
{
    using System.IO;
    using System.Text;

    public static class BehaviorCleaner
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
                    if (line.Contains("InvokeNext"))
                    {
                        continue;
                    }
                    stringBuilder.AppendLine(line);
                }
                return stringBuilder.ToString().Trim();
            }
        }
    }
}