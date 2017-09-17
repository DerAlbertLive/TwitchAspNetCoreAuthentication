namespace OpenId
{
    public class OpenIdConfiguration
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ResponseType { get; set; }
        public string Scopes { get; set; }
        public string DisplayName { get; set; }
    }
}