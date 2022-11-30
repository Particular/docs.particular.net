using System.Threading.Tasks;

namespace Core8.Recoverability
{
    using System;
    using NServiceBus;
    using NServiceBus.Settings;

    public class UnrecoverableExceptions
    {
        void Configure(EndpointConfiguration endpointConfiguration)
        {
            #region UnrecoverableExceptions

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.AddUnrecoverableException<ValidationException>();
            recoverability.AddUnrecoverableException(typeof(ArgumentException));

            #endregion
        }

        void Configure(SettingsHolder settings)
        {
            #region UnrecoverableExceptionsSettings

            settings.AddUnrecoverableException(typeof(ValidationException));

            #endregion
        }

        class ValidationException : Exception
        {
        }

        class SomeMessage : IMessage
        {
        }

        class SomeMessageHandler : IHandleMessages<SomeMessage>
        {
            #region ThrowUnrecoverableException
            public Task Handle(SomeMessage message, IMessageHandlerContext context)
            {
                throw new UnrecoverableException("Something went wrong, don't retry");
            }
            #endregion
        }
    }
}
