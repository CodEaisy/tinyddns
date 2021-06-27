using System.Threading;
using System.Threading.Tasks;

namespace TinyDdns.Contracts
{
    /// <summary>
    /// DDNS client interface
    /// </summary>
    public interface IDdnsClient
    {
        /// <summary>
        /// update ddns record
        /// </summary>
        /// <param name="ip"></param>
        /// <returns><see cref="Task{Bool}"/></returns>
        Task<bool> UpdateDdns(string ip, CancellationToken cancellationToken = default);
    }
}
