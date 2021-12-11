namespace SPZLab1WS;

public interface IMemoryManager
{ 
    void ResetUsageBit();
    void Read(VirtualPage virtualPage);
    void Write(VirtualPage virtualPage);
    int GetPhysicalPage(VirtualPage virtualPage);
}