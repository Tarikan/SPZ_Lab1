using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace SPZLab1WS;

public class TestBench : ITestBench
{
    public const int ProcessesCount = 8;
    public const int IterationsToRun = 30;
    public readonly List<Process> Processes = new();
    private readonly Random _random = new();
    private readonly ILogger<TestBench> _logger;
    private readonly IMemoryManager _memoryManager;

    public TestBench(ILogger<TestBench> logger, IMemoryManager memoryManager)
    {
        _logger = logger;
        _memoryManager = memoryManager;
        foreach (var i in Enumerable.Range(0, ProcessesCount))
        {
            var pagesNum = Convert.ToInt32(Math.Ceiling(_random.NextDouble() * 5));
            Processes.Add(new Process(pagesNum, i, 2));
        }
    }

    public void Test()
    {
        for (var i = 0; i < IterationsToRun; i++){
            Processes.ForEach(p => {
                p.StartProcessing();
                if (_random.NextDouble() < AppConstants.UpdateWorkingSetThreshold) p.UpdateWorkingSet();
            });
            _memoryManager.ResetUsageBit(); // фоновий процес
            _logger.LogInformation(_memoryManager.ToString());
        }
    }
}