using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace SPZLab1WS;

public class Program
{
    public static ServiceProvider ProgramServiceProvider { get; } = BuildServices();

    public static void Main(string[] args)
    {
        var logger = CreateLoggerInstance<Program>();
        logger.LogInformation("Program starting");
    }

    public static ILogger<T> CreateLoggerInstance<T>()
    {
        return ProgramServiceProvider.GetRequiredService<ILogger<T>>()
               ?? throw new Exception($"Cannot construct logger for type {nameof(T)}");
    }

    private static ServiceProvider BuildServices()
    {
        var builder = new ServiceCollection();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            // .WriteTo.File("log.txt")
            .CreateLogger();
        
        builder.AddLogging(configure => configure.AddSerilog()
            .SetMinimumLevel(LogLevel.Trace));

        builder.AddSingleton<IMemoryManager, MemoryManager>();

        return builder.BuildServiceProvider();
    }
}