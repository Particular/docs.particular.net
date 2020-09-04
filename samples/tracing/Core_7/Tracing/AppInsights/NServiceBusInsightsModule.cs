using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace Tracing.AppInsights
{
    class NServiceBusInsightsModule : ITelemetryModule, IDisposable, IObserver<DiagnosticListener>
    {
        TelemetryClient client;
        List<IDisposable> subscriptions = new List<IDisposable>();
        List<IDisposable> listeners = new List<IDisposable>();

        IDisposable allSubscriptions;

        public void Initialize(TelemetryConfiguration configuration)
        {
            client = new TelemetryClient(configuration);

            allSubscriptions = DiagnosticListener.AllListeners.Subscribe(this);
        }

        public void Dispose()
        {
            foreach (var subscription in subscriptions)
            {
                subscription.Dispose();
            }

            foreach (var listener in listeners)
            {
                listener.Dispose();
            }

            allSubscriptions.Dispose();
        }

        public void OnNext(DiagnosticListener value)
        {
            #region TracingListenerRegistration

            if (value.Name == MessageTracing.ReceiveMessage)
            {
                var listener = new NServiceBusTracingListener(client);
                var subscription = value.Subscribe(listener);

                subscriptions.Add(subscription);
                listeners.Add(listener);
            }
            #endregion
            else if (value.Name == MessageTracing.SendMessage)
            {
                var listener = new NServiceBusTracingListener(client);
                var subscription = value.Subscribe(listener);

                subscriptions.Add(subscription);
                listeners.Add(listener);
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