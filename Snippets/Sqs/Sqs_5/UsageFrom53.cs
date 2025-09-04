using System;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using NServiceBus;
using NServiceBus.Pipeline;

class UsageFrom53
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