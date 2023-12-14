using System.Diagnostics;

namespace HttpsTrace
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!"); 
          //  var listener = new HttpEventListener(_output);


            using var listener2 = new HttpEventListener();

            using var client = new HttpClient();

            while (true)
            {
                try
                {
                    // Ask the user to enter a URL
                    Console.Write("Please enter a URL: ");
                    var url = Console.ReadLine();

                    // Example: Send a GET request to a URL
                    var response = await client.GetAsync(url);
                    Console.WriteLine("Response received:");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occurred: {e.Message}");
                }

                // Ask the user if they want to exit
                Console.Write("Do you want to exit? (y/n): ");
                var exit = Console.ReadLine();
                if (exit.ToLower() == "y")
                {
                    break;
                }
            }
        }
    }

    // Keep the listener around while you want the logging to continue, dispose it after.

}
