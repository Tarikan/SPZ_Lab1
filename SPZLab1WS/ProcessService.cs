using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using SPZLab1WS.Abstractions;

namespace SPZLab1WS;

public class ProcessService : IProcessService
{
    private readonly ILogger<ProcessService> _logger;
    private readonly Random _random = new();
    private readonly IMemoryManager _memoryManager;

    public ProcessService(
        ILogger<ProcessService> logger,
        IMemoryManager memoryManager)
    {
        _logger = logger;
        _memoryManager = memoryManager;
    }

    public void StartProcessing(Process process)
    {
        var read = _random.NextDouble() > AppConstants.ProcessReadWriteThreshold;
        var useWorkingSet = _random.NextDouble() > AppConstants.ProcessWorkingSetThreshold;

        var page = useWorkingSet
            ? process.WorkingSet.OrderBy(_ => _random.Next()).First()
            : process.VirtualPages.OrderBy(_ => _random.Next()).First();

        if (read)
        {
            _logger.LogInformation($"Process {process.ProcessId} reading virtual page, from working set = {useWorkingSet}");
            _memoryManager.Read(page);
        }
        else
        {
            _logger.LogInformation($"Process {process.ProcessId} writing to virtual page, from working set = {useWorkingSet}");
            _memoryManager.Write(page);
        }
    }

    public void UpdateWorkingSet(Process process) =>
        process.WorkingSet = process.VirtualPages.OrderBy(_ => _random.Next())
            .Take(process.WorkingSetMaxSize)
            .ToList();
}