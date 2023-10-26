public class PauseController
{
    private readonly TimeScaleService _timeScaleService;
    private float _previousPhysicTimeScale;
    private float _previousUnityTimeScale;
    public PauseController(TimeScaleService timeScaleService)
    {
        _timeScaleService = timeScaleService;
    }
    
    public void PauseGame()
    {
        _previousPhysicTimeScale = _timeScaleService.PhysicTimeScale;
        _previousUnityTimeScale = _timeScaleService.UnityTimeScale;
        _timeScaleService.NullifyUnityTimeScale();
        _timeScaleService.NullifyPhysicTimeScale();
    }

    public void ResumeGame()
    {
        _timeScaleService.SetPhysicTimeScale(_previousPhysicTimeScale);
        _timeScaleService.SetUnityTimeScale(_previousUnityTimeScale);
    }
}
