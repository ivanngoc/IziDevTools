using System;
using System.Net;

namespace IziHardGames.Libs.Methods.Tester.Randoms;

public class RandomsForBclNetwork
{
    public static IPAddress GetRandomAddress()
    {
        // return new IPAddress(uint.MaxValue);
        return new IPAddress(Random.Shared.NextInt64(0, uint.MaxValue));
    }
}