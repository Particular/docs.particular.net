using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Tracing.AppInsights
{
    class NServiceBusTracingListener : IObserver<KeyValuePair<string, object>>, IDisposable
    {
        TelemetryClient client;
        ConcurrentDictionary<string, IOperationHolder<DependencyTelemetry>> inProgressOperations;

        public NServiceBusTracingListener(TelemetryClient client)
        {
            this.client = client;

            inProgressOperations = new ConcurrentDictionary<string, IOperationHolder<DependencyTelemetry>>();
        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            #region ActivityProcessing

            var key = value.Key;
            var activity = Activity.Current;

            if (key.EndsWith("Start"))
            {
                var operation = client.StartOperation<DependencyTelemetry>(activity);
                operation.Telemetry.Type = "NServiceBus";

                inProgressOperations.AddOrUpdate(activity.Id, _ => operation, (_, __) => operation);

            }
            else if (key.EndsWith("Stop"))
            {
                if (inProgressOperations.TryRemove(activity.Id, out var operation))
                {
                    operation.Telemetry.Success = (bool) value.Value;

                    client.StopOperation(operation);

                    operation.Dispose();
                }
            }

            #endregion
        }

        public void Dispose()
        {
            foreach (var operation in inProgressOperations)
            {
                operation.Value.Dispose();
            }
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }
    }
}