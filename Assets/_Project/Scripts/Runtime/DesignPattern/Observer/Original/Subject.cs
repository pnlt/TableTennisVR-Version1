using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dorkbots.XR.Runtime.DesignPattern.Observer
{
    public abstract class Subject : MonoBehaviour
    {
        public Dictionary<Type, List<IObserver>> observers = new Dictionary<Type, List<IObserver>>();
        protected abstract void AddSubscriber(IObserver observer);
        protected abstract void RemoveSubscriber(IObserver observer);
    }
}