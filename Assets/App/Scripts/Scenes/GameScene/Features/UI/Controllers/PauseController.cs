using App.Scripts.Scenes.GameScene.Features.InputFeatures;

public class PauseController
{
    private readonly TimeScaleService _timeScaleService;
    private readonly Slicer _slicer;
    private float _previousPhysicTimeScale;
    private float _previousUnityTimeScale;
    public PauseController(TimeScaleService timeScaleService, Slicer slicer)
    {
        _timeScaleService = timeScaleService;
        _slicer = slicer;
    }
    
    public void PauseGame()
    {
        _previousPhysicTimeScale = _timeScaleService.PhysicTimeScale;
        _previousUnityTimeScale = _timeScaleService.UnityTimeScale;
        _timeScaleService.NullifyUnityTimeScale();
        _timeScaleService.NullifyPhysicTimeScale();
        _slicer.Disable();
    }

    public void ResumeGame()
    {
        _timeScaleService.SetPhysicTimeScale(_previousPhysicTimeScale);
        _timeScaleService.SetUnityTimeScale(_previousUnityTimeScale);
        _slicer.Enable();
    }
}
