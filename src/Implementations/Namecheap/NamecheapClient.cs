using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TinyDdns.Contracts;

namespace TinyDdns.Implementations.Namecheap
{
    /// <summary>
    /// default implement for namecheap DDNS service
    /// </summary>
    public class NamecheapClient : BaseDdnsClient
    {
        private readonly NamecheapOptions _options;
        private readonly HttpClient _httpClient;
        private const string URL = "https://dynamicdns.park-your-domain.com/update?host={0}&domain={1}&password={2}&ip={3}";

        /// <summary>
        /// create an instance of <see cref="NamecheapClient"/>
        /// </summary>
        /// <param name="httpClient"></param>
        public NamecheapClient(HttpClient httpClient, IOptions<NamecheapOptions> options,
            IOptions<DdnsOptions> ddnsOptions, ILogger<NamecheapClient> logger)
            : base(ddnsOptions, logger)
        {
            _options = options.Value;
            _httpClient = httpClient;
        }

        /// <summary>
        /// update dns for given host with ip
        /// </summary>
        /// <param name="host"></param>
        /// <param name="ip"></param>
        /// <param name="cancellationToken"></param>
        public async override Task UpdateDdns(string host, string ip,
            CancellationToken cancellationToken = default)
        {
            var cleanHost = CleanHost(host, _options.Domain);
            var url = string.Format(URL, cleanHost, _options.Domain, _options.Password, ip);
            await _httpClient.GetStringAsync(url, cancellationToken);
        }

        private static string CleanHost(string host, string domain) =>
            host.Replace($".{domain}", string.Empty);
    }
}
