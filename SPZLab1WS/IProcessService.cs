namespace SPZLab1WS;

public interface IProcessService
{
    void StartProcessing(Process process);

    void UpdateWorkingSet(Process process);
}