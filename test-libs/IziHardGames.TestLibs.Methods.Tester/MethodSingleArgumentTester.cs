using System.Reflection;

namespace IziHardGames.Libs.Methods.Tester;

public static class MethodSingleArgumentTester
{
    public static async Task TestAsync(object? target, MethodInfo targetInfo, Type model)
    {
        var methodInfo = typeof(MethodSingleArgumentTester).GetMethods(BindingFlags.Static | BindingFlags.Public).First(x => x.Name == nameof(TestAsyncGen) && x.ContainsGenericParameters);
        ArgumentNullException.ThrowIfNull(methodInfo);
        var method = methodInfo.MakeGenericMethod(model);
        var result = method.Invoke(null, new object[] { target!, targetInfo });
        await AwaitResult(result).ConfigureAwait(false);
    }

    public static async Task TestAsyncGen<TModel>(object? target, MethodInfo targetMethod)
    {
        var typeModel = typeof(TModel);
        var variator = new VariatorForModel();
        var variants = variator.GetTestingVariants<TModel>();
        foreach (var variant in variants)
        {
            var result = targetMethod.Invoke(target, new object?[] { variant });
            if (result is Task task)
            {
                await task.ConfigureAwait(false);
            }
            else
            {
                throw new InvalidOperationException("impossible!");
            }
        }
    }

    public static async Task AwaitResult(object? result)
    {
        if (result != null)
        {
            if (result is Task task)
            {
                await task.ConfigureAwait(false);
            }
            else if (result is ValueTask valueTask)
            {
                await valueTask.ConfigureAwait(false);
            }
            else if (result.GetType().IsGenericType && result.GetType().GetGenericTypeDefinition() == typeof(ValueTask<>))
            {
                // typeof(ValueTask<>).   
                var dummyAwaiter = typeof(MethodSingleArgumentTester).GetMethod("HelpToAwait", BindingFlags.Static | BindingFlags.NonPublic);
                var toCall = dummyAwaiter!.MakeGenericMethod(result.GetType().GenericTypeArguments.First());
                var toWait = toCall.Invoke(null, new[] { result });

                if (toWait is Task task2)
                {
                    await task2.ConfigureAwait(false);
                }
                else throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException($"custom awaiter? {result.GetType().FullName}");
            }
        }
    }

    private static async Task HelpToAwait<T>(ValueTask<T> t)
    {
        await t.ConfigureAwait(false);
    }
}