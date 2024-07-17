using System;
using System.Collections.Generic;

namespace IziHardGames.Libs.Methods.Tester;

public interface IRegistryForVariators
{
    Func<IEnumerable<T>> GetVariatorGeneric<T>();
    object GetVariator(Type type);
    Delegate GetVariatorAsDelegate(Type type);
    bool Registered(Type type);
    bool TryGetAs<T>(Type type, out T variator);
}