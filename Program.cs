using System;
using System.Diagnostics;

namespace HttpsTrace
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a URL as a command-line argument.");
                Console.WriteLine("Example: dotnet run https://example.com");
                return;
            }

            var url = args[0];
            using var listener2 = new HttpEventListener();

            using var client = new HttpClient();

            try
            {
                var response = await client.GetAsync(url);
                Console.WriteLine("Response received:");
                // Process the response as needed
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
            var exit = Console.ReadLine();
        }
    }
}

