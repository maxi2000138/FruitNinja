using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class FreezeController : MonoBehaviour, ILooseGameListener
{
    [SerializeField] private FreezeView _freezeView;
    
    private TimeScaleService _timeScaleService;
    private TokenController _tokenController;
    private CancellationToken _currentToken;

    public void Construct(TimeScaleService timeScaleService)
    {
        _tokenController = new TokenController();
        _timeScaleService = timeScaleService;
    }

    public void OnLooseGame()
    {
        _tokenController.CancelTokens();
        RemoveFrozen();
    }

    private void OnDestroy()
    {
        _tokenController.CancelTokens();
    }

    public async Task SetFrozeEffect(float secondsTime)
    {
        _tokenController.CancelToken(_currentToken);
        SetFrozen();
        _currentToken = _tokenController.CreateCancellationToken();
        bool canceled = await UniTask.Delay((int)(secondsTime * 1000), DelayType.DeltaTime, PlayerLoopTiming.Update, _currentToken)
            .SuppressCancellationThrow();
        if(canceled)
            return;
        RemoveFrozen();
    }

    private void SetFrozen()
    {        
        _freezeView.Show();
        _timeScaleService.SetPhysicFrozenTimeScale();
    }

    private void RemoveFrozen()
    {
        _freezeView.Hide();
        _timeScaleService.ResetPhysicTimeScale();
    }
}
