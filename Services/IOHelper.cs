using AzureAdJWT.Models;
using System;

namespace AzureAdJWT.Services
{
    public static class IOHelper
    {
        public static Credentials GetCredentialsFromInput()
        {
            var creds = new Credentials
            {
                TenantId = ValidatedInput("Enter Tenant ID:", 36),
                ClientId = ValidatedInput("Enter Client ID:", 36),
                Scope = $"{ValidatedInput("Enter scope (e.g. 'api://{scope}'):")}/.default",
                Secret = ValidatedInput("Enter secret:", 34)
            };

            Console.WriteLine(Environment.NewLine);

            return creds;
        }

        public static void PrettyPrint(string title, string body, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;

            Console.WriteLine(title);
            Console.ResetColor();

            Console.WriteLine(body);
        }

        private static string ValidatedInput(string message, int minLength = 0)
        {
            Console.WriteLine(message);
            var input = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(input) 
                || (minLength != 0 && input.Length < minLength))
            {
                PrettyPrint("Invalid format. Please re-enter correct value", string.Empty, ConsoleColor.Red);
                ValidatedInput(message, minLength);
            }

            return input;
        } 
    }
}
