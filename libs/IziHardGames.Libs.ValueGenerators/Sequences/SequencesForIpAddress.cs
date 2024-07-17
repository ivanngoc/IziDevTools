using System.Collections.Generic;
using System.Net;
using IziHardGames.TestLibs.ValueGenerators.Increments;

namespace IziHardGames.TestLibs.ValueGenerators.Sequences;

public class SequencesForIpAddress
{
    public static IEnumerable<IPAddress> GenIncrements(IPAddress address, int count = int.MaxValue)
    {
        yield return address;
        for (int i = 0; i < count; i++)
        {
            address = IncrementForBcl.IncrementForIPAddress(address);
            yield return address;
        }
    }
}