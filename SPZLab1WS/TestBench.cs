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
    private readonly IProcessService _processService;

    public TestBench(
        ILogger<TestBench> logger,
        IMemoryManager memoryManager,
        IProcessService processService)
    {
        _logger = logger;
        _memoryManager = memoryManager;
        _processService = processService;
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
                _processService.StartProcessing(p);
                if (_random.NextDouble() < AppConstants.UpdateWorkingSetThreshold)
                {
                    _processService.UpdateWorkingSet(p);
                };
            });
            _memoryManager.ResetUsageBit();
            _logger.LogInformation(_memoryManager.ToString());
        }
    }
}