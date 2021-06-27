using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TinyDdns.Contracts;

namespace TinyDdns
{
    /// <summary>
    /// DDNS update worker
    /// </summary>
    public class DdnsWorker : BackgroundService
    {
        private readonly ILogger<DdnsWorker> _logger;
        private readonly IDdnsClient _ddnsClient;
        private readonly DdnsOptions _options;
        private readonly HttpClient _httpClient;
        private string _lastIpAddress;

        public DdnsWorker(IDdnsClient ddnsClient, IOptions<DdnsOptions> options,
            HttpClient httpClient, ILogger<DdnsWorker> logger)
        {
            _ddnsClient = ddnsClient;
            _options = options.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// run update continuously
        /// </summary>
        /// <param name="stoppingToken"></param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested
                && !string.IsNullOrEmpty(_options.Domains))
            {
                _logger.LogInformation("DDNS Update Client running at: {time}", DateTimeOffset.Now);
                var currentIp = await GetCurrentIp();
                if (!string.IsNullOrEmpty(currentIp) && currentIp != _lastIpAddress)
                {
                    _logger.LogInformation("Updating DNS records for {0}", _options.Provider);
                    await UpdateDnsRecord(currentIp);
                    _lastIpAddress = currentIp;
                }
                await Task.Delay(1000 * _options.CheckInterval, stoppingToken);
            }
        }

        /// <summary>
        /// get current router public IP address
        /// </summary>
        /// <returns><see cref="Task{String}"/></returns>
        private async Task<string> GetCurrentIp()
        {
            const string ICANHAZIPURL = "https://ipv4.icanhazip.com";
            const string IPIFYURL = "https://api.ipify.org";
            var ip = string.Empty;
            try
            {
                ip = await _httpClient.GetStringAsync(ICANHAZIPURL);
            } catch (Exception ex)
            {
                _logger.LogError(ex, "failed to get IP from ICANHAZIP");
                try
                {
                    ip = await _httpClient.GetStringAsync(IPIFYURL);
                }
                catch (Exception exx)
                {
                    _logger.LogError(exx, "faied to get IP from IPIFY");
                }
            }

            return ip.Trim();
        }

        /// <summary>
        /// update DNS record with new ip
        /// </summary>
        /// <param name="ip"></param>
        private async Task UpdateDnsRecord(string ip)
        {
            _logger.LogInformation("updating DDNS records for {0}", _options.Provider);
            var result = await _ddnsClient.UpdateDdns(ip);
            if (result) _logger.LogInformation("DDNS records updated for {0}", _options.Provider);
        }
    }
}
