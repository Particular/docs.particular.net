using Elsa.Builders;
using Elsa.Services.Models;

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NServiceBus.Activities
{
    public static class NServiceBusBuilderExtensions
    {
        public static IActivityBuilder SendNServiceBusMessage(this IBuilder builder, Action<ISetupActivity<SendNServiceBusMessage>> setup = default, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string sourceFile = null)
        {
            return builder.Then(setup, null, lineNumber, sourceFile);
        }

        public static IActivityBuilder ReceiveNServiceBusMessage(this IBuilder builder, Action<ISetupActivity<NServiceBusMessageReceived>> setup = default, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string sourceFile = null)
        {
            return builder.Then(setup, null, lineNumber, sourceFile);
        }

        public static IActivityBuilder PublishNServiceBusEvent(this IBuilder builder, Action<ISetupActivity<PublishNServiceBusEvent>> setup = default, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string sourceFile = null)
        {
            return builder.Then(setup, null, lineNumber, sourceFile);
        }

        public static IActivityBuilder PublishNServiceBusEvent(this IBuilder builder, object eventToPublish, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string sourceFile = null)
        {
            return builder.PublishNServiceBusEvent(setup => setup.Set("Message", context => ValueTask.FromResult(eventToPublish)), lineNumber, sourceFile);
        }

        public static IActivityBuilder PublishNServiceBusEvent<T>(this IBuilder builder, Func<ActivityExecutionContext, T> value, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string sourceFile = null)
        {
            return builder.PublishNServiceBusEvent(setup => setup.Set("Message", context => ValueTask.FromResult(value(context))), lineNumber, sourceFile);
        }


        public static IActivityBuilder SendNServiceBusMessage(this IBuilder builder, object messageToSend, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string sourceFile = null)
        {
            return builder.SendNServiceBusMessage(setup => setup.Set("Message", context => ValueTask.FromResult(messageToSend)), lineNumber, sourceFile);
        }

        public static IActivityBuilder ReceiveNServiceBusMessage(this IBuilder builder, Type messageType, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string sourceFile = null)
        {
            return builder.ReceiveNServiceBusMessage(setup => setup.Set("MessageType", context => ValueTask.FromResult(messageType)), lineNumber, sourceFile);
        }
    }
}
