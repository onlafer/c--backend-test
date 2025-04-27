using System;
using Microsoft.Extensions.Logging;
using LoggingDemo.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace LoggingDemo
{
    public class PostgreSqlLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool>? _filter;
        private readonly IServiceProvider _serviceProvider;

        public PostgreSqlLoggerProvider(
            Func<string, LogLevel, bool>? filter,
            IServiceProvider serviceProvider)
        {
            _filter = filter;
            _serviceProvider = serviceProvider;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new PostgreSqlLogger(categoryName, _filter, _serviceProvider);
        }

        public void Dispose() { }

        private class PostgreSqlLogger : ILogger
        {
            private readonly string _categoryName;
            private readonly Func<string, LogLevel, bool>? _filter;
            private readonly IServiceProvider _serviceProvider;

            public PostgreSqlLogger(
                string categoryName,
                Func<string, LogLevel, bool>? filter,
                IServiceProvider serviceProvider)
            {
                _categoryName = categoryName;
                _filter = filter;
                _serviceProvider = serviceProvider;
            }

            public IDisposable? BeginScope<TState>(TState state) where TState : notnull
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return (_filter == null) || _filter(_categoryName, logLevel);
            }

            public void Log<TState>(
                LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception? exception,
                Func<TState, Exception?, string> formatter)
            {
                if (!IsEnabled(logLevel))
                {
                    return;
                }

                var message = formatter(state, exception);

                Task.Run(async () =>
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        var logEntry = new LogEntry
                        {
                            Timestamp = DateTime.UtcNow,
                            Level = logLevel.ToString(),
                            Message = message,
                            Exception = exception?.ToString(),
                            Logger = _categoryName,
                            Properties = null
                        };

                        try
                        {
                            await dbContext.Logs.AddAsync(logEntry);
                            await dbContext.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка записи лога в PostgreSQL: {ex.Message}");
                        }
                    }
                });
            }
        }
    }
}