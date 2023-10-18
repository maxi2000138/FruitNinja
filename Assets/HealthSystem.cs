using UnityEngine;

public class HealthSystem
{
    private int _health;
    private HealthConfig _healthConfig;
    private readonly HealthView _healthView;

    public HealthSystem(HealthConfig healthConfig, HealthView healthView)
    {
        _healthConfig = healthConfig;
        _healthView = healthView;

        _health = _healthConfig.Health;
        _healthView.Construct(_healthConfig);
        _healthView.SetupHealth(_health);
    }
    
    public void LooseLife()
    {
        _health = Mathf.Clamp(_health - 1, 0, _health);
        _healthView.LooseHealth();
    }
}
