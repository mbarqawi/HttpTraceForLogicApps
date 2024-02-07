using CommandLine;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace HttpsTrace
{
    class Options
    {
        [Value(0, MetaName = "url", Required = true, HelpText = "The URL to process.")]
        public string Url { get; set; }

        [Option('c', "certificate", Required = false, HelpText = "Path to the PFX certificate file.")]
        public string CertificatePath { get; set; }

        [Option('p', "password", Required = false, HelpText = "Password for the PFX certificate.")]
        public string CertificatePassword { get; set; }
    }
    internal class Program
    {
        static Options Options;
        static async Task Main(string[] args)
        {

            Parser.Default.ParseArguments<Options>(args)
           .WithParsed(o =>
           {
               Options = o;
               Console.WriteLine($"URL: {o.Url}");

               if (!string.IsNullOrEmpty(o.CertificatePath))
               {
                   Console.WriteLine($"Certificate Path: {o.CertificatePath}");
               }

               if (!string.IsNullOrEmpty(o.CertificatePassword))
               {
                   Console.WriteLine($"Certificate Password: {o.CertificatePassword}");
               }
           }).WithNotParsed(errors =>
           {
               foreach (var error in errors)
               {
                   Console.WriteLine($"Error: {error}");
               }
               return;
           });
            ;

          

            using var listener2 = new HttpEventListener();

            using var handler = new HttpClientHandler();

            
            if (Options.CertificatePath!=null)
            {
             
                handler.ClientCertificates.Add(new X509Certificate2(Options.CertificatePath, Options.CertificatePassword));
            }

            using var client = new HttpClient(handler);

            try
            {
                var response = await client.GetAsync(Options.Url);
                Console.WriteLine("\r\n\r\n\r\n⬇️⬇️Response received:⬇️⬇️\r\n\r\n\r\n");
                 var content = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"status code { response.StatusCode}");
                 
                    Console.WriteLine(content);
                
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
            var exit = Console.ReadLine();
        }
       

    }
}

