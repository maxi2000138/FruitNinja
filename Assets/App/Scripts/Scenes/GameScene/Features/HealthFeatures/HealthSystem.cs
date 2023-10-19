using UnityEngine;

public class HealthSystem : IRestartGameListener
{
    public int Health { get; private set; }
    
    private HealthConfig _healthConfig;
    private readonly HealthView _healthView;

    public HealthSystem(HealthConfig healthConfig, HealthView healthView)
    {
        _healthConfig = healthConfig;
        _healthView = healthView;

        _healthView.Construct(_healthConfig);
        OnRestartGame();
    }

    public void OnRestartGame()
    {
        Health = _healthConfig.Health;
        _healthView.SetupHealth(Health);
    }

    public void LooseLife()
    {
        if(Health == 0)
            return;

        if(Health > 0)
            _healthView.LooseHealth();
        
        Health = Mathf.Clamp(Health - 1, 0, Health);
    }
}
