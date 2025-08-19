using System.Security.Principal;
using System.Threading;

public class PrincipalAccessor : IPrincipalAccessor
{
    AsyncLocal<CurrentPrincipalHolder> principalCurrent = new AsyncLocal<CurrentPrincipalHolder>();

    public IPrincipal CurrentPrincipal
    {
        get
        {
            return principalCurrent.Value?.CurrentPrincipal;
        }
        set
        {
            var holder = principalCurrent.Value;
            if (holder != null)
            {
                // Clear current IPrincipal trapped in the AsyncLocals, as its done.
                holder.CurrentPrincipal = null;
            }

            if (value != null)
            {
                // Use an object indirection to hold the IPrincipal in the AsyncLocal,
                // so it can be cleared in all ExecutionContexts when its cleared.
                principalCurrent.Value = new CurrentPrincipalHolder { CurrentPrincipal = value };
            }
        }
    }

    class CurrentPrincipalHolder
    {
        public IPrincipal CurrentPrincipal;
    }
}
