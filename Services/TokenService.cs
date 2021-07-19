using AzureAdJWT.Models;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace AzureAdJWT.Services
{
    public static class TokenService
    {
        public static string RetrieveBearerToken(Credentials creds)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://login.microsoftonline.com/{creds.TenantId}/oauth2/v2.0/token"),
                Method = HttpMethod.Post
            };

            var content = new MultipartFormDataContent
            {
                { new StringContent("client_credentials"), "grant_type" },
                { new StringContent(creds.ClientId), "client_id" },
                { new StringContent(creds.Scope), "scope" },
                { new StringContent(creds.Secret), "client_secret" }
            };
            request.Content = content;

            var response = client.SendAsync(request).Result;
            var result = response.Content.ReadAsStringAsync().Result;

            IOHelper.PrettyPrint("Bearer token", result, ConsoleColor.Blue);

            return result.Split(':').Last().Trim('}').Trim('"');
        }

        public static void ValidateToken(Credentials creds, string token, out SecurityToken validatedToken)
        {
            validatedToken = null;

            var issuer = string.Format(CultureInfo.InvariantCulture, "https://login.microsoftonline.com/{0}/v2.0", creds.TenantId);
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(creds.Secret));
            var stsDiscoveryEndpoint = string.Format(CultureInfo.InvariantCulture, "https://login.microsoftonline.com/{0}/.well-known/openid-configuration", creds.TenantId);

            var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(stsDiscoveryEndpoint, new OpenIdConnectConfigurationRetriever());
            var config = configManager.GetConfigurationAsync().Result;

            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidAudience = creds.ClientId,
                ValidIssuer = issuer,
                IssuerSigningKeys = config.SigningKeys,
                ValidateLifetime = false,
                IssuerSigningKey = securityKey
            };

            tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        }
    }
}
