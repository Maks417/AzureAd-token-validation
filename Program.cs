using System;
using AzureAdJWT.Services;

namespace AzureAdJWT
{
    class Program
    {
        static void Main(string[] args)
        {
            var creds = IOHelper.GetCredentialsFromInput();
            var bearerToken = TokenService.RetrieveBearerToken(creds);

            try
            {
                TokenService.ValidateToken(creds, bearerToken,out var validatedToken);
                IOHelper.PrettyPrint("Token is valid", validatedToken.ToString(), ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                IOHelper.PrettyPrint("Invalid token", ex.Message, ConsoleColor.Red);
            }

            Console.ReadLine();
        }
    }
}
