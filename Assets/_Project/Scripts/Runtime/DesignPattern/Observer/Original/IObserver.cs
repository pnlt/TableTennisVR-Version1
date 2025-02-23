namespace Dorkbots.XR.Runtime.DesignPattern.Observer
{
    public interface IObserver
    {
        void Notify<T>(T data);
        void Notify();
    }
}