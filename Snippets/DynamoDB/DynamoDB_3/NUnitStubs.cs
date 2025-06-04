using Amazon.DynamoDBv2.Model;

namespace DynamoDB_3
{
    internal class Assert
    {
        internal static void That(IReadOnlyCollection<TransactWriteItem> transactWriteItems, object value)
        {
            return;
        }
    }
    internal class Has
    {
        public static Count Count { get; internal set; }
    }

    internal class Count
    {
        internal bool EqualTo(int v)
        {
            return true;
        }
    }

    internal class TestAttribute : Attribute
    {
    }
}