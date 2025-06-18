using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Network
{
    public class SubnetIPsRetriever
    {
        public static List<IPAddress> GetAllIPsInSubnet(UnicastIPAddressInformation unicastInfo)
        {
            List<IPAddress> result = new List<IPAddress>();

            if (unicastInfo.Address.AddressFamily != AddressFamily.InterNetwork)
                return result; // Only support IPv4 here

            byte[] ipBytes = unicastInfo.Address.GetAddressBytes();
            byte[] maskBytes = unicastInfo.IPv4Mask.GetAddressBytes();

            // Calculate network address
            byte[] networkBytes = new byte[4];
            for (int i = 0; i < 4; i++)
                networkBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);

            // Calculate broadcast address
            byte[] broadcastBytes = new byte[4];
            for (int i = 0; i < 4; i++)
                broadcastBytes[i] = (byte)(networkBytes[i] | ~maskBytes[i]);

            uint start = BytesToUInt(networkBytes);
            uint end = BytesToUInt(broadcastBytes);

            // Skip network and broadcast addresses (optional)
            for (uint i = start + 1; i < end; i++)
            {
                result.Add(UIntToIPAddress(i));
            }

            return result;
        }

        private static uint BytesToUInt(byte[] bytes)
        {
            return ((uint)bytes[0] << 24) | ((uint)bytes[1] << 16) | ((uint)bytes[2] << 8) | bytes[3];
        }

        private static IPAddress UIntToIPAddress(uint ip)
        {
            return new IPAddress(new byte[]
            {
            (byte)(ip >> 24),
            (byte)(ip >> 16),
            (byte)(ip >> 8),
            (byte)(ip)
            });
        }




    }
}
