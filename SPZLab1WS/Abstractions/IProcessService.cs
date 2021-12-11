namespace SPZLab1WS.Abstractions;

public interface IProcessService
{
    void StartProcessing(Process process);

    void UpdateWorkingSet(Process process);
}