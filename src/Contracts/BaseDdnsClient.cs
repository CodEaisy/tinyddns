using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TinyDdns.Contracts
{
    /// <summary>
    /// base DDNS client
    /// </summary>
    public abstract class BaseDdnsClient : IDdnsClient
    {
        private readonly DdnsOptions _options;
        protected readonly ILogger<BaseDdnsClient> Logger;

        /// <summary>
        /// create base client
        /// </summary>
        /// <param name="logger"></param>
        protected BaseDdnsClient(IOptions<DdnsOptions> options, ILogger<BaseDdnsClient> logger)
        {
            _options = options.Value;
            Logger = logger;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateDdns(string ip, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(_options.Domains))
            {
                var hosts = _options.Domains.Split(',');
                var updated = 0;
                if (hosts.Any())
                {
                    await foreach(var host in UpdateDdns(hosts, ip, cancellationToken))
                    {
                        Logger.LogInformation("Updated records for host: {0}", host);
                        updated++;
                    }
                }
                return updated == hosts.Length;
            }

            return true;
        }

        private async IAsyncEnumerable<string> UpdateDdns(string[] hosts, string ip,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach(var host in hosts)
            {
                var done = false;
                try
                {
                    await UpdateDdns(host, ip, cancellationToken);
                    done = true;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "failed to update record for host: {0}", host);
                }

                if (done) yield return host;
            }
        }

        /// <inheritdoc />
        public abstract Task UpdateDdns(string host, string ip, CancellationToken cancellationToken = default);
    }
}
