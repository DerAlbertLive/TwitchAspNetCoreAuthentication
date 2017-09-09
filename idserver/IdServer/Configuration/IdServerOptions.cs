namespace IdServer.Configuration
{
    public class IdServerOptions
    {
        public IdServerOptions()
        {
            Signing=new SigningOptions();
        }
        
        public SigningOptions  Signing { get; set; }
    }

    public class SigningOptions
    {
        public string Thumbprint { get; set; }
    }
}