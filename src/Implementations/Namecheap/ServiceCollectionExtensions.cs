
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TinyDdns.Implementations.Namecheap
{
    /// <summary>
    /// service collection extensions for namecheap support
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// add Namecheap DDNS options
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddNamecheapDdns(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<NamecheapOptions>(configuration.GetSection("NamecheapOptions"));
            services.AddHttpClient<NamecheapClient>();
            return services;
        }
    }
}
