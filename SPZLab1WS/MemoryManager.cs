using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using SPZLab1WS.Abstractions;

namespace SPZLab1WS;

public class MemoryManager : IMemoryManager
{
    private readonly Dictionary<int, VirtualPage> _memoryMap = new();
    private readonly Random _random = new();
    private readonly ILogger<MemoryManager> _logger;

    public MemoryManager(ILogger<MemoryManager> logger)
    {
        _logger = logger;
        foreach (var i in Enumerable.Range(0, AppConstants.NumberOfPhysicalPages))
        {
            _memoryMap[i] = null;
        }
    }

    public void ResetUsageBit()
    {
        var now = DateTime.UtcNow;
        var entriesToUpdate = _memoryMap
            .Where(kvp => kvp.Value != null &&
                          (now - kvp.Value.TimeOfLastUsage).Milliseconds > AppConstants.MillisecondsToResetUsageBit);

        foreach (var mapEntry in entriesToUpdate)
        {
            mapEntry.Value.U = false;
            _logger.LogInformation($"Usage bit is reset for page with address {mapEntry.Value.PageAddress}");
        }
    }

    public void Read(VirtualPage virtualPage)
    {
        GetPhysicalPage(virtualPage);
        virtualPage.U = true;
        virtualPage.TimeOfLastUsage = DateTime.UtcNow;

    }

    public void Write(VirtualPage virtualPage)
    {
        GetPhysicalPage(virtualPage);
        virtualPage.U = true;
        virtualPage.M = true;
        virtualPage.TimeOfLastUsage = DateTime.UtcNow;

    }

    public int GetPhysicalPage(VirtualPage virtualPage)
    {
        if (!virtualPage.P){
            ReplacePage(virtualPage);
        }else{
            virtualPage.U = true;
            if(_random.NextDouble() > 0.5){
                virtualPage.M = true;
            }
        }
        return virtualPage.PageAddress;
    }

    private void ReplacePage(VirtualPage newVirtualPage) {
        var isPageReplaced = false;
        var notUsedPages = new List<VirtualPage>();
        foreach (var entry in _memoryMap) {
            if (entry.Value == null){
                newVirtualPage.PageAddress = entry.Key;
                newVirtualPage.P = true;
                _memoryMap[newVirtualPage.PageAddress] = newVirtualPage;
                return;
            }
            if (entry.Value.U) {
                entry.Value.U = false;
                _logger.LogInformation($"VirtualPage with address {entry.Value.PageAddress} did not changed");
            }
            else if(!isPageReplaced && ((DateTime.UtcNow - entry.Value.TimeOfLastUsage).Milliseconds > AppConstants.T))
            {
                ReplaceVirtualPages(entry.Value, newVirtualPage);
                notUsedPages.Add(entry.Value);
                isPageReplaced = true;
                _logger.LogInformation($"VirtualPage with address {entry.Value.PageAddress} replaced by" +
                                       $"page with address {newVirtualPage.PageAddress}");
            }
            else {
                notUsedPages.Add(entry.Value);
            }
        }
        if (!isPageReplaced && notUsedPages.Count > 0){
            var v = notUsedPages.First();
            foreach (var notUsedPage in notUsedPages)
            {
                if (v.TimeOfLastUsage > notUsedPage.TimeOfLastUsage){
                    v = notUsedPage;
                }
            }
            ReplaceVirtualPages(v, newVirtualPage);
        }
        else
        {
            var rand = _random.NextDouble();
            var index = rand > 0.1
                ? Convert.ToInt32(Math.Round(rand * _memoryMap.Count))
                : Convert.ToInt32(Math.Round(rand * _memoryMap.Count));
            
            var virtualPage = _memoryMap[index];
            
            _logger.LogInformation(virtualPage.ToString());
            _logger.LogInformation(newVirtualPage.ToString());
            ReplaceVirtualPages(virtualPage, newVirtualPage);
            virtualPage.P = true;
            virtualPage.U = false;
            virtualPage.M = false;
        }
    }

    private void ReplaceVirtualPages(VirtualPage virtualPage, VirtualPage newVirtualPage) {
        virtualPage.U = false;
        virtualPage.P = false;
        newVirtualPage.P = true;
        _memoryMap[virtualPage.PageAddress] = newVirtualPage;
        newVirtualPage.PageAddress = virtualPage.PageAddress;
    }

    public override string ToString()
    {
        var result = new StringBuilder();
        foreach (var mapEntry in _memoryMap
                     .Where(kvp => kvp.Value != null))
        {
            result.Append($"{mapEntry.Key} : {mapEntry.Value}\n");
        }

        return result.ToString();
    }
}