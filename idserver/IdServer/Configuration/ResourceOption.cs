using System.Collections.Generic;

namespace IdServer.Configuration
{
    public abstract class ResourceOption
    {
        public bool Enabled { get; set; } = true;

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public ICollection<string> UserClaims { get; set; } = new HashSet<string>();
    }
}