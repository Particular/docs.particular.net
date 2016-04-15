namespace Snippets5.Headers.Writers
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
            using (StringReader stringReader = new StringReader(stackTrace))
            {
                StringBuilder stringBuilder = new StringBuilder();
                while (true)
                {
                    string line = stringReader.ReadLine();
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