using System;
using System.Collections.Generic;
using System.Linq;

namespace SPZLab1WS;

public class Process
{
    public readonly int NumberOfPages;
    public readonly int ProcessId;
    public List<VirtualPage> VirtualPages { get; }
    public List<VirtualPage> WorkingSet { get; set; }
    public int WorkingSetMaxSize { get; }

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
        var random = new Random();
        WorkingSet = VirtualPages.OrderBy(_ => random.Next())
            .Take(WorkingSetMaxSize)
            .ToList();
    }

    public override string ToString()
    {
        return $"Process {ProcessId}\n" +
               $"Number of pages: {NumberOfPages}\n";
    }
}