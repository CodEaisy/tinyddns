using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TinyDdns.Contracts;
using TinyDdns.Implementations;
using TinyDdns.Implementations.Namecheap;

namespace TinyDdns
{
    /// <summary>
    /// main entry point
    /// </summary>
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<DdnsOptions>(hostContext.Configuration.GetSection("DdnsOptions"));
                    var ddnsProviderType = GetProvider(hostContext.Configuration.GetValue<DdnsProviders>("DdnsOptions:Provider"));
                    services.AddSingleton(typeof(IDdnsClient), ddnsProviderType);
                    services.AddNamecheapDdns(hostContext.Configuration);
                    services.AddHostedService<DdnsWorker>();
                });

        public static Type GetProvider(DdnsProviders provider) =>
            provider switch {
                DdnsProviders.NameCheap => typeof(NamecheapClient),
                _ => throw new NotSupportedException($"provider type {provider} not supported")
            };
    }
}
