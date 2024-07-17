using System;

namespace IziHardGames.Libs.Methods.Tester;

public interface ICollectionOfRandomizers
{
    IRandomizer GetRandomizer(Type type);
}