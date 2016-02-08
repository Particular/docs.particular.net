namespace Snippets5.RavenDB
{
    using System;
    using NServiceBus;
    using Raven.Client.Document;

    class ChangeResourceManagerID
    {
        public ChangeResourceManagerID()
        {
            // Do not include ChangeResourceManagerID region. Core V6 must use 
            // Raven V4 which will not include the issue that necessitates this workaround.
        }
    }
}
