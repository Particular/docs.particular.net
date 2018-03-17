namespace SqsAll.QueueCreation
{
    using System;
    using System.IO;
    using System.Linq;

    public class CloudFormationHelper
    {
        /// <summary>
        /// CloudFormation doesn't allow comments found at the beginning and end of template for documentation in the template body.
        /// </summary>
        public static string ConvertToValidJson(string templatePath)
        {
            var allLines = File.ReadAllLines(templatePath);
            return string.Join(Environment.NewLine, allLines.Skip(1).Take(allLines.Length - 2));
        }
    }
}