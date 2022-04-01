using Elsa.Builders;
using Elsa.Models;
using Elsa.Services;
using Elsa.Services.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NServiceBus.Pipeline;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBus.Activities
{
    public class CustomElsaHandlerTrigger
        : Behavior<IIncomingLogicalMessageContext>
    {
        private Func<IServiceProvider> _serviceProviderFactory;

        public CustomElsaHandlerTrigger(Func<IServiceProvider> serviceProviderFactory)
        {
            _serviceProviderFactory = serviceProviderFactory;
        }

        public async override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
        {
            Type messageType = context.Message.Instance.GetType();
            var serviceProvider = _serviceProviderFactory();

            if (serviceProvider == null)
            {
                await next();
                return;
            }

            using (var scope = serviceProvider.CreateScope())
            {
                var workflowRunner = scope.ServiceProvider.GetService<IWorkflowLaunchpad>();

                #region ElsaWorkflow
                if (workflowRunner != null)
                {
                    // This bookmark distinguishes between different message types
                    // for the same Activity
                    var bookmark = new MessageReceivedBookmark(
                        messageType.AssemblyQualifiedName);
                    var query = new WorkflowsQuery(
                        nameof(NServiceBusMessageReceived), bookmark);

                    // Find any workflows that are suspended or triggered by this
                    // event type and execute them in-process
                    // The message instance is passed into the workflow as "Input"
                    var workflowsFound = await workflowRunner
                        .CollectAndExecuteWorkflowsAsync(query,
                            new WorkflowInput( Input: context.Message.Instance));

                    // If workflows were found, do not continue the pipeline.
                    // In this case, there aren't any IHandleMessages implementations
                    // defined and NServiceBus will throw an exception if the
                    // pipeline continues.
                    if (workflowsFound.Any())
                    {
                        return;
                    }

                    // If no workflows are found, continue the pipeline as normal.
                    // This allows normal message processing if the handlers aren't
                    // defined at runtime by Elsa.
                    await next();
                }
                #endregion
                else
                {
                    // if anything is amiss, just continue the pipeline.
                    await next();
                }
            }
        }
    }
}

