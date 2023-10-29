using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class HealthSystem : IRestartGameListener
{
    public int Health { get; private set; }
    
    private HealthConfig _healthConfig;
    private readonly HealthController _healthController;
    private TweenCore _tweenCore;
    private bool _isImmortal = false;


    public HealthSystem(HealthConfig healthConfig, HealthController healthController, TweenCore tweenCore)
    {
        _tweenCore = tweenCore;
        _healthConfig = healthConfig;
        _healthController = healthController;

        _healthController.Construct(_healthConfig, _tweenCore);
        OnRestartGame();
        SetNonImmortal();
    }

    public void SetImmortal() => 
        _isImmortal = true;

    public void SetNonImmortal() => 
        _isImmortal = false;

    public void OnRestartGame()
    {
        Health = _healthConfig.Health;
        _healthController.SetupHealth(Health);
        SetNonImmortal();
    }

    public void LooseLife()
    {
        if(Health == 0 || _isImmortal)
            return;

        if(Health > 0)
            _healthController.LooseHealth();
        
        Health = Mathf.Clamp(Health - 1, 0, Health);
    }
    
    public void HealLife(Vector2 startAnimationPoint)
    {
        if(Health >= _healthConfig.Health)
            return;

        _healthController.HealthHealth(startAnimationPoint);
        
        Health = Mathf.Clamp(Health + 1, 0, _healthConfig.Health);
    }
}
