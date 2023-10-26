using System;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.DestroyTrigger;
using App.Scripts.Scenes.Infrastructure.MonoInterfaces;

public class HealthOverLoosePolicy : ILoosePolicy, IRestartGameListener, IUpdatable
{
    public event Action NeedLoose;
    public event Action NeedLateLoose;

    private bool _looseInvoked;
    private bool _lateLooseInvoked;
    private readonly HealthSystem _healthSystem;
    private readonly IDestroyTrigger _destroyTrigger;

    public HealthOverLoosePolicy(HealthSystem healthSystem, IDestroyTrigger destroyTrigger)
    {
        _healthSystem = healthSystem;
        _destroyTrigger = destroyTrigger;
        OnRestartGame();
    }

    public void Update(float deltaTime)
    {
        if (!_looseInvoked && _healthSystem.Health == 0)
        {
            NeedLoose?.Invoke();
            _looseInvoked = true;
        }

        if (_looseInvoked && !_lateLooseInvoked && _destroyTrigger.DestroyGroupCount == 0)
        {
            NeedLateLoose?.Invoke();
            _lateLooseInvoked = true;
        }
    }

    public void OnRestartGame()
    {
        _looseInvoked = false;
        _lateLooseInvoked = false;

    }
}
