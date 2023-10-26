public class PauseController
{
    private readonly TimeScaleService _timeScaleService;

    public PauseController(TimeScaleService timeScaleService)
    {
        _timeScaleService = timeScaleService;
    }
    
    public void PauseGame()
    {
        _timeScaleService.NullifyUnityTimeScale();
        _timeScaleService.NullifyPhysicTimeScale();
    }

    public void ResumeGame()
    {
        _timeScaleService.ResetPhysicTimeScale();
        _timeScaleService.ResetUnityTimeScale();
    }
}
