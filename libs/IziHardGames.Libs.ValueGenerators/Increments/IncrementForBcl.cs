using System;
using System.Net;

namespace IziHardGames.TestLibs.ValueGenerators.Increments;

public class IncrementForBcl
{
    public static IPAddress IncrementForIPAddress(IPAddress ip)
    {
        byte[] bytes = ip.GetAddressBytes();

        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
            // IPv4 address
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                if (bytes[i] < 255)
                {
                    bytes[i]++;
                    break;
                }
                bytes[i] = 0;
            }
        }
        else if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
        {
            // IPv6 address
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                if (bytes[i] < 255)
                {
                    bytes[i]++;
                    break;
                }
                bytes[i] = 0;
            }
        }
        else
        {
            throw new ArgumentException("Only IPv4 and IPv6 addresses are supported.");
        }

        return new IPAddress(bytes);
    }
}