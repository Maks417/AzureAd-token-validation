namespace AzureAdJWT.Models
{
    public class Credentials
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string Scope { get; set; }
        public string Secret { get; set; }
    }
}
