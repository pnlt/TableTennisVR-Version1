using System;
using UnityEngine;
using UnityEngine.Playables;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private Checkpoints checkpoints;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(checkpoints);
        builder.Register<Notification>(Lifetime.Singleton);
    }
}
