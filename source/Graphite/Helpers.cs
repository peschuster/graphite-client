using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Graphite
{
    public static class Helpers
    {
        /// <see cref="http://stackoverflow.com/questions/106179/regular-expression-to-match-hostname-or-ip-address#106223" />
        public const string ValidHostnamePattern = @"^(([a-zA-Z]|[a-zA-Z][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z]|[A-Za-z][A-Za-z0-9\-]*[A-Za-z0-9])$";
        
        public static bool IsHostname(string value)
        {
            return Regex.IsMatch(value, ValidHostnamePattern);
        }

        public static IPAddress LookupIpAddress(string hostname)
        {
            if (string.IsNullOrEmpty(hostname) || !IsHostname(hostname))
                throw new ArgumentException("Specified value is no valid hostname.", "hostname");

            IPHostEntry host = Dns.GetHostEntry(hostname);

            if (host.AddressList.Length == 0)
            {
                throw new InvalidOperationException("Unable to find an ip address for specified hostname.");
            }

            return host.AddressList[0];
        }

        /// <exception cref="System.ArgumentException" />
        public static IPAddress ParseAddress(string ipAddressOrHostname)
        {
            IPAddress result;

            if (IPAddress.TryParse(ipAddressOrHostname, out result))
            {
                return result;
            }
            else if (IsHostname(ipAddressOrHostname))
            {
                try
                {
                    return LookupIpAddress(ipAddressOrHostname);
                }
                catch (SystemException exception)
                {
                    throw new ArgumentException(exception.Message, "ipAddressOrHostname", exception);
                }
            }

            throw new ArgumentException("'" + ipAddressOrHostname + "' is neigher an ip address nor a hostname.", "ipAddressOrHostname");
        }
    }
}
