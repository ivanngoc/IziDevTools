using Microsoft.Extensions.Logging;
using System;
using Xunit.Abstractions;

namespace IziHardGames.Libs.Tests
{
    public class XUnitLogger<T> : ILogger<T>
    {
        private readonly ITestOutputHelper _output;

        public XUnitLogger(ITestOutputHelper output)
        {
            _output = output;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter)
        {
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));

            string message = formatter(state, exception!);
            
            if (!string.IsNullOrEmpty(message))
            {
                _output.WriteLine($"{logLevel}: {message}");
            }

            if (exception != null)
            {
                _output.WriteLine($"{logLevel}: {exception}");
            }
        }
    }

    public class XUnitLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _output;

        public XUnitLoggerProvider(ITestOutputHelper output)
        {
            _output = output;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new XUnitLogger<object>(_output);
        }

        public void Dispose()
        {
        }
    }
}