using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SPZLab1WS;

public class Process
{
    public readonly int NumberOfPages;
    public readonly int ProcessId;
    public List<VirtualPage> VirtualPages { get; }
    public List<VirtualPage> WorkingSet { get; private set; }
    public int WorkingSetMaxSize { get; }

    private static readonly Random Random = new();
    private static readonly ILogger<Process> Logger = Program.CreateLoggerInstance<Process>();

    public Process(int numberOfPages, int processId, int workingSetMaxSize)
    {
        NumberOfPages = numberOfPages;
        ProcessId = processId;
        WorkingSetMaxSize = workingSetMaxSize;

        VirtualPages = Enumerable.Range(0, numberOfPages)
            .Select(idx => new VirtualPage
            {
                PageAddress = idx * AppConstants.PageSize,
                P = false,
                U = false,
                M = false,
                TimeOfLastUsage = DateTime.UtcNow,
                ProcessId = processId
            }).ToList();
        UpdateWorkingSet();
    }

    public void StartProcessing()
    {
        var memoryManager = Program.ProgramServiceProvider.GetRequiredService<IMemoryManager>();
        var read = Random.NextDouble() > AppConstants.ProcessReadWriteThreshold;
        var useWorkingSet = Random.NextDouble() > AppConstants.ProcessWorkingSetThreshold;

        var page = useWorkingSet
            ? WorkingSet.OrderBy(_ => Random.Next()).First()
            : VirtualPages.OrderBy(_ => Random.Next()).First();

        if (read)
        {
            Logger.LogInformation($"Process {ProcessId} reading virtual page, from working set = {useWorkingSet}");
            memoryManager.Read(page);
        }
        else
        {
            Logger.LogInformation($"Process {ProcessId} writing to virtual page, from working set = {useWorkingSet}");
            memoryManager.Write(page);
        }
    }

    public void UpdateWorkingSet() =>
        WorkingSet = VirtualPages.OrderBy(_ => Random.Next())
            .Take(WorkingSetMaxSize)
            .ToList();

    public override string ToString()
    {
        return $"Process {ProcessId}\n" +
               $"Number of pages: {NumberOfPages}\n";
    }
}