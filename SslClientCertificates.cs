﻿using CommandLine;
using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace HttpsTrace
{
    class Options2
    {
        [Value(0, MetaName = "ServerName", Required = true, HelpText = "The Server address to process.")]
        public string ServerName { get; set; }

        [Option('c', "certificate", Required = false, HelpText = "Path to the PFX certificate file.")]
        public string CertificatePath { get; set; }

        [Option('p', "password", Required = false, HelpText = "Password for the PFX certificate.")]
        public string CertificatePassword { get; set; }
    }
    public class SslClientCertificates
    {
        static Options2? Options;
        static string htmlFilePath = "SslClientCertificates_output.html";
        static StreamWriter htmlWriter;

        public static void Main(string[] args)
        {
            using (htmlWriter = new StreamWriter(htmlFilePath))
            {
                WriteHtmlHeader();

                Parser.Default.ParseArguments<Options2>(args)
                .WithParsed(o =>
                {
                    Options = o;
                    WriteSection("Server Information", $"<p>Server: {o.ServerName}</p>");

                    if (!string.IsNullOrEmpty(o.CertificatePath))
                    {

                 
                        WriteSection("Client Certificate Path", $"<p>{o.CertificatePath}</p><p><textarea rows=\"15\" class=\"block p-2.5 w-full text-sm text-gray-900 bg-gray-50 rounded-lg border border-gray-300 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500\">{EncodePfxToBase64(o.CertificatePath)}</textarea></p>");

                        X509Certificate2 clientCert = new X509Certificate2(o.CertificatePath, o.CertificatePassword);

                      
                        WriteSection("Selected Client Certificate", $"<p>Subject:{clientCert.Subject}</p><p>Issuer:{clientCert.Issuer}</p>");

                    }

                   
                })
                .WithNotParsed(errors =>
                {
                    foreach (var error in errors)
                    {
                        WriteSection("Error", $"<p class='text-red-500'>{error}</p>");
                    }
                    return;
                });

           
                string hostname = Options.ServerName;
                int port = 443;
                WriteSection("Environment Variables",
                    $"<p>WEBSITE_LOAD_USER_PROFILE: {Environment.GetEnvironmentVariable("WEBSITE_LOAD_USER_PROFILE")}</p>" +
                    $"<p>WEBSITE_LOAD_ROOT_CERTIFICATES: {Environment.GetEnvironmentVariable("WEBSITE_LOAD_ROOT_CERTIFICATES")}</p>");

                try
                {
                    using (TcpClient client = new TcpClient(hostname, port))
                    {
                        using (SslStream sslStream = new SslStream(client.GetStream(), false, ValidateServerCertificate, SelectLocalCertificate))
                        {
                           
                            sslStream.AuthenticateAsClient(hostname);

                            X509Certificate serverCertificate = sslStream.RemoteCertificate;
                            if (serverCertificate != null)
                            {
                                WriteSection("Server Certificate", $"<p><b>Subject:</b> {serverCertificate.Subject}</p>" +
                                                              $"<p><b>Issuer:</b> {serverCertificate.Issuer}</p>" +
                                                              $"<p><b>Thumbprint:</b> {serverCertificate.GetCertHashString()}</p>");
                            }
                            else
                            {
                                WriteSection("Server Certificate", "<p class='text-red-500'>No server certificate received.</p>");
                            }
                            WriteSection("Client Certificate Authentication",
                               sslStream.IsMutuallyAuthenticated ? "<p class='text-green-500'>Client certificate was required and successfully authenticated.</p>" :
                               "<p>---------</p>");
                           
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteSection("Error", $"<p class='text-red-500'>{ex.Message}</p>");
                }

                WriteHtmlFooter();

                Console.WriteLine($"HTML report generated at: {Path.GetFullPath(htmlFilePath)}");

            }
        }

        private static void WriteHtmlHeader()
        {
            htmlWriter.WriteLine("<html><head><title>HTTPS Trace Report</title>");
         
            htmlWriter.WriteLine("<script src=\"https://cdn.tailwindcss.com\"></script>");
            htmlWriter.WriteLine("</head><body class='p-8 bg-gray-900 text-white'>");
            htmlWriter.WriteLine("<h1 class='text-3xl font-bold text-center mb-6 text-gray-200'>HTTPS Trace Report</h1>");
        }

        private static void WriteSection(string title, string content)
        {
            htmlWriter.WriteLine($"<section class='bg-gray-800 shadow-lg rounded-lg p-6 mb-6'>");
            htmlWriter.WriteLine($"<h2 class='text-xl font-semibold mb-3 text-gray-300'>{title}</h2>");
            htmlWriter.WriteLine(content);
            htmlWriter.WriteLine("</section>");
        }

        private static void WriteHtmlFooter()
        {
            htmlWriter.WriteLine("</body></html>");
        }

        private static bool ValidateServerCertificate(
            object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            WriteSection("Server Certificate Validation", $"<p>Server Certificate: {certificate?.Subject} tumpprint {certificate?.GetCertHashString()} </p>");
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }
            else
            {
                WriteSection("SSL Policy Errors", $"<p class='text-red-500'>{sslPolicyErrors}</p>");
                return false;
            }
        }

        private static X509Certificate SelectLocalCertificate(
            object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            //WriteSection("Client Certificate Selection", "<p>Server requested a client certificate.</p>");

            if (acceptableIssuers != null && acceptableIssuers.Length > 0)
            {
                string issuerList = string.Join("", Array.ConvertAll(acceptableIssuers, issuer => $"<li>{issuer}</li>"));
                WriteSection("Acceptable Issuers", $"<ul class='list-disc ml-6'>{issuerList}</ul>");
            }
            if (!string.IsNullOrEmpty(Options.CertificatePath))
            {
                string certPath = Options.CertificatePath;
                string certPassword = Options.CertificatePassword;

                try
                {
                    X509Certificate2 clientCert = new X509Certificate2(certPath, certPassword);
                    return clientCert;
                }
                catch (Exception ex)
                {
                    WriteSection("Error Loading Client Certificate", $"<p class='text-red-500'>{ex.Message}</p>");
                    return null;
                }
            }
            else
            {
               // WriteSection("Error", "<p class='text-red-500'>Certificate path is empty or null.</p>");
                return null;
            }
        }
        public static string EncodePfxToBase64(string pfxFilePath)
        {
            byte[] pfxBytes = File.ReadAllBytes(pfxFilePath);
            return Convert.ToBase64String(pfxBytes);
        }
    }
}
