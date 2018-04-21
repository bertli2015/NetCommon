using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace NetCommon.Console.Common
{
    public static class NetworkUtility
    {
        public static List<string> GetIPv4s()
        {
            var ipA = FindLanAddress();
            if (ipA.Count > 0)
            {
                return ipA.Select(c => c.ToString()).ToList();
            }
            var ipB = GetIPv4Ex();
            if (ipB.Count > 0)
            {
                return ipB.Select(c => c.ToString()).ToList();
            }
            return new List<string>();
        }

        public static string GetIPv4()
        {
            var ipA = FindLanAddress();
            if (ipA.Count > 0)
            {
                return ipA.FirstOrDefault().ToString();
            }
            var ipB = GetIPv4Ex();
            if (ipB.Count > 0)
            {
                return ipB.FirstOrDefault().ToString();
            }
            return String.Empty;
        }

        private static List<IPAddress> GetIPv4Ex()
        {
            List<IPAddress> ips = new List<IPAddress>();
            //string localIp = "?";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ips.Add(ip);
                    //localIp = ip.ToString();
                }
            }
            return ips;
        }

        //http://stackoverflow.com/questions/1069103/how-to-get-the-ip-address-of-the-server-on-which-my-c-sharp-application-is-runni
        private static List<IPAddress> FindLanAddress()
        {
            IPAddress gateway = FindGetGatewayAddress();
            if (gateway == null) return new List<IPAddress>();

            List<IPAddress> ips = new List<IPAddress>();
            IPAddress[] pIPAddress = Dns.GetHostAddresses(Dns.GetHostName());

            foreach (IPAddress address in pIPAddress)
            {
                if (IsAddressOfGateway(address, gateway))
                {
                    ips.Add(address);
                }
            }

            return ips;
        }

        static bool IsAddressOfGateway(IPAddress address, IPAddress gateway)
        {
            if (address != null && gateway != null)
                return IsAddressOfGateway(address.GetAddressBytes(), gateway.GetAddressBytes());
            return false;
        }

        static bool IsAddressOfGateway(byte[] address, byte[] gateway)
        {
            if (address != null && gateway != null)
            {
                int gwLen = gateway.Length;
                if (gwLen > 0)
                {
                    if (address.Length == gateway.Length)
                    {
                        --gwLen;
                        int counter = 0;
                        for (int i = 0; i < gwLen; i++)
                        {
                            if (address[i] == gateway[i])
                                ++counter;
                        }
                        return (counter == gwLen);
                    }
                }
            }
            return false;

        }

        static IPAddress FindGetGatewayAddress()
        {
            IPGlobalProperties ipGlobProps = IPGlobalProperties.GetIPGlobalProperties();

            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                IPInterfaceProperties ipInfProps = ni.GetIPProperties();
                foreach (GatewayIPAddressInformation gi in ipInfProps.GatewayAddresses)
                {
                    return gi.Address;
                }

            }
            return null;
        }
    }
}
