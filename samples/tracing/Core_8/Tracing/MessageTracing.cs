using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus.Features;
using NServiceBus.Pipeline;

namespace Tracing
{
    public class MessageTracing : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            EnableByDefault();

            context.Pipeline.Register(typeof(TraceIncomingMessagesBehavior), "Traces incoming messages.");
            context.Pipeline.Register(typeof(TraceOutgoingMessageBehavior), "Traces outgoing messages.");
        }

        public static string ReceiveMessage = "NServiceBus.Receive";
        public static string SendMessage = "NServiceBus.Send";
        
        public static string ParentActivityIdHeaderName = "NServiceBus.Tracing.ParentActivityId";
    }

    public class TraceOutgoingMessageBehavior : Behavior<IOutgoingPhysicalMessageContext>
    {

        public override async Task Invoke(IOutgoingPhysicalMessageContext context, Func<Task> next)
        {
            Activity activity = null;
            var success = true;

            #region ActivityCreation

            if (diagnosticSource.IsEnabled(MessageTracing.SendMessage))
            {
                activity = new Activity(MessageTracing.SendMessage);

                foreach (var (key, value) in context.Headers)
                {
                    activity.AddTag(key, value);
                }

                diagnosticSource.StartActivity(activity, context.Headers);

                // This must happen after StartActivity to ensure the Id is initialized
                context.Headers.Add(MessageTracing.ParentActivityIdHeaderName, activity.Id);
            }

            #endregion

            try
            {
                await next();
            }
            catch
            {
                success = false;

                throw;
            }
            finally
            {
                if (activity != null)
                {
                    diagnosticSource.StopActivity(activity, success);
                }
            }
        }

        #region DiagnosticSourceDefinition

        static DiagnosticSource diagnosticSource = new DiagnosticListener(MessageTracing.SendMessage);

        #endregion
    }
    public class TraceIncomingMessagesBehavior : Behavior<IIncomingPhysicalMessageContext>
    {
        public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            Activity activity = null;
            var success = true;

            if (diagnosticSource.IsEnabled(MessageTracing.ReceiveMessage))
            {
                activity = new Activity(MessageTracing.ReceiveMessage);

                if (context.MessageHeaders.TryGetValue(MessageTracing.ParentActivityIdHeaderName, out var activityId))
                {
                    activity.SetParentId(activityId);
                }

                foreach (var (key, value) in context.MessageHeaders)
                {
                    activity.AddTag(key, value);
                }

                diagnosticSource.StartActivity(activity, context.MessageHeaders);
            }

            try
            {
                await next();
            }
            catch
            {
                success = false;
                
                throw;
            }
            finally
            {
                if (activity != null)
                {
                    diagnosticSource.StopActivity(activity, success);
                }
            }
        }

        static DiagnosticSource diagnosticSource = new DiagnosticListener(MessageTracing.ReceiveMessage);
    }
}