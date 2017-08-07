namespace SqsAll.QueueCreation
{
    using System;

    static class QueueNameHelper
    {
        public static string GetSqsQueueName(string destination)
        {
            if (string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentNullException(nameof(destination));
            }

            // SQS queue names can only have alphanumeric characters, hyphens and underscores.
            // Any other characters will be replaced with a hyphen.
            for (var i = 0; i < destination.Length; ++i)
            {
                var c = destination[i];
                if (!char.IsLetterOrDigit(c)
                    && c != '-'
                    && c != '_')
                {
                    destination = destination.Replace(c, '-');
                }
            }

            if (destination.Length > 80)
            {
                throw new Exception(
                    $"Address {destination} is longer than 80 characters and therefore cannot be used to create an SQS queue. Use a shorter queue name.");
            }

            return destination;
        }
    }
}