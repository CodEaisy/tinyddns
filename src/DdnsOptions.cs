
namespace TinyDdns
{
    /// <summary>
    /// DDNS options
    /// </summary>
    public class DdnsOptions
    {
        /// <summary>
        /// default check interval
        /// </summary>
        public readonly int DefaultCheckInterval = 30;
        private int? _checkInterval;

        /// <summary>
        /// configured check interval
        /// </summary>
        public int CheckInterval {
            get => _checkInterval ?? DefaultCheckInterval;
            set => _checkInterval = value;
        }

        /// <summary>
        /// configured DDNS provider
        /// </summary>
        public DdnsProviders Provider { get; set; }

        /// <summary>
        /// comma-seperated list of domains
        /// </summary>
        public string Domains { get; set; }
    }
}
