using System.Security.Principal;

namespace Steganografia.Security.Entities
{
    public class AppPrincipal : GenericPrincipal, IPrincipal
    {
        public AppPrincipal(AppIdentity identity) : base(identity, new string[] { })
        {
        }

        public new AppIdentity Identity => base.Identity as AppIdentity;

    }
}