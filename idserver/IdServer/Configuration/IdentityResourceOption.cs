namespace IdServer.Configuration
{
    public class IdentityResourceOption : ResourceOption
    {
        public bool Required { get; set; }

        public bool Emphasize { get; set; }

        public bool ShowInDiscoveryDocument { get; set; } = false;
    }
}