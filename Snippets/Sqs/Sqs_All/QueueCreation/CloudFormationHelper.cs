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
            var joined = string.Join(Environment.NewLine, allLines
                .Where(l => !l.StartsWith("//", StringComparison.InvariantCultureIgnoreCase) || 
                                !l.StartsWith("//", StringComparison.InvariantCultureIgnoreCase)));
            return joined;
        }
    }
}