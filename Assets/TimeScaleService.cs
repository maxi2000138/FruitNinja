using UnityEngine;

public class TimeScaleService
{
    private readonly BonusesConfig _bonusesConfig;

    public float PhysicTimeScale { get; private set; }
    public float UnityTimeScale =>
        Time.timeScale;
    
    public TimeScaleService(BonusesConfig bonusesConfig)
    {
        _bonusesConfig = bonusesConfig;
        ResetAllScales();
    }


    public void SetPhysicFrozenTimeScale() => PhysicTimeScale = _bonusesConfig.FrozenTimeScale;
    public void NullifyUnityTimeScale() => Time.timeScale = 0f;
    public void NullifyPhysicTimeScale() => PhysicTimeScale = 0f;
    public void SetPhysicTimeScale(float timeScale) => PhysicTimeScale = timeScale;
    public void SetUnityTimeScale(float timeScale) => Time.timeScale = timeScale;
    public void ResetPhysicTimeScale() => PhysicTimeScale = 1f;
    public void ResetUnityTimeScale() => Time.timeScale = 1f;

    private void ResetAllScales()
    {
        ResetPhysicTimeScale();
        ResetUnityTimeScale();
    }
}
