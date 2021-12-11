namespace SPZLab1WS.Abstractions;

public interface IMemoryManager
{ 
    void ResetUsageBit();
    void Read(VirtualPage virtualPage);
    void Write(VirtualPage virtualPage);
    int GetPhysicalPage(VirtualPage virtualPage);
}