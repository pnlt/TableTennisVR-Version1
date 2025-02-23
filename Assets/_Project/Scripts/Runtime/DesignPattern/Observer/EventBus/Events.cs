using Dorkbots.XR.Runtime.DesignPattern.Observer;

public interface IEvent { }

public struct TestEvent : IEvent { }

public struct PlayerEvent : IEvent {
    public int health;
    public int mana;
}
