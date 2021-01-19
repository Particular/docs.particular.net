using NServiceBus;

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