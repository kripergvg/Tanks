using System;
using Microsoft.Extensions.Logging;
using UnityEngine;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Tanks.Logger
{
    public class UnityLoggerProvider : ILoggerProvider
    {
        public void Dispose()
        {
        
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new UnityLogger();
        }
    
        private class UnityLogger : ILogger
        {
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                switch (logLevel)
                {
                    case LogLevel.Trace:
                    case LogLevel.Debug:
                    case LogLevel.Information:
                        Debug.Log(formatter(state, exception));
                        break;
                    case LogLevel.Warning:
                        Debug.LogWarning(formatter(state, exception));
                        break;
                    case LogLevel.Error:
                    case LogLevel.Critical:
                        if (exception != null)
                        {
                            Debug.LogException(exception);
                        }

                        if (formatter == null)
                        {
                        
                        }
                        else
                        {
                            Debug.LogError(formatter(state, exception));
                        }
                   
                        break;
                    case LogLevel.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
                }
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                throw new NotImplementedException();
            }
        }
    }
}