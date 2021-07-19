using System;
using System.Text;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter Tenant ID:");
            var tenantId = Console.ReadLine();

            Console.WriteLine("Enter Client ID:");
            var clientId = Console.ReadLine();

            Console.WriteLine("Enter scope (like 'api://{scope}/.default'):");
            var scope = Console.ReadLine();

            Console.WriteLine("Enter secret:");
            var secret = Console.ReadLine();
            Console.WriteLine(Environment.NewLine);


            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token"),
                Method = HttpMethod.Post
            };

            var content = new MultipartFormDataContent
            {
                { new StringContent("client_credentials"), "grant_type" },
                { new StringContent(clientId), "client_id" },
                { new StringContent(scope), "scope" },
                { new StringContent(secret), "client_secret" }
            };
            request.Content = content;

            var response = client.SendAsync(request).Result;
            var result = response.Content.ReadAsStringAsync();

            Console.WriteLine("Bearer token");
            Console.WriteLine(result.Result);
            Console.WriteLine(Environment.NewLine);

            var token = result.Result.Split(':').Last().Trim('}').Trim('"');

            var issuer = string.Format(CultureInfo.InvariantCulture, "https://login.microsoftonline.com/{0}/v2.0", tenantId);
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            var stsDiscoveryEndpoint = string.Format(CultureInfo.InvariantCulture, "https://login.microsoftonline.com/{0}/.well-known/openid-configuration", tenantId);

            var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(stsDiscoveryEndpoint, new OpenIdConnectConfigurationRetriever());
            var config = configManager.GetConfigurationAsync().Result;

            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidAudience = clientId,
                ValidIssuer = issuer,
                IssuerSigningKeys = config.SigningKeys,
                ValidateLifetime = false,
                IssuerSigningKey = securityKey
            };

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("Token is valid");
                Console.ResetColor();

                Console.WriteLine(validatedToken);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine("Invalid token");
                Console.WriteLine(ex.Message);
            }
            

            Console.ReadLine();
        }
    }
}
