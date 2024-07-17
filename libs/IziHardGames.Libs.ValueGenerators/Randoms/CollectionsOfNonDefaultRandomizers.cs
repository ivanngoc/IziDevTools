using System;
using System.Runtime.CompilerServices;

namespace IziHardGames.Libs.Methods.Tester.Randoms;

public class CollectionsOfNonDefaultRandomizers : ICollectionOfRandomizers
{
    private RandomizerPrimetiveNonDefault randomizer = new RandomizerPrimetiveNonDefault();
    private ObjectWithRandomValues randomizerModel = new ObjectWithRandomValues();

    public IRandomizer GetRandomizer(Type type)
    {
        if (type.IsPrimitive)
        {
            return randomizer;
        }

        if (type.IsClass || type.IsValueType)
        {
            return randomizerModel;
        }

        throw new NotImplementedException(type.AssemblyQualifiedName);
    }
}