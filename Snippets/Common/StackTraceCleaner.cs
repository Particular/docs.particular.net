using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Common
{
    public static class StackTraceCleaner
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

                    stringBuilder.AppendLine(line.Split(new[]
                    {
                        " in "
                    }, StringSplitOptions.RemoveEmptyEntries).First());
                }
                return stringBuilder.ToString().Trim();
            }
        }
    }
}