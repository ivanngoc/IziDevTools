using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace IziHardGames.Libs.Methods.Tester;

public class MethodSingleArgumentTesterTests
{
    private readonly ITestOutputHelper console;

    public MethodSingleArgumentTesterTests(ITestOutputHelper helper)
    {
        this.console = helper;
    }

    [Fact]
    private async Task Test_NonGeneric_Call()
    {
        var method = GetType().GetMethod(nameof(Dummy), BindingFlags.Instance | BindingFlags.NonPublic)!;
        await MethodSingleArgumentTester.TestAsync(this, method, typeof(float));
    }

    [Fact]
    private async Task Test()
    {
        var task = Task.Delay(1);
        await MethodSingleArgumentTester.AwaitResult(task);

        object taskValue = ValueTask.CompletedTask;
        await MethodSingleArgumentTester.AwaitResult(taskValue);

        object taskValueGen = ValueTask.FromResult(float.NaN);
        await MethodSingleArgumentTester.AwaitResult(taskValueGen);

        await MethodSingleArgumentTester.AwaitResult(null);
        await Assert.ThrowsAsync<NotImplementedException>(async () => await MethodSingleArgumentTester.AwaitResult(new object()));
    }

    private async Task Dummy(float f)
    {
        await Task.CompletedTask;
        console.WriteLine(f.ToString());
    }
}

public class VariatorForPrimitiveTests
{
    private readonly ITestOutputHelper console;

    public VariatorForPrimitiveTests(ITestOutputHelper helper)
    {
        this.console = helper;
    }

    [Fact]
    private void Test_CastToFunc()
    {
        var func = VariatorForPrimitive.CreateFuncAsGen<string>(nameof(VariatorForPrimitive.VariantsForStrings));
        Assert.NotNull(func);
        Assert.IsType<Func<IEnumerable<string>>>(func);

        var expected = VariatorForPrimitive.VariantsForStrings();
        var fact = func.Invoke();
        var eTorExp = expected.GetEnumerator();
        var eTorFact = fact.GetEnumerator();

        for (int i = 0; i < expected.Count(); i++)
        {
            Assert.True(eTorExp.MoveNext());
            Assert.True(eTorFact.MoveNext());
            Assert.Equal(eTorExp.Current, eTorExp.Current);
        }

        Assert.Throws<ArgumentException>(() =>
        {
            var castStringToInt = VariatorForPrimitive.CreateFuncAsGen<int>(nameof(VariatorForPrimitive.VariantsForStrings));
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var castFloatToInt = VariatorForPrimitive.CreateFuncAsGen<int>(nameof(VariatorForPrimitive.VariantsForFloat));
        });

        var castStringToObject = VariatorForPrimitive.CreateFuncAsGen<object>(nameof(VariatorForPrimitive.VariantsForStrings));
        Assert.NotNull(castStringToObject);

        Assert.Throws<ArgumentException>(() =>
        {
            var castLongToInt = VariatorForPrimitive.CreateFuncAsGen<int>(nameof(VariatorForPrimitive.VariantsForLong));
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var castIntToLong = VariatorForPrimitive.CreateFuncAsGen<long>(nameof(VariatorForPrimitive.VariantsForInt));
        });
    }

    [Fact]
    private async Task Test_Variant_Float()
    {
        Func<float, Task> func = async (x) =>
        {
            await Task.CompletedTask;
            console.WriteLine(x.ToString());
        };
        await MethodSingleArgumentTester.TestAsyncGen<float>(func.Target, func.Method);
    }

    [Fact]
    private async Task Test_Variant_Dobule()
    {
        Func<double, Task> func = async (x) =>
        {
            await Task.CompletedTask;
            console.WriteLine(x.ToString());
        };
        await MethodSingleArgumentTester.TestAsyncGen<double>(func.Target, func.Method);
    }

    [Fact]
    private async Task Test_Variant_String()
    {
        Func<string, Task> func = async (x) =>
        {
            await Task.CompletedTask;
            console.WriteLine(x?.ToString() ?? "null");
        };
        await MethodSingleArgumentTester.TestAsyncGen<string>(func.Target, func.Method);
    }

    [Fact]
    private async Task Test_Variant_Int()
    {
        await TestVariant<int>();
    }

    [Fact]
    private async Task Test_Variant_Long()
    {
        await TestVariant<long>();
    }

    private async Task TestVariant<T>()
    {
        Func<T, Task> func = async (x) =>
        {
            await Task.CompletedTask;
            console.WriteLine(x?.ToString() ?? String.Empty);
        };
        await MethodSingleArgumentTester.TestAsyncGen<T>(func.Target, func.Method);
    }
}