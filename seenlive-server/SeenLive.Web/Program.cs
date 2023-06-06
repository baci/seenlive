using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SeenLive.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseKestrel(options =>
                     {
                         options.Listen(IPAddress.Loopback, 80);
                         
                         string? certPath = Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Path");
                         string? certPwd = Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Password");
                             
                         if(certPath != null && certPwd != null)
                         {
                             options.Listen(IPAddress.Loopback, 443, listenOptions =>
                             {
                                 listenOptions.UseHttps(certPath, certPwd);
                             });
                         }
                     });              
                })
                .ConfigureAppConfiguration(
                    (hostContext, builder) =>
                    {
                        builder.SetBasePath(Directory.GetCurrentDirectory());
                        builder.AddJsonFile("appsettings.json", optional: true);
                        builder.AddJsonFile(path: $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        {
                            builder.AddJsonFile("appsettings.linux.json", optional: true);
                        }
                    })
            ;
    }
}
