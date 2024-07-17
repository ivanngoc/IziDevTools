using System.Reflection;
using IziHardGames.Libs.Methods.Tester;

namespace System.Reflection;

public static class ExtensionsForMethodInfoAsTestSubject
{
    public static bool IsSingleArgument(this MethodInfo info)
    {
        return ReflectionsHelperForMethods.IsSingleArgument(info);
    }
}