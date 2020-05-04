using System;
using System.Collections.Concurrent;

namespace NServiceBus
{
    class SessionProvider : ISessionProvider
    {
        public IMessageSession GetSession(string endpointName)
        {
            if (sessions.TryGetValue(endpointName, out var session))
            {
                return session;
            }

            throw new InvalidOperationException($"No session found with name '{endpointName}'");
        }

        public IDisposable Manage(IMessageSession messageSession, string endpointName)
        {
            return new Scope(messageSession, endpointName, sessions);
        }

        sealed class Scope : IDisposable
        {
            private readonly string endpointName;
            private ConcurrentDictionary<string, IMessageSession> sessions;

            public Scope(IMessageSession session, string endpointName, ConcurrentDictionary<string, IMessageSession> sessions)
            {
                this.sessions = sessions;
                this.endpointName = endpointName;

                this.sessions.TryAdd(endpointName, session);
            }

            public void Dispose()
            {
                sessions.TryRemove(endpointName, out _);
            }
        }

        private ConcurrentDictionary<string, IMessageSession> sessions = new ConcurrentDictionary<string, IMessageSession>();
    }
}