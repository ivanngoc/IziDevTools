using System;
using System.Threading;
using System.Threading.Tasks;

namespace IziHardGames.Libs.ForTasks
{
    public static class UtilityForTasks
    {
        private static readonly Task fromException = Task.FromException(new RetriesExceededException());

        public static async Task<Task> RetryAsync(Func<Task> func, int count, int timeout, CancellationToken token = default)
        {
            REPEAT:
            if (token.IsCancellationRequested) return Task.FromCanceled(token);
            if (count == 0) return fromException;
            try
            {
                var t1 = func();
                await t1.ConfigureAwait(false);
                if (t1.Status != TaskStatus.RanToCompletion || t1.Exception != null)
                {
                    await Task.Delay(timeout).ConfigureAwait(false);
                    t1 = await RetryAsync(func, count - 1, timeout, token).ConfigureAwait(false);
                }

                return t1;
            }
            catch (System.Exception)
            {
                count -= 1;
                await Task.Delay(timeout);
                goto REPEAT;
            }
        }

        
        public static async Task<Task<T>> RetryAsync<T>(Func<Task<T>> func, int count, int timeout, CancellationToken token = default)
        {
            REPEAT:
            if (token.IsCancellationRequested) return Task.FromCanceled<T>(token);
            if (count == 0) return Task.FromException<T>(fromException.Exception);
            try
            {
                var t1 = func();
                await t1.ConfigureAwait(false);
                if (t1.Status != TaskStatus.RanToCompletion || t1.Exception != null)
                {
                    await Task.Delay(timeout).ConfigureAwait(false);
                    t1 = await RetryAsync<T>(func, count - 1, timeout, token).ConfigureAwait(false);
                }

                return t1;
            }
            catch (System.Exception)
            {
                count -= 1;
                await Task.Delay(timeout);
                goto REPEAT;
            }
        }
    }
}