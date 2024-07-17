using System.Collections.Concurrent;
using Moq.AutoMock;
using IziHardGames.Libs.ForTasks;

namespace IziHardGames.Lib.Tasks.UnitTest;

public class UtilityForTasksTests
{
    [Fact]
    private async Task RetryAsync_Ok()
    {
        int repeatCount = 5;
        int timeout = 0;
        CancellationTokenSource cts = new CancellationTokenSource();
        Func<Task> func = TestFunc;
        AutoMocker provider = new AutoMocker();
        var fact0 = UtilityForTasks.RetryAsync(func, repeatCount, timeout, cts.Token);
        await fact0;
        Assert.NotNull(fact0);
        Assert.True(fact0.IsCompletedSuccessfully);
    }

    [Fact]
    private async Task RetryAsync_Fail()
    {
        int repeatCount = 5;
        int timeout = 0;
        CancellationTokenSource cts = new CancellationTokenSource();
        Func<Task> func = TestFunc;
        AutoMocker provider = new AutoMocker();
        var fact0 = UtilityForTasks.RetryAsync(() => { throw new MyException("i throw"); }, repeatCount, timeout, cts.Token);
        await fact0;
        Assert.NotNull(fact0);
        Assert.True(fact0.IsCompleted);
        Assert.Equal(TaskStatus.RanToCompletion, fact0.Status);
        Assert.Null(fact0.Exception);
    }

    //    [Fact]
    private async Task RetryAsyncGeneric_Common()
    {
        await Task.CompletedTask;
        ConcurrentQueue<bool> flags = new ConcurrentQueue<bool>();
        flags.Enqueue(false);
        flags.Enqueue(false);
        flags.Enqueue(true);
        flags.Enqueue(false);
        CancellationTokenSource cts = new CancellationTokenSource();
        State state = new State();

        var fact0 = UtilityForTasks.RetryAsync(() =>
        {
            state.CountCall();

            if (flags.TryDequeue(out var value))
            {
                if (value)
                {
                    return Task.CompletedTask;
                }
            }

            return Task.FromException(new Exception(""));
        }, state.repeatCount, state.timeout, cts.Token);
    }


    private Task TestFunc()
    {
        return Task.Run(async () => { await Task.CompletedTask; });
    }

    [Fact]
    private async Task RetryAsync_Generic_Ok()
    {
        int repeatCount = 5;
        int timeout = 1;
        CancellationTokenSource cts = new CancellationTokenSource();
        var fact0 = UtilityForTasks.RetryAsync<int>(() => Task.FromResult(10), repeatCount, timeout, cts.Token);
        Assert.NotNull(fact0.Result);
        await fact0.Result;
        Assert.Equal(10, fact0.Result.Result);
    }

    [Fact]
    private async Task RetryAsync_Generic_Fail()
    {
        await Task.CompletedTask;
        int repeatCount = 5;
        int timeout = 1;
        CancellationTokenSource cts = new CancellationTokenSource();
        var fact0 = UtilityForTasks.RetryAsync<int>(() => throw new MyException("this is custom exception"), repeatCount, timeout, cts.Token);
        Assert.NotNull(fact0.Result);
        var fact1 = fact0.Result;
        await Assert.ThrowsAsync<AggregateException>(async () => await fact1);
    }

    [Fact]
    private async Task RetryAsync_Generic_OkAfterFail()
    {
        await Task.CompletedTask;
        CancellationTokenSource cts = new CancellationTokenSource();
        State state = new State();
        ConcurrentQueue<bool> flags = new ConcurrentQueue<bool>();

        var fact0 = UtilityForTasks.RetryAsync<int>(() =>
        {
            state.CountCall();

            if (flags.TryDequeue(out var value))
            {
                if (value)
                {
                    return Task.FromResult(20);
                }
            }
            return Task.FromException<int>(new Exception(""));
        }, state.repeatCount, state.timeout, cts.Token);
    }


    private class State
    {
        private int counter;
        public int repeatCount = 50;
        public int timeout = 0;

        internal void CountCall()
        {
            lock (this)
            {
                counter++;
            }
        }
    }

    private class MyException : Exception
    {
        public MyException(string msg) : base(msg)
        {
        }
    }
}