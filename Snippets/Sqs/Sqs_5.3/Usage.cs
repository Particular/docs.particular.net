using System;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using NServiceBus;
using NServiceBus.Pipeline;

class Usage
{
    void AssumePermissionsOnPolicy(TransportExtensions<SqsTransport> transport)
    {
        #region assume-permissions

        var policies = transport.Policies();
        policies.AssumePolicyHasAppropriatePermissions();

        #endregion
    }

    void WildcardsAccount(TransportExtensions<SqsTransport> transport)
    {
        #region wildcard-account-condition

        var policies = transport.Policies();
        policies.AddAccountCondition();

        #endregion
    }

    void WildcardsPrefix(TransportExtensions<SqsTransport> transport)
    {
        #region wildcard-prefix-condition

        var policies = transport.Policies();
        policies.AddTopicNamePrefixCondition();

        #endregion
    }

    void WildcardsNamespace(TransportExtensions<SqsTransport> transport)
    {
        #region wildcard-namespace-condition

        var policies = transport.Policies();
        policies.AddNamespaceCondition("Sales.");
        policies.AddNamespaceCondition("Shipping.HighValueOrders.");

        #endregion
    }
}

#region sqs-access-to-native-message
class AccessToAmazonSqsNativeMessage : Behavior<IIncomingContext>
{
    public override Task Invoke(IIncomingContext context, Func<Task> next)
    {
        // get the native Amazon SQS message
        var message = context.Extensions.Get<Message>();

        //do something useful

        return next();
    }
}
#endregion