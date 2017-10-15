using System.Collections.Concurrent;
using System.Net;

namespace QueryMultiDb
{
    public class DnsResolverWithCache
    {
        private readonly ConcurrentDictionary<string,IPAddress> _ipAddressCache;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Suppreses beforefieldinit in static singleton.")]
        static DnsResolverWithCache()
        {
        }

        private DnsResolverWithCache()
        {
            _ipAddressCache = new ConcurrentDictionary<string, IPAddress>();
        }

        public static DnsResolverWithCache Instance { get; } = new DnsResolverWithCache();

        public IPAddress Resolve(string hostName)
        {
            var ipAdress = _ipAddressCache.GetOrAdd(hostName, InternalResolve);
            
            return ipAdress;
        }
        
        private static IPAddress InternalResolve(string hostName)
        {
            Logger.Instance.Info($"Resolving host '{hostName}' not in cache.");
            var hostEntry = Dns.GetHostEntry(hostName);
            var addressCount = hostEntry.AddressList.Length;
            Logger.Instance.Info($"Found {addressCount} address for host.");

            return addressCount > 0 ? hostEntry.AddressList[0] : null;
        }
    }
}
