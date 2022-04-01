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
        private Func<IServiceProvider?> _serviceProviderFactory;

        public CustomElsaHandlerTrigger(Func<IServiceProvider?> serviceProviderFactory)
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

                if (workflowRunner != null)
                {
                    // Bookmarks are used by Elsa as a "Marker" for suspended/triggered workflows.  
                    // This one here is used to distinguish between the same Activity for different message types.
                    // TODO: Check that multiple message types for the same endpoint works.
                    // TODO: Check that a "Normal" handler for yet another message type still works.
                    var bookmark = new MessageReceivedBookmark(messageType.AssemblyQualifiedName);
                    var query = new WorkflowsQuery(nameof(NServiceBusMessageReceived), bookmark);

                    // This line finds any workflows that are suspended or triggered by this event type and executes them inproc.
                    // The message instance is passed into the workflow as "Input"
                    var workflowsFound = await workflowRunner.CollectAndExecuteWorkflowsAsync(query, new WorkflowInput { Input = context.Message.Instance });

                    // If workflows were found, do not continue the pipeline.
                    // In the case of this POC there aren't any IHandleMessages implementations defined and
                    // NSB to throw if the pipeline continues.
                    if (workflowsFound.Any())
                    {
                        return;
                    }

                    // if no workflows are found, continue the pipeline as normal.
                    // This should allow normal message processing if the handlers aren't defined at runtime by Elsa.
                    await next();
                }
                else
                {
                    // if anything is amyss, just continue the pipeline.
                    await next();
                }
            }
        }
    }
}

