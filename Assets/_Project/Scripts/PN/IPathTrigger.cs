public interface IPathTrigger
{
    bool IsEnabled { get; set; }
    void OnPathTriggered();
}