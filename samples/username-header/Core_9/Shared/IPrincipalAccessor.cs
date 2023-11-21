using System.Security.Principal;

public interface IPrincipalAccessor
{
    IPrincipal CurrentPrincipal { get; set; }
}