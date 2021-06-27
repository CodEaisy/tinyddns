
namespace TinyDdns.Implementations.Namecheap
{
    /// <summary>
    /// namecheap configuration options
    /// </summary>
    public class NamecheapOptions
    {
        /// <summary>
        /// TLD domain
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// name cheap DDNS password
        /// </summary>
        public string Password { get; set; }
    }
}
