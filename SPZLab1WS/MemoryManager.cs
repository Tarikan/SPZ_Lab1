using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace SPZLab1WS;

public class MemoryManager : IMemoryManager
{
    private readonly Dictionary<int, VirtualPage> _memoryMap = new();
    private readonly Random _random = new();
    private readonly ILogger<IMemoryManager> _logger = Program.CreateLoggerInstance<IMemoryManager>();

    public void ResetUsageBit()
    {
        throw new System.NotImplementedException();
    }

    public void Read(VirtualPage virtualPage)
    {
        throw new System.NotImplementedException();
    }

    public void Write(VirtualPage virtualPage)
    {
        throw new System.NotImplementedException();
    }

    public int GetPhysicalPage(VirtualPage virtualPage)
    {
        throw new System.NotImplementedException();
    }
}