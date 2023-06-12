namespace Core7.Conventions
{
    using System;
    using NServiceBus;

    #region message-conventions-class

    class MyNamespaceMessageConvention : IMessageConvention
    {
        public bool IsMessageType(Type type) => type.Namespace == "MyNamespace.Messages";
        public bool IsCommandType(Type type) => type.Namespace == "MyNamespace.Messages.Events";
        public bool IsEventType(Type type) => type.Namespace == "MyNamespace.Messages.Commands";
        public string Name { get; } = "MyNamespace message convention";
    }

    #endregion

    public class ApplyConventions
    {
        public static void ApplyConventionsClass(EndpointConfiguration endpointConfiguration)
        {
            #region message-conventions-class-installation
            endpointConfiguration.Conventions().Add(new MyNamespaceMessageConvention());
            #endregion
        }
    }
}
