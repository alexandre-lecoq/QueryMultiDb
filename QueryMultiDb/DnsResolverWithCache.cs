using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using NLog;

namespace QueryMultiDb
{
    public class DnsResolverWithCache
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
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
            if (string.IsNullOrWhiteSpace(hostName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(hostName));
            }

            var ipAddress = _ipAddressCache.GetOrAdd(hostName, InternalResolve);
            
            return ipAddress;
        }
        
        private static IPAddress InternalResolve(string hostName)
        {
            if (string.IsNullOrWhiteSpace(hostName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(hostName));
            }

            Logger.Trace($"Resolving host '{hostName}' not in cache.");

            if (IPAddress.TryParse(hostName, out var address))
            {
                return address;
            }

            return LocalHostResolve(hostName) ?? DnsResolve(hostName);
        }

        private static IPAddress LocalHostResolve(string hostName)
        {
            if (string.IsNullOrWhiteSpace(hostName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(hostName));
            }

            var trimmedName = hostName.Trim();

            if (trimmedName == ".")
                return IPAddress.Loopback;

            if (trimmedName == "localhost")
                return IPAddress.Loopback;

            if (trimmedName == Dns.GetHostName())
                return IPAddress.Loopback;

            return null;
        }

        private static IPAddress DnsResolve(string hostName)
        {
            if (string.IsNullOrWhiteSpace(hostName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(hostName));
            }

            try
            {
                var hostEntry = Dns.GetHostEntry(hostName);
                var addressCount = hostEntry.AddressList.Length;
                Logger.Trace($"Found {addressCount} address for host.");

                return addressCount > 0 ? hostEntry.AddressList[0] : null;
            }
            catch (SocketException exp)
            {
                Logger.Warn($"No address found for host '{hostName}' in DNS.", exp);
                return null;
            }
        }
    }
}
