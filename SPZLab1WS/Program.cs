using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SPZLab1WS.Abstractions;

namespace SPZLab1WS;

public class Program
{
    public static void Main(string[] args)
    {
        var serviceProvider = BuildServices();
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("Program starting");

        var testBench = serviceProvider.GetRequiredService<ITestBench>();
        
        testBench.Test();
        
        logger.LogInformation("Program ended successfully");
    }

    private static ServiceProvider BuildServices()
    {
        var builder = new ServiceCollection();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("log.txt")
            .CreateLogger();
        
        builder.AddLogging(configure => configure.AddSerilog()
            .SetMinimumLevel(LogLevel.Trace));

        builder.AddSingleton<IMemoryManager, MemoryManager>();
        builder.AddSingleton<ITestBench, TestBench>();
        builder.AddSingleton<IProcessService, ProcessService>();

        return builder.BuildServiceProvider();
    }
}