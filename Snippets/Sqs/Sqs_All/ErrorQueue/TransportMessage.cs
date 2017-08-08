namespace SqsAll.ErrorQueue
{
    using System.Collections.Generic;

    class TransportMessage
    {
        public Dictionary<string, string> Headers { get; set; }

        public string Body { get; set; }

        public string S3BodyKey { get; set; }
    }
}