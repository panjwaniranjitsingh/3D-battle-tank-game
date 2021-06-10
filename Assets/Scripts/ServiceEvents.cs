using System;

public class ServiceEvents : SingletonDemo<ServiceEvents>
{
    public event Action EnemyKilled;
    public event Action BulletFired;
    public event Action GameStarted;

    public void InvokeEnemyKilledEvent()
    {
        EnemyKilled?.Invoke();
    }

    public void InvokeBulletFiredEvent()
    {
        BulletFired?.Invoke();
    }

    public void InvokeGameStartedEvent()
    {
        GameStarted?.Invoke();
    }
}
